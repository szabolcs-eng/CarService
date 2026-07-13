using CarServiceApi.DTOs;
using CarServiceApi.Filters;
using CarServiceApi.Wrappers;

namespace CarServiceApi.Services
{
    public interface IUserService
    {
        Task<PagedResponse<List<UserResponseDto>>> GetAllUsersAsync(PaginationFilter filter);
    }
}
