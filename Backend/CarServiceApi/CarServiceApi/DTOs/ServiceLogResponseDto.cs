namespace CarServiceApi.DTOs
{
    public record ServiceLogResponseDto(
        int Id,
        int VehicleId,
        DateTime Date,
        int CarKmCount,
        string ServiceDescription,
        decimal ServiceCost
    );
}
