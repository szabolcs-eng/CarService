using CarServiceApi.DTOs;
using CarServiceApi.Filters;
using CarServiceApi.Wrappers;

namespace CarServiceApi.Services
{
    public interface IFuelLogService
    {
        Task AddFuelLogAsync(FuelLogCreateDto request, int requestingUserId);
        Task<PagedResponse<List<FuelLogResponseDto>>> GetFuelLogsForVehicleAsync(int vehicleId, PaginationFilter filter, int requestingUserId);
        Task UpdateFuelLogAsync(int fuelLogId, FuelLogCreateDto request, int requestingUserId);
        Task DeleteFuelLogAsync(int fuelLogId, int requestingUserId);
        Task<object> GetAverageFuelConsumptionAsync(int vehicleId, int requestingUserId);
    }
}
