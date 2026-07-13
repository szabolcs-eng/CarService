namespace CarServiceApi.DTOs
{
    public record ServiceLogCreateDto
    {
        public int VehicleId { get; set; }
        public DateTime Date { get; set; }
        public int CarKmCount { get; set; }
        public string ServiceDescription { get; set; } = string.Empty;
        public decimal ServiceCost { get; set; }
    }
}