using System.ComponentModel.DataAnnotations;

namespace CarServiceApi.DTOs
{
    public class ServiceLogCreateDto
    {
        [Required(ErrorMessage = "Vehicle ID is required!")]
        public int VehicleId { get; set; }

        [Required(ErrorMessage = "Date is required!")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Odometer reading is required!")]
        [Range(0, 2000000, ErrorMessage = "Invalid odometer reading!")]
        public int CarKmCount { get; set; }

        [Required(ErrorMessage = "Service description is required!")]
        [StringLength(500, ErrorMessage = "The description is too long (maximum 500 characters).")]
        public string ServiceDescription { get; set; } = string.Empty;

        [Required(ErrorMessage = "Service cost is required!")]
        [Range(0, 10000000, ErrorMessage = "Invalid cost amount!")]
        public int ServiceCost { get; set; }
    }
}