namespace CarServiceApi.DTOs
{
    public record VehicleResponseDto(
        int Id,
        int UserId,
        string LicensePlate,
        string Brand,
        string Model,
        int Year
    );
}
