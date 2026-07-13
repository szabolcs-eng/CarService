namespace CarServiceApi.Models
{
    public class ServiceLog
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }
        public DateTime Date { get; set; }
        public int CarKmCount { get; set; }
        public string ServiceDescription { get; set; } = string.Empty;
        public decimal ServiceCost { get; set; }
    }
}
