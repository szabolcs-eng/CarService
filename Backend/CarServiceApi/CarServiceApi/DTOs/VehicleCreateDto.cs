using System.ComponentModel.DataAnnotations;

namespace CarServiceApi.DTOs
{
    public class VehicleCreateDto
    {
        [Required]
        public int UserId { get; set; }

        [Required(ErrorMessage = "License plate is required!")]
        [StringLength(10, ErrorMessage = "License plate is too long.")]
        public string LicensePlate { get; set; } = string.Empty;

        [Required(ErrorMessage = "Brand is required!")]
        public string Brand { get; set; } = string.Empty;

        [Required(ErrorMessage = "Model is required!")]
        public string Model { get; set; } = string.Empty;

        [Required]
        public int Year { get; set; }
    }
}