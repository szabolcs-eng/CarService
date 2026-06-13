using Microsoft.AspNetCore.Mvc;
using CarServiceApi.Data;
using CarServiceApi.Models;
using CarServiceApi.DTOs;

namespace CarServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceLogController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ServiceLogController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("add")]
        public IActionResult AddServiceLog(ServiceLogCreateDto request)
        {
            var vehicleExists = _context.Vehicles.Any(v => v.Id == request.VehicleId);
            if (!vehicleExists)
            {
                return NotFound("A megadott jármű nem található.");
            }

            var serviceLog = new ServiceLog
            {
                VehicleId = request.VehicleId,
                Date = request.Date,
                CarKmCount = request.CarKmCount,
                ServiceDescription = request.ServiceDescription,
                ServiceCost = request.ServiceCost
            };

            _context.ServiceLogs.Add(serviceLog);
            _context.SaveChanges();

            return Ok("A szervizbejegyzés sikeresen rögzítve!");
        }

        [HttpGet("vehicle/{vehicleId}")]
        public IActionResult GetServiceLogsForVehicle(int vehicleId)
        {
            var logs = _context.ServiceLogs
                .Where(s => s.VehicleId == vehicleId)
                .OrderByDescending(s => s.Date)
                .Select(s => new
                {
                    s.Id,
                    s.Date,
                    s.CarKmCount,
                    s.ServiceDescription,
                    s.ServiceCost
                }).ToList();

            return Ok(logs);
        }

        [HttpPut("update/{id}")]
        public IActionResult UpdateServiceLog(int id, ServiceLogCreateDto request)
        {
            var log = _context.ServiceLogs.Find(id);
            if (log == null) return NotFound("A bejegyzés nem található.");

            log.Date = request.Date;
            log.CarKmCount = request.CarKmCount;
            log.ServiceDescription = request.ServiceDescription;
            log.ServiceCost = request.ServiceCost;

            _context.SaveChanges();
            return Ok("A szervizbejegyzés sikeresen frissítve!");
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteServiceLog(int id)
        {
            var log = _context.ServiceLogs.Find(id);
            if (log == null) return NotFound("A bejegyzés nem található.");

            _context.ServiceLogs.Remove(log);
            _context.SaveChanges();
            return Ok("A szervizbejegyzés törölve!");
        }
    }
}