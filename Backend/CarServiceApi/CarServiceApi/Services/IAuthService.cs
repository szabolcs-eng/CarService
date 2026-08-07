using CarServiceApi.DTOs;

namespace CarServiceApi.Services
{
    public interface IAuthService
    {
        Task RegisterAsync(UserRegisterDto request);
        Task<AuthResponseDto> LoginAsync(UserLoginDto request);
    }
}
