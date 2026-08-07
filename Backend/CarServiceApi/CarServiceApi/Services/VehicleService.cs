using CarServiceApi.Data;
using CarServiceApi.DTOs;
using CarServiceApi.Exceptions;
using CarServiceApi.Filters;
using CarServiceApi.Models;
using CarServiceApi.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace CarServiceApi.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly ApplicationDbContext _context;

        public VehicleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddVehicleAsync(VehicleCreateDto request, int ownerUserId)
        {
            var vehicle = new Vehicle
            {
                UserId = ownerUserId,
                LicensePlate = request.LicensePlate,
                Brand = request.Brand,
                Model = request.Model,
                Year = request.Year,
                TechnicalInspectionExpiry = request.TechnicalInspectionExpiry
            };

            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVehicleAsync(int vehicleId, int requestingUserId)
        {
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);
            if (vehicle == null) throw new KeyNotFoundException("Vehicle not found.");

            EnsureOwnedBy(vehicle, requestingUserId);

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResponse<List<VehicleResponseDto>>> GetUserVehiclesAsync(int requestingUserId, PaginationFilter filter)
        {
            // requestingUserId always comes from the JWT (see VehicleController),
            // so this inherently only ever returns the caller's own vehicles.
            var query = _context.Vehicles
                .AsNoTracking()
                .Where(v => v.UserId == requestingUserId);

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.ToLower();
                query = query.Where(v =>
                    v.LicensePlate.ToLower().Contains(searchTerm) ||
                    v.Brand.ToLower().Contains(searchTerm) ||
                    v.Model.ToLower().Contains(searchTerm));
            }

            int totalRecords = await query.CountAsync();

            var vehicles = await query
                .OrderBy(v => v.Brand).ThenBy(v => v.Model)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(v => new VehicleResponseDto(
                    v.Id,
                    v.UserId,
                    v.LicensePlate,
                    v.Brand,
                    v.Model,
                    v.Year,
                    v.TechnicalInspectionExpiry
                )).ToListAsync();

            return new PagedResponse<List<VehicleResponseDto>>(vehicles, filter.PageNumber, filter.PageSize, totalRecords);
        }

        public async Task UpdateVehicleAsync(int vehicleId, VehicleCreateDto request, int requestingUserId)
        {
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);
            if (vehicle == null) throw new KeyNotFoundException("Vehicle not found.");

            EnsureOwnedBy(vehicle, requestingUserId);

            vehicle.LicensePlate = request.LicensePlate;
            vehicle.Brand = request.Brand;
            vehicle.Model = request.Model;
            vehicle.Year = request.Year;
            vehicle.TechnicalInspectionExpiry = request.TechnicalInspectionExpiry;

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Confirms the vehicle belongs to the requesting user before any read/write.
        /// This is the check that was previously missing entirely - without it, any
        /// authenticated user could act on any vehicle by guessing its id.
        /// </summary>
        internal static void EnsureOwnedBy(Vehicle vehicle, int requestingUserId)
        {
            if (vehicle.UserId != requestingUserId)
            {
                throw new ForbiddenAccessException("You do not have access to this vehicle.");
            }
        }
    }
}
