namespace CarServiceApi.DTOs
{
    public record FuelLogResponseDto(
        int Id,
        int VehicleId,
        DateTime Date,
        int CarKmCount,
        double FuelAmount,
        decimal FuelCost
    );
}
