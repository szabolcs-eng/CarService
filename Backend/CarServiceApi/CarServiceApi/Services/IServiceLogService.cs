using CarServiceApi.DTOs;
using CarServiceApi.Filters;
using CarServiceApi.Wrappers;

namespace CarServiceApi.Services
{
    public interface IServiceLogService
    {
        Task AddServiceLogAsync(ServiceLogCreateDto request);
        Task<PagedResponse<List<ServiceLogResponseDto>>> GetServiceLogsForVehicleAsync(int vehicleId, PaginationFilter filter);
        Task UpdateServiceLogAsync(int id, ServiceLogCreateDto request);
        Task DeleteServiceLogAsync(int id);
    }
}
