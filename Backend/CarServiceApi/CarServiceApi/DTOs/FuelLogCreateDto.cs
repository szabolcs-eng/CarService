using System.ComponentModel.DataAnnotations;

namespace CarServiceApi.DTOs
{
    public class FuelLogCreateDto
    {
        [Required(ErrorMessage = "Vehicle ID is required!")]
        public int VehicleId { get; set; }

        [Required(ErrorMessage = "Date is required!")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Odometer reading is required!")]
        [Range(0, 2000000, ErrorMessage = "Invalid odometer reading!")]
        public int CarKmCount { get; set; }

        [Required(ErrorMessage = "Fuel amount is required!")]
        [Range(1, 200, ErrorMessage = "Invalid fuel amount!")]
        public int FuelAmount { get; set; }

        [Required(ErrorMessage = "Fuel cost is required!")]
        [Range(0, 1000000, ErrorMessage = "Invalid cost amount!")]
        public int FuelCost { get; set; }
    }
}