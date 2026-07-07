using Microsoft.AspNetCore.Mvc;
using CarServiceApi.Data;
using CarServiceApi.Models;
using CarServiceApi.DTOs;
using CarServiceApi.Services;

namespace CarServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;


        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }



        [HttpPost("add")]
        public async Task<IActionResult> AddVehicle(VehicleCreateDto request)
        {
            try
            {
                await _vehicleService.AddVehicleAsync(request);
                return Ok("Vehicle successfully added to the profile!");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }



        [HttpGet("user-vehicles/{userId}")]
        public async Task<IActionResult> GetUserVehicles(int userId)
        {
            var vehicles = await _vehicleService.GetUserVehiclesAsync(userId);
            return Ok(vehicles);
        }



        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateVehicle(int id, VehicleCreateDto request)
        {
            try
            {
                await _vehicleService.UpdateVehicleAsync(id, request);
                return Ok("Vehicle details successfully updated!");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);

            }
        }



        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            try
            {
                await _vehicleService.DeleteVehicleAsync(id);
                return Ok("Vehicle (and all associated logs) successfully deleted!");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);

            }
        }
    }

}