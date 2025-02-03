using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Interfaces
{
    public interface IUsersService
    {
        Task<(Users user, string errorMessage)> CreateUsersAsync(Users users);
        Task<List<UserDisplayDto>> GetAllUsersAsync();
        Task<Users> GetUsersByIdAsync(int id);
        Task<UserDisplayDto> GetUsersByIdForDisplayAsync(int id);
        Task<UserDisplayDto> DeleteUsersByIdAsync(int id);
        Task<(Users user, string errorMessage)> UpdateUsersAsync(int id, Users users);
        Task<(Users user, string errorMessage)> UpdateUserBasicInfoAsync(int id, UsersEditDto dto);
        Task<(List<UserDisplayDto> users, int totalUsers)> GetUsersPagedAsync(int page, int pageSize);

    }
}
