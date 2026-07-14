using CarServiceApi.Data;
using CarServiceApi.DTOs;
using CarServiceApi.Filters;
using CarServiceApi.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace CarServiceApi.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }



        public async Task<PagedResponse<List<UserResponseDto>>> GetAllUsersAsync(PaginationFilter filter)
        {
            var query = _context.Users.AsNoTracking();

            int totalRecords = await query.CountAsync();

            var users = await query
                .OrderBy(u => u.Id)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(u => new UserResponseDto(
                    u.Id,
                    u.Username,
                    u.Email,
                    u.Role
                ))
                .ToListAsync();

            return new PagedResponse<List<UserResponseDto>>(users, filter.PageNumber, filter.PageSize, totalRecords);
        }
    }
}
