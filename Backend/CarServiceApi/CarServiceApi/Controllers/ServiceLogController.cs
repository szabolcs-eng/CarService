using CarServiceApi.DTOs;
using CarServiceApi.Filters;
using CarServiceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ServiceLogController : BaseApiController
    {
        private readonly IServiceLogService _serviceLogService;

        public ServiceLogController(IServiceLogService serviceLogService)
        {
            _serviceLogService = serviceLogService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddServiceLog(ServiceLogCreateDto request)
        {
            await _serviceLogService.AddServiceLogAsync(request, CurrentUserId);
            return Ok("Service log successfully added!");
        }

        [HttpGet("vehicle/{vehicleId}")]
        public async Task<IActionResult> GetServiceLogsForVehicle(int vehicleId, [FromQuery] PaginationFilter filter)
        {
            var response = await _serviceLogService.GetServiceLogsForVehicleAsync(vehicleId, filter, CurrentUserId);
            return Ok(response);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateServiceLog(int id, ServiceLogCreateDto request)
        {
            await _serviceLogService.UpdateServiceLogAsync(id, request, CurrentUserId);
            return Ok("Service log successfully updated!");
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteServiceLog(int id)
        {
            await _serviceLogService.DeleteServiceLogAsync(id, CurrentUserId);
            return Ok("Service log successfully deleted!");
        }
    }
}
