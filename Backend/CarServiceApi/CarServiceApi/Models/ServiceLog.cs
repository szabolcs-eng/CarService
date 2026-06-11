namespace CarServiceApi.Models
{
    public class ServiceLog
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public DateTime Date { get; set; }
        public int CarKmCount { get; set; }
        public string ServiceDescription { get; set; } = string.Empty;
        public int ServiceCost { get; set; }
    }
}
