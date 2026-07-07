using CarServiceApi.Data;
using CarServiceApi.DTOs;
using CarServiceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CarServiceApi.Services
{
    public class ServiceLogService : IServiceLogService
    {

        private readonly ApplicationDbContext _context;

        public ServiceLogService(ApplicationDbContext context)
        {
            _context = context;
        }



        public async Task AddServiceLogAsync(ServiceLogCreateDto request)
        {
            var vehicleExists = await _context.Vehicles.AnyAsync(v => v.Id == request.VehicleId);
            if (!vehicleExists) throw new KeyNotFoundException("The specified vehicle was not found.");

            var serviceLog = new ServiceLog
            {
                VehicleId = request.VehicleId,
                Date = request.Date,
                CarKmCount = request.CarKmCount,
                ServiceDescription = request.ServiceDescription,
                ServiceCost = request.ServiceCost
            };

            await _context.ServiceLogs.AddAsync(serviceLog);
            await _context.SaveChangesAsync();
        }



        public async Task DeleteServiceLogAsync(int id)
        {
            var log = await _context.ServiceLogs.FindAsync(id);
            if (log == null) throw new KeyNotFoundException("Service log not found.");

            _context.ServiceLogs.Remove(log);
            await _context.SaveChangesAsync();
        }



        public async Task<List<object>> GetServiceLogsForVehicleAsync(int vehicleId)
        {
            var logs = await _context.ServiceLogs
                .AsNoTracking()
                .Where(s => s.VehicleId == vehicleId)
                .OrderByDescending(s => s.Date)
                .Select(s => new
                {
                    s.Id,
                    s.Date,
                    s.CarKmCount,
                    s.ServiceDescription,
                    s.ServiceCost
                }).ToListAsync();

            return logs.Cast<object>().ToList();
        }



        public async Task UpdateServiceLogAsync(int id, ServiceLogCreateDto request)
        {
            var log = await _context.ServiceLogs.FindAsync(id);
            if (log == null) throw new KeyNotFoundException("Service log not found.");

            log.Date = request.Date;
            log.CarKmCount = request.CarKmCount;
            log.ServiceDescription = request.ServiceDescription;
            log.ServiceCost = request.ServiceCost;

            await _context.SaveChangesAsync();
        }
    }
}
