using CarServiceApi.DTOs;

namespace CarServiceApi.Services
{
    public interface IServiceLogService
    {
        Task AddServiceLogAsync(ServiceLogCreateDto request);
        Task<List<object>> GetServiceLogsForVehicleAsync(int vehicleId);
        Task UpdateServiceLogAsync(int id, ServiceLogCreateDto request);
        Task DeleteServiceLogAsync(int id);
    }
}
