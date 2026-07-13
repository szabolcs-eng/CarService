using CarServiceApi.DTOs;
using CarServiceApi.Filters;
using CarServiceApi.Wrappers;

namespace CarServiceApi.Services
{
    public interface IFuelLogService
    {
        Task AddFuelLogAsync(FuelLogCreateDto request);
        Task<PagedResponse<List<FuelLogResponseDto>>> GetFuelLogsForVehicleAsync(int vehicleId, PaginationFilter filter);
        Task UpdateFuelLogAsync(int fuelLogId, FuelLogCreateDto request);
        Task DeleteFuelLogAsync(int fuelLogId);
        Task<object> GetAverageFuelConsumptionAsync(int vehicleId);
    }
}
