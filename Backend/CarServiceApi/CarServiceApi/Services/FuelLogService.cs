using CarServiceApi.Data;
using CarServiceApi.DTOs;
using CarServiceApi.Models;
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



        public async Task AddFuelLogAsync(FuelLogCreateDto request)
        {
            var vehicleExists = await _context.Vehicles.AnyAsync(v => v.Id == request.VehicleId);
            if (!vehicleExists) throw new KeyNotFoundException("The specified vehicle was not found.");

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

        public async Task DeleteFuelLogAsync(int fuelLogId)
        {
            var log = await _context.FuelLogs.FindAsync(fuelLogId);
            if (log == null) throw new KeyNotFoundException("Fuel log not found.");

            _context.FuelLogs.Remove(log);
            await _context.SaveChangesAsync();
        }

        public async Task<object> GetAverageFuelConsumptionAsync(int vehicleId)
        {
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

        public async Task<List<object>> GetFuelLogsForVehicleAsync(int vehicleId)
        {
            var logs =  await _context.FuelLogs
                .AsNoTracking()
                .Where(f => f.VehicleId == vehicleId)
                .OrderByDescending(f => f.Date)
                .Select(f => new
                {
                    f.Id,
                    f.Date,
                    f.CarKmCount,
                    f.FuelAmount,
                    f.FuelCost
                }).ToListAsync();

            return logs.Cast<object>().ToList();
        }

        public async Task UpdateFuelLogAsync(int fuelLogId, FuelLogCreateDto request)
        {
            var log = await _context.FuelLogs.FindAsync(fuelLogId);
            if (log == null) throw new KeyNotFoundException("Fuel log not found.");

            log.Date = request.Date;
            log.CarKmCount = request.CarKmCount;
            log.FuelAmount = request.FuelAmount;
            log.FuelCost = request.FuelCost;

            await _context.SaveChangesAsync();
        }
    }
}
