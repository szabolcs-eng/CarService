using CarServiceApi.DTOs;

namespace CarServiceApi.Services
{
    public interface IVehicleService
    {
        Task AddVehicleAsync(VehicleCreateDto request);
        Task<List<object>> GetUserVehiclesAsync(int userId);
        Task UpdateVehicleAsync(int vehicleId, VehicleCreateDto request);
        Task DeleteVehicleAsync(int vehicleId);
    }
}
