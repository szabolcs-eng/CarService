using CarServiceApi.DTOs;

namespace CarServiceApi.Services
{
    public interface IFuelLogService
    {
        Task AddFuelLogAsync(FuelLogCreateDto request);
        Task <List<object>> GetFuelLogsForVehicleAsync(int vehicleId);
        Task UpdateFuelLogAsync(int fuelLogId, FuelLogCreateDto request);
        Task DeleteFuelLogAsync(int fuelLogId);
        Task<object> GetAverageFuelConsumptionAsync(int vehicleId);
    }
}
