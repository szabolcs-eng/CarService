using CarServiceApi.Data;
using CarServiceApi.DTOs;
using CarServiceApi.Models;
using CarServiceApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuelLogController : ControllerBase
    {

        private readonly IFuelLogService _fuelLogService;


        public FuelLogController(IFuelLogService fuelLogService)
        {
            _fuelLogService = fuelLogService;
        }



        [HttpPost("add")]
        public async Task<IActionResult> AddFuelLog(FuelLogCreateDto request)
        {
            try
            {
                await _fuelLogService.AddFuelLogAsync(request);
                return Ok("Fuel log successfully added!");

            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }



        [HttpGet("vehicle/{vehicleId}")]
        public async Task<IActionResult> GetFuelLogsForVehicle(int vehicleId)
        {
            var logs = await _fuelLogService.GetFuelLogsForVehicleAsync(vehicleId);
            return Ok(logs);
        }



        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateFuelLog(int id, FuelLogCreateDto request)
        {
            try
            {
                await _fuelLogService.UpdateFuelLogAsync(id, request);
                return Ok("Fuel log details successfully updated!");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteFuelLog(int id)
        {
            try
            {
                await _fuelLogService.DeleteFuelLogAsync(id);
                return Ok("Fuel log successfully deleted!");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("vehicle/{vehicleId}/average-consumption")]
        public async Task<IActionResult> GetAverageConsumption(int vehicleId)
        {
            try
            {
                var result = await _fuelLogService.GetAverageFuelConsumptionAsync(vehicleId);
                return Ok(result);

            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}