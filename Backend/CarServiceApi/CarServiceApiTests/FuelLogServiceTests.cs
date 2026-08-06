using CarServiceApi.Data;
using CarServiceApi.DTOs;
using CarServiceApi.Exceptions;
using CarServiceApi.Filters;
using CarServiceApi.Models;
using CarServiceApi.Services;
using Microsoft.EntityFrameworkCore;

namespace CarServiceApi.Tests
{
    public class FuelLogServiceTests
    {
        private static ApplicationDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        private static async Task<(ApplicationDbContext Context, Vehicle OwnerVehicle)> SeedVehicleAsync(int ownerId = 1)
        {
            var context = CreateInMemoryContext();
            var vehicle = new Vehicle { Id = 1, UserId = ownerId, Brand = "Toyota", Model = "Corolla", LicensePlate = "ABC-123", Year = 2020 };
            context.Vehicles.Add(vehicle);
            await context.SaveChangesAsync();
            return (context, vehicle);
        }

        [Fact]
        public async Task GetAverageFuelConsumptionAsync_LessThanTwoLogs_Throws()
        {
            var (context, vehicle) = await SeedVehicleAsync();
            context.FuelLogs.Add(new FuelLog { VehicleId = vehicle.Id, CarKmCount = 100000, FuelAmount = 50 });
            await context.SaveChangesAsync();
            var service = new FuelLogService(context);

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.GetAverageFuelConsumptionAsync(vehicle.Id, vehicle.UserId));
        }

        [Fact]
        public async Task GetAverageFuelConsumptionAsync_NoDistanceCovered_Throws()
        {
            var (context, vehicle) = await SeedVehicleAsync();
            context.FuelLogs.AddRange(
                new FuelLog { VehicleId = vehicle.Id, CarKmCount = 100000, FuelAmount = 50 },
                new FuelLog { VehicleId = vehicle.Id, CarKmCount = 100000, FuelAmount = 40 });
            await context.SaveChangesAsync();
            var service = new FuelLogService(context);

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.GetAverageFuelConsumptionAsync(vehicle.Id, vehicle.UserId));
        }

        [Fact]
        public async Task GetAverageFuelConsumptionAsync_ValidData_ReturnsExpectedAverage()
        {
            var (context, vehicle) = await SeedVehicleAsync();
            context.FuelLogs.AddRange(
                new FuelLog { VehicleId = vehicle.Id, CarKmCount = 100000, FuelAmount = 50 },
                new FuelLog { VehicleId = vehicle.Id, CarKmCount = 100500, FuelAmount = 40 });
            await context.SaveChangesAsync();
            var service = new FuelLogService(context);

            var result = await service.GetAverageFuelConsumptionAsync(vehicle.Id, vehicle.UserId);

            var averageConsumption = result.GetType().GetProperty("AverageConsumption")!.GetValue(result);
            var totalDistance = result.GetType().GetProperty("TotalDistanceKm")!.GetValue(result);

            Assert.Equal(500, totalDistance);
            Assert.Equal(8.0, averageConsumption);
        }

        [Fact]
        public async Task GetAverageFuelConsumptionAsync_VehicleBelongsToAnotherUser_ThrowsForbidden()
        {
            var (context, vehicle) = await SeedVehicleAsync(ownerId: 1);
            context.FuelLogs.AddRange(
                new FuelLog { VehicleId = vehicle.Id, CarKmCount = 100000, FuelAmount = 50 },
                new FuelLog { VehicleId = vehicle.Id, CarKmCount = 100500, FuelAmount = 40 });
            await context.SaveChangesAsync();
            var service = new FuelLogService(context);

            const int someoneElsesUserId = 2;

            // This is the regression test for the IDOR fix: a different user must
            // never be able to read this vehicle's fuel logs, even if they know its id.
            await Assert.ThrowsAsync<ForbiddenAccessException>(
                () => service.GetAverageFuelConsumptionAsync(vehicle.Id, someoneElsesUserId));
        }

        [Fact]
        public async Task AddFuelLogAsync_VehicleBelongsToAnotherUser_ThrowsForbidden()
        {
            var (context, vehicle) = await SeedVehicleAsync(ownerId: 1);
            var service = new FuelLogService(context);

            var request = new FuelLogCreateDto
            {
                VehicleId = vehicle.Id,
                Date = DateTime.UtcNow,
                CarKmCount = 100000,
                FuelAmount = 40,
                FuelCost = 25000
            };

            await Assert.ThrowsAsync<ForbiddenAccessException>(
                () => service.AddFuelLogAsync(request, requestingUserId: 2));
        }

        [Fact]
        public async Task AddFuelLogAsync_VehicleDoesNotExist_ThrowsKeyNotFound()
        {
            var context = CreateInMemoryContext();
            var service = new FuelLogService(context);

            var request = new FuelLogCreateDto { VehicleId = 999, Date = DateTime.UtcNow, CarKmCount = 100, FuelAmount = 10, FuelCost = 5000 };

            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => service.AddFuelLogAsync(request, requestingUserId: 1));
        }
    }
}
