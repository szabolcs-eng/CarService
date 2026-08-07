using CarServiceApi.DTOs;
using CarServiceApi.Filters;
using CarServiceApi.Wrappers;

namespace CarServiceApi.Services
{
    public interface IVehicleService
    {
        Task AddVehicleAsync(VehicleCreateDto request, int ownerUserId);
        Task<PagedResponse<List<VehicleResponseDto>>> GetUserVehiclesAsync(int requestingUserId, PaginationFilter filter);
        Task UpdateVehicleAsync(int vehicleId, VehicleCreateDto request, int requestingUserId);
        Task DeleteVehicleAsync(int vehicleId, int requestingUserId);
    }
}
