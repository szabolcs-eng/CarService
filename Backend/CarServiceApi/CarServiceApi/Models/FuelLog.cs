namespace CarServiceApi.Models
{
    public class FuelLog
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public DateTime Date { get; set; }
        public int CarKmCount { get; set; }
        public int FuelAmount { get; set; } 
        public int FuelCost { get; set; }
    }
}
