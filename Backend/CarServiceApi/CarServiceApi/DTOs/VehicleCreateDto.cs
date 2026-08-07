namespace CarServiceApi.DTOs
{
    // Note: no UserId here on purpose. Ownership is always derived from the
    // caller's JWT (see BaseApiController.CurrentUserId), never taken from
    // client input - otherwise a user could create a vehicle under someone
    // else's account just by editing the request body.
    public record VehicleCreateDto
    {
        public string LicensePlate { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public DateTime? TechnicalInspectionExpiry { get; set; }
    }
}
