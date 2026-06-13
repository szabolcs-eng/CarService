using System.ComponentModel.DataAnnotations;

namespace CarServiceApi.DTOs
{
    public class VehicleCreateDto
    {
        [Required]
        public int UserId { get; set; }

        [Required(ErrorMessage = "A rendszám megadása kötelező!")]
        [StringLength(10, ErrorMessage = "A rendszám túl hosszú.")]
        public string LicensePlate { get; set; } = string.Empty;

        [Required(ErrorMessage = "A márka megadása kötelező!")]
        public string Brand { get; set; } = string.Empty;

        [Required(ErrorMessage = "A modell megadása kötelező!")]
        public string Model { get; set; } = string.Empty;

        [Required]
        public int Year { get; set; }
    }
}