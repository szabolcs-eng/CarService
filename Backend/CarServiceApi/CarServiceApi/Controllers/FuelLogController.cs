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
    public class FuelLogController : BaseApiController
    {
        private readonly IFuelLogService _fuelLogService;

        public FuelLogController(IFuelLogService fuelLogService)
        {
            _fuelLogService = fuelLogService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddFuelLog(FuelLogCreateDto request)
        {
            await _fuelLogService.AddFuelLogAsync(request, CurrentUserId);
            return Ok("Fuel log successfully added!");
        }

        [HttpGet("vehicle/{vehicleId}")]
        public async Task<IActionResult> GetFuelLogsForVehicle(int vehicleId, [FromQuery] PaginationFilter filter)
        {
            var response = await _fuelLogService.GetFuelLogsForVehicleAsync(vehicleId, filter, CurrentUserId);
            return Ok(response);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateFuelLog(int id, FuelLogCreateDto request)
        {
            await _fuelLogService.UpdateFuelLogAsync(id, request, CurrentUserId);
            return Ok("Fuel log details successfully updated!");
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteFuelLog(int id)
        {
            await _fuelLogService.DeleteFuelLogAsync(id, CurrentUserId);
            return Ok("Fuel log successfully deleted!");
        }

        [HttpGet("vehicle/{vehicleId}/average-consumption")]
        public async Task<IActionResult> GetAverageConsumption(int vehicleId)
        {
            var result = await _fuelLogService.GetAverageFuelConsumptionAsync(vehicleId, CurrentUserId);
            return Ok(result);
        }
    }
}
