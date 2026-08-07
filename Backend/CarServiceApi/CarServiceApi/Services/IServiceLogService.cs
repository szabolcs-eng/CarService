using CarServiceApi.DTOs;
using CarServiceApi.Filters;
using CarServiceApi.Wrappers;

namespace CarServiceApi.Services
{
    public interface IServiceLogService
    {
        Task AddServiceLogAsync(ServiceLogCreateDto request, int requestingUserId);
        Task<PagedResponse<List<ServiceLogResponseDto>>> GetServiceLogsForVehicleAsync(int vehicleId, PaginationFilter filter, int requestingUserId);
        Task UpdateServiceLogAsync(int id, ServiceLogCreateDto request, int requestingUserId);
        Task DeleteServiceLogAsync(int id, int requestingUserId);
    }
}
