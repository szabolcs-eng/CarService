using Microsoft.AspNetCore.Mvc;
using CarServiceApi.Data;
using CarServiceApi.Models;
using CarServiceApi.DTOs;

namespace CarServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuelLogController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FuelLogController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("add")]
        public IActionResult AddFuelLog(FuelLogCreateDto request)
        {
            // Ellenőrizzük, hogy létezik-e az autó
            var vehicleExists = _context.Vehicles.Any(v => v.Id == request.VehicleId);
            if (!vehicleExists)
            {
                return NotFound("A megadott jármű nem található.");
            }

            var fuelLog = new FuelLog
            {
                VehicleId = request.VehicleId,
                Date = request.Date,
                CarKmCount = request.CarKmCount,
                FuelAmount = request.FuelAmount,
                FuelCost = request.FuelCost
            };

            _context.FuelLogs.Add(fuelLog);
            _context.SaveChanges();

            return Ok("A tankolás sikeresen rögzítve!");
        }

        [HttpGet("vehicle/{vehicleId}")]
        public IActionResult GetFuelLogsForVehicle(int vehicleId)
        {
            var logs = _context.FuelLogs
                .Where(f => f.VehicleId == vehicleId)
                .OrderByDescending(f => f.Date)
                .Select(f => new
                {
                    f.Id,
                    f.Date,
                    f.CarKmCount,
                    f.FuelAmount,
                    f.FuelCost
                }).ToList();

            return Ok(logs);
        }

        [HttpPut("update/{id}")]
        public IActionResult UpdateFuelLog(int id, FuelLogCreateDto request)
        {
            var log = _context.FuelLogs.Find(id);
            if (log == null) return NotFound("A tankolás nem található.");

            log.Date = request.Date;
            log.CarKmCount = request.CarKmCount;
            log.FuelAmount = request.FuelAmount;
            log.FuelCost = request.FuelCost;

            _context.SaveChanges();
            return Ok("A tankolás adatai frissítve!");
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteFuelLog(int id)
        {
            var log = _context.FuelLogs.Find(id);
            if (log == null) return NotFound("A tankolás nem található.");

            _context.FuelLogs.Remove(log);
            _context.SaveChanges();
            return Ok("A tankolás törölve!");
        }

        [HttpGet("vehicle/{vehicleId}/average-consumption")]
        public IActionResult GetAverageConsumption(int vehicleId)
        {
            var logs = _context.FuelLogs
                .Where(f => f.VehicleId == vehicleId)
                .OrderBy(f => f.CarKmCount)
                .ToList();

            if (logs.Count < 2)
            {
                return BadRequest("Legalább két tankolás rögzítése szükséges az átlagfogyasztás kiszámításához.");
            }

            var firstLog = logs.First();
            var lastLog = logs.Last();
            int totalDistance = lastLog.CarKmCount - firstLog.CarKmCount;

            if (totalDistance <= 0)
            {
                return BadRequest("A kilométeróra állások alapján nem történt haladás.");
            }

            double totalFuelUsed = logs.Skip(1).Sum(f => f.FuelAmount);

            double averageConsumption = (totalFuelUsed / totalDistance) * 100;

            return Ok(new
            {
                VehicleId = vehicleId,
                TotalDistanceKm = totalDistance,
                TotalFuelUsedLiters = totalFuelUsed,
                AverageConsumption = Math.Round(averageConsumption, 2)
            });
        }
    }
}