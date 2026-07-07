using Microsoft.AspNetCore.Mvc;
using CarServiceApi.DTOs;
using CarServiceApi.Services;

namespace CarServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceLogController : ControllerBase
    {
        private readonly IServiceLogService _serviceLogService;

        public ServiceLogController(IServiceLogService serviceLogService)
        {
            _serviceLogService = serviceLogService;
        }



        [HttpPost("add")]
        public async Task<IActionResult> AddServiceLog(ServiceLogCreateDto request)
        {
            try
            {
                await _serviceLogService.AddServiceLogAsync(request);
                return Ok("Service log successfully added!");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }



        [HttpGet("vehicle/{vehicleId}")]
        public async Task<IActionResult> GetServiceLogsForVehicle(int vehicleId)
        {
            var logs = await _serviceLogService.GetServiceLogsForVehicleAsync(vehicleId);
            return Ok(logs);
        }



        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateServiceLog(int id, ServiceLogCreateDto request)
        {
            try
            {
                await _serviceLogService.UpdateServiceLogAsync(id, request);
                return Ok("Service log successfully updated!");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }



        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteServiceLog(int id)
        {
            try
            {
                _serviceLogService.DeleteServiceLogAsync(id);
                return Ok("Service log successfully deleted!");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}