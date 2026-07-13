namespace CarServiceApi.Models
{
    public class FuelLog
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }
        public DateTime Date { get; set; }
        public int CarKmCount { get; set; }
        public double FuelAmount { get; set; } 
        public decimal FuelCost { get; set; }
    }
}
