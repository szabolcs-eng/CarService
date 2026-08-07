using CarServiceApi.Data;
using CarServiceApi.DTOs;
using CarServiceApi.Filters;
using CarServiceApi.Models;
using CarServiceApi.Wrappers;
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

        public async Task AddServiceLogAsync(ServiceLogCreateDto request, int requestingUserId)
        {
            var vehicle = await _context.Vehicles.FindAsync(request.VehicleId);
            if (vehicle == null) throw new KeyNotFoundException("The specified vehicle was not found.");
            VehicleService.EnsureOwnedBy(vehicle, requestingUserId);

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

        public async Task DeleteServiceLogAsync(int id, int requestingUserId)
        {
            var log = await _context.ServiceLogs.Include(s => s.Vehicle).FirstOrDefaultAsync(s => s.Id == id);
            if (log?.Vehicle == null) throw new KeyNotFoundException("Service log not found.");
            VehicleService.EnsureOwnedBy(log.Vehicle, requestingUserId);

            _context.ServiceLogs.Remove(log);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResponse<List<ServiceLogResponseDto>>> GetServiceLogsForVehicleAsync(int vehicleId, PaginationFilter filter, int requestingUserId)
        {
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);
            if (vehicle == null) throw new KeyNotFoundException("The specified vehicle was not found.");
            VehicleService.EnsureOwnedBy(vehicle, requestingUserId);

            var query = _context.ServiceLogs
                .AsNoTracking()
                .Where(s => s.VehicleId == vehicleId);

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                query = query.Where(s => s.ServiceDescription.ToLower().Contains(filter.SearchTerm.ToLower()));
            }

            int totalRecords = await query.CountAsync();

            var logs = await query
                .OrderByDescending(s => s.Date)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(s => new ServiceLogResponseDto(
                    s.Id,
                    s.VehicleId,
                    s.Date,
                    s.CarKmCount,
                    s.ServiceDescription,
                    s.ServiceCost
                )).ToListAsync();

            return new PagedResponse<List<ServiceLogResponseDto>>(logs, filter.PageNumber, filter.PageSize, totalRecords);
        }

        public async Task UpdateServiceLogAsync(int id, ServiceLogCreateDto request, int requestingUserId)
        {
            var log = await _context.ServiceLogs.Include(s => s.Vehicle).FirstOrDefaultAsync(s => s.Id == id);
            if (log?.Vehicle == null) throw new KeyNotFoundException("Service log not found.");
            VehicleService.EnsureOwnedBy(log.Vehicle, requestingUserId);

            log.Date = request.Date;
            log.CarKmCount = request.CarKmCount;
            log.ServiceDescription = request.ServiceDescription;
            log.ServiceCost = request.ServiceCost;

            await _context.SaveChangesAsync();
        }
    }
}
