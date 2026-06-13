using System.ComponentModel.DataAnnotations;

namespace CarServiceApi.DTOs
{
    public class FuelLogCreateDto
    {
        [Required(ErrorMessage = "A jármű azonosítójának megadása kötelező!")]
        public int VehicleId { get; set; }

        [Required(ErrorMessage = "A dátum megadása kötelező!")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "A kilométeróra állásának megadása kötelező!")]
        [Range(0, 2000000, ErrorMessage = "Érvénytelen kilométeróra állás!")]
        public int CarKmCount { get; set; }

        [Required(ErrorMessage = "A tankolt mennyiség megadása kötelező!")]
        [Range(1, 200, ErrorMessage = "Irreális üzemanyag mennyiség!")]
        public int FuelAmount { get; set; }

        [Required(ErrorMessage = "A költség megadása kötelező!")]
        [Range(0, 1000000, ErrorMessage = "Érvénytelen összeg!")]
        public int FuelCost { get; set; }
    }
}