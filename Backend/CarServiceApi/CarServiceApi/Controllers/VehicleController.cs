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
    public class VehicleController : BaseApiController
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddVehicle(VehicleCreateDto request)
        {
            // Ownership is always the caller - never taken from the request body.
            await _vehicleService.AddVehicleAsync(request, CurrentUserId);
            return Ok("Vehicle successfully added to the profile!");
        }

        // Deliberately no {userId} route parameter - a caller can only ever
        // list their own vehicles, determined from their access token.
        [HttpGet("my-vehicles")]
        public async Task<IActionResult> GetMyVehicles([FromQuery] PaginationFilter filter)
        {
            var response = await _vehicleService.GetUserVehiclesAsync(CurrentUserId, filter);
            return Ok(response);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateVehicle(int id, VehicleCreateDto request)
        {
            await _vehicleService.UpdateVehicleAsync(id, request, CurrentUserId);
            return Ok("Vehicle details successfully updated!");
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            await _vehicleService.DeleteVehicleAsync(id, CurrentUserId);
            return Ok("Vehicle (and all associated logs) successfully deleted!");
        }
    }
}
