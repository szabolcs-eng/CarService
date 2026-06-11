namespace CarServiceApi.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty; 
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }  
    }
}
