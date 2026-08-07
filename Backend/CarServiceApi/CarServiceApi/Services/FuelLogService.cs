using CarServiceApi.Data;
using CarServiceApi.DTOs;
using CarServiceApi.Exceptions;
using CarServiceApi.Filters;
using CarServiceApi.Models;
using CarServiceApi.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace CarServiceApi.Services
{
    public class FuelLogService : IFuelLogService
    {
        private readonly ApplicationDbContext _context;

        public FuelLogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddFuelLogAsync(FuelLogCreateDto request, int requestingUserId)
        {
            var vehicle = await _context.Vehicles.FindAsync(request.VehicleId);
            if (vehicle == null) throw new KeyNotFoundException("The specified vehicle was not found.");
            VehicleService.EnsureOwnedBy(vehicle, requestingUserId);

            var fuelLog = new FuelLog
            {
                VehicleId = request.VehicleId,
                Date = request.Date,
                CarKmCount = request.CarKmCount,
                FuelAmount = request.FuelAmount,
                FuelCost = request.FuelCost
            };

            await _context.FuelLogs.AddAsync(fuelLog);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFuelLogAsync(int fuelLogId, int requestingUserId)
        {
            var log = await _context.FuelLogs.Include(f => f.Vehicle).FirstOrDefaultAsync(f => f.Id == fuelLogId);
            if (log?.Vehicle == null) throw new KeyNotFoundException("Fuel log not found.");
            VehicleService.EnsureOwnedBy(log.Vehicle, requestingUserId);

            _context.FuelLogs.Remove(log);
            await _context.SaveChangesAsync();
        }

        public async Task<object> GetAverageFuelConsumptionAsync(int vehicleId, int requestingUserId)
        {
            await EnsureVehicleOwnedByAsync(vehicleId, requestingUserId);

            var logs = await _context.FuelLogs
                .Where(f => f.VehicleId == vehicleId)
                .OrderBy(f => f.CarKmCount)
                .ToListAsync();

            if (logs.Count < 2) throw new InvalidOperationException("At least two fuel logs are required to calculate average consumption.");

            var firstLog = logs.First();
            var lastLog = logs.Last();
            int totalDistance = lastLog.CarKmCount - firstLog.CarKmCount;

            if (totalDistance <= 0) throw new InvalidOperationException("No distance covered based on the odometer.");

            double totalFuelUsed = logs.Skip(1).Sum(f => f.FuelAmount);
            double averageConsumption = (totalFuelUsed / totalDistance) * 100;

            return new
            {
                VehicleId = vehicleId,
                TotalDistanceKm = totalDistance,
                TotalFuelUsedLiters = totalFuelUsed,
                AverageConsumption = Math.Round(averageConsumption, 2)
            };
        }

        public async Task<PagedResponse<List<FuelLogResponseDto>>> GetFuelLogsForVehicleAsync(int vehicleId, PaginationFilter filter, int requestingUserId)
        {
            await EnsureVehicleOwnedByAsync(vehicleId, requestingUserId);

            var query = _context.FuelLogs
                .AsNoTracking()
                .Where(s => s.VehicleId == vehicleId);

            int totalRecords = await query.CountAsync();

            var logs = await query
                .OrderByDescending(s => s.Date)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(s => new FuelLogResponseDto(
                    s.Id,
                    s.VehicleId,
                    s.Date,
                    s.CarKmCount,
                    s.FuelAmount,
                    s.FuelCost
                )).ToListAsync();

            return new PagedResponse<List<FuelLogResponseDto>>(logs, filter.PageNumber, filter.PageSize, totalRecords);
        }

        public async Task UpdateFuelLogAsync(int fuelLogId, FuelLogCreateDto request, int requestingUserId)
        {
            var log = await _context.FuelLogs.Include(f => f.Vehicle).FirstOrDefaultAsync(f => f.Id == fuelLogId);
            if (log?.Vehicle == null) throw new KeyNotFoundException("Fuel log not found.");
            VehicleService.EnsureOwnedBy(log.Vehicle, requestingUserId);

            log.Date = request.Date;
            log.CarKmCount = request.CarKmCount;
            log.FuelAmount = request.FuelAmount;
            log.FuelCost = request.FuelCost;

            await _context.SaveChangesAsync();
        }

        private async Task EnsureVehicleOwnedByAsync(int vehicleId, int requestingUserId)
        {
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);
            if (vehicle == null) throw new KeyNotFoundException("The specified vehicle was not found.");
            VehicleService.EnsureOwnedBy(vehicle, requestingUserId);
        }
    }
}
