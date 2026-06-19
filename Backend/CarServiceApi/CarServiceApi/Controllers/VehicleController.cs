using Microsoft.AspNetCore.Mvc;
using CarServiceApi.Data;
using CarServiceApi.Models;
using CarServiceApi.DTOs;

namespace CarServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VehicleController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("add")]
        public IActionResult AddVehicle(VehicleCreateDto request)
        {
            var vehicle = new Vehicle
            {
                UserId = request.UserId,
                LicensePlate = request.LicensePlate,
                Brand = request.Brand,
                Model = request.Model,
                Year = request.Year
            };

            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();

            return Ok("Vehicle successfully added to the profile!");
        }

        [HttpGet("user-vehicles/{userId}")]
        public IActionResult GetUserVehicles(int userId)
        {
            var vehicles = _context.Vehicles
                .Where(v => v.UserId == userId)
                .Select(v => new
                {
                    v.Id,
                    v.LicensePlate,
                    v.Brand,
                    v.Model,
                    v.Year
                }).ToList();

            return Ok(vehicles);
        }

        [HttpPut("update/{id}")]
        public IActionResult UpdateVehicle(int id, VehicleCreateDto request)
        {
            var vehicle = _context.Vehicles.Find(id);
            if (vehicle == null) return NotFound("Vehicle not found.");

            vehicle.LicensePlate = request.LicensePlate;
            vehicle.Brand = request.Brand;
            vehicle.Model = request.Model;
            vehicle.Year = request.Year;

            _context.SaveChanges();
            return Ok("Vehicle details successfully updated!");
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteVehicle(int id)
        {
            var vehicle = _context.Vehicles.Find(id);
            if (vehicle == null) return NotFound("Vehicle not found.");

            _context.Vehicles.Remove(vehicle);
            _context.SaveChanges();
            return Ok("Vehicle (and all associated logs) successfully deleted!");
        }
    }
}