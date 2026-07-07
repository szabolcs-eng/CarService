using CarServiceApi.Data;
using CarServiceApi.DTOs;
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



        public async Task AddVehicleAsync(VehicleCreateDto request)
        {
            var vehicle = new Vehicle
            {
                UserId = request.UserId,
                LicensePlate = request.LicensePlate,
                Brand = request.Brand,
                Model = request.Model,
                Year = request.Year
            };

            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();

        }



        public async Task DeleteVehicleAsync(int vehicleId)
        {
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);
            if (vehicle == null) throw new KeyNotFoundException("Vehicle not found.");

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
        }



        public async Task<PagedResponse<List<object>>> GetUserVehiclesAsync(int userId, PaginationFilter filter)
        {
            var query = _context.Vehicles
                .AsNoTracking()
                .Where(v => v.UserId == userId);

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
                .Select(v => new
                {
                    v.Id,
                    v.LicensePlate,
                    v.Brand,
                    v.Model,
                    v.Year
                }).ToListAsync();

            var data = vehicles.Cast<object>().ToList();

            return new PagedResponse<List<object>>(data, filter.PageNumber, filter.PageSize, totalRecords);
        }



        public async Task UpdateVehicleAsync(int vehicleId, VehicleCreateDto request)
        {
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);
            if (vehicle == null) throw new KeyNotFoundException("Vehicle not found.");

            vehicle.LicensePlate = request.LicensePlate;
            vehicle.Brand = request.Brand;
            vehicle.Model = request.Model;
            vehicle.Year = request.Year;

            await _context.SaveChangesAsync();
        }
    }
}
