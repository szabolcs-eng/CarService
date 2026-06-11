namespace CarServiceApi.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty; 
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }

        public ICollection<ServiceLog> ServiceLogs { get; set; } = new List<ServiceLog>();
        public ICollection<FuelLog> FuelLogs { get; set; } = new List<FuelLog>();
    }
}
