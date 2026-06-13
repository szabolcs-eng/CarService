using System.ComponentModel.DataAnnotations;

namespace CarServiceApi.DTOs
{
    public class ServiceLogCreateDto
    {
        [Required(ErrorMessage = "Meg kell adni, hogy melyik járműhöz tartozik a bejegyzés!")]
        public int VehicleId { get; set; }

        [Required(ErrorMessage = "A dátum megadása kötelező!")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "A kilométeróra állásának megadása kötelező!")]
        [Range(0, 2000000, ErrorMessage = "Érvénytelen kilométeróra állás!")]
        public int CarKmCount { get; set; }

        [Required(ErrorMessage = "A szerviz leírásának megadása kötelező!")]
        [StringLength(500, ErrorMessage = "A leírás túl hosszú (maximum 500 karakter).")]
        public string ServiceDescription { get; set; } = string.Empty;

        [Required(ErrorMessage = "A költség megadása kötelező!")]
        [Range(0, 10000000, ErrorMessage = "Érvénytelen összeg!")]
        public int ServiceCost { get; set; }
    }
}