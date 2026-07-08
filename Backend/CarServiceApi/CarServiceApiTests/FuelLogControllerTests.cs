//using CarServiceApi.Controllers;
//using CarServiceApi.Data;
//using CarServiceApi.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace CarServiceApi.Tests
//{
//    public class FuelLogControllerTests
//    {   
//        private ApplicationDbContext GetInMemoryDbContext()
//        {
//            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//                .Options;

//            return new ApplicationDbContext(options);
//        }

//        [Fact]
//        public void GetAverageConsumption_LessThanTwoRefuels_ReturnsBadRequest()
//        {
//            // Arrange
//            var context = GetInMemoryDbContext();
//            var controller = new FuelLogController(context);
//            int vehicleId = 1;

//            context.FuelLogs.Add(new FuelLog { VehicleId = vehicleId, CarKmCount = 100000, FuelAmount = 50 });
//            context.SaveChanges();

//            // Act
//            var result = controller.GetAverageConsumption(vehicleId);

//            // Assert
//            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//            Assert.Equal("At least two fuel logs are required to calculate average consumption.", badRequestResult.Value);
//        }

//        [Fact]
//        public void GetAverageConsumption_NoCoveredDistance_ReturnsBadRequest()
//        {
//            // Arrange
//            var context = GetInMemoryDbContext();
//            var controller = new FuelLogController(context);
//            int vehicleId = 1;

//            context.FuelLogs.AddRange(new List<FuelLog>
//            {
//                new FuelLog { VehicleId = vehicleId, CarKmCount = 100000, FuelAmount = 50 },
//                new FuelLog { VehicleId = vehicleId, CarKmCount = 100000, FuelAmount = 40 } 
//            });
//            context.SaveChanges();

//            // Act
//            var result = controller.GetAverageConsumption(vehicleId);

//            // Assert
//            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//            Assert.Equal("No distance covered based on the odometer.", badRequestResult.Value);
//        }

//        [Fact]
//        public void GetAverageConsumption_CorrectData_ReturntsTheAverageConsumption()
//        {
//            // Arrange
//            var context = GetInMemoryDbContext();
//            var controller = new FuelLogController(context);
//            int vehicleId = 1;

//            context.FuelLogs.AddRange(new List<FuelLog>
//            {
//                new FuelLog { VehicleId = vehicleId, CarKmCount = 100000, FuelAmount = 50 },
//                new FuelLog { VehicleId = vehicleId, CarKmCount = 100500, FuelAmount = 40 } 
//            });
//            context.SaveChanges();

//            // Act
//            var result = controller.GetAverageConsumption(vehicleId);

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result);

//            var responseValue = okResult.Value;

//            var vehicleIdProp = responseValue.GetType().GetProperty("VehicleId").GetValue(responseValue, null);
//            var totalDistanceKmProp = responseValue.GetType().GetProperty("TotalDistanceKm").GetValue(responseValue, null);
//            var totalFuelUsedLitersProp = responseValue.GetType().GetProperty("TotalFuelUsedLiters").GetValue(responseValue, null);
//            var averageConsumptionProp = responseValue.GetType().GetProperty("AverageConsumption").GetValue(responseValue, null);

//            Assert.Equal(1, vehicleIdProp);
//            Assert.Equal(500, totalDistanceKmProp);
//            Assert.Equal(40.0, totalFuelUsedLitersProp);
//            Assert.Equal(8.0, averageConsumptionProp);
//        }
//    }
//}