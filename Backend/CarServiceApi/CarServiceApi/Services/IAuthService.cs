using CarServiceApi.DTOs;

namespace CarServiceApi.Services
{
    public interface IAuthService
    {
        Task RegisterAsync(UserRegisterDto request);
        Task<string> LoginAsync(UserLoginDto request);
    }
}
