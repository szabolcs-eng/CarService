namespace CarServiceApi.DTOs
{
    public class FuelLogCreateDto
    {
        public int VehicleId { get; set; }
        public DateTime Date { get; set; }
        public int CarKmCount { get; set; }
        public int FuelAmount { get; set; }
        public int FuelCost { get; set; }
    }
}