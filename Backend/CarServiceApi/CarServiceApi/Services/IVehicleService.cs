using CarServiceApi.DTOs;
using CarServiceApi.Filters;
using CarServiceApi.Wrappers;

namespace CarServiceApi.Services
{
    public interface IVehicleService
    {
        Task AddVehicleAsync(VehicleCreateDto request);
        Task<PagedResponse<List<object>>> GetUserVehiclesAsync(int userId, PaginationFilter filter);
        Task UpdateVehicleAsync(int vehicleId, VehicleCreateDto request);
        Task DeleteVehicleAsync(int vehicleId);
    }
}
