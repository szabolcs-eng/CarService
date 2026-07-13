namespace CarServiceApi.DTOs
{
    public record UserResponseDto(
        int Id,
        string Username,
        string Email,
        string Role
    );
}
