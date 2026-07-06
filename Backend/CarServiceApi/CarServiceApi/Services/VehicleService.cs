using CarServiceApi.Data;
using CarServiceApi.DTOs;
using CarServiceApi.Models;
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

        public async Task<List<object>> GetUserVehiclesAsync(int userId)
        {
            var vehicles = await _context.Vehicles
                .AsNoTracking()
                .Where(v => v.UserId == userId)
                .Select(v => new
                {
                    v.Id,
                    v.LicensePlate,
                    v.Brand,
                    v.Model,
                    v.Year
                }).ToListAsync();

            return vehicles.Cast<object>().ToList();
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
