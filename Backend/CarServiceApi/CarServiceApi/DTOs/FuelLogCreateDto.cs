namespace CarServiceApi.DTOs
{
    public record FuelLogCreateDto
    {
        public int VehicleId { get; set; }
        public DateTime Date { get; set; }
        public int CarKmCount { get; set; }
        public double FuelAmount { get; set; }
        public decimal FuelCost { get; set; }
    }
}