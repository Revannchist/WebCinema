using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Interfaces
{
    public interface IUsersService
    {
        Task<Users> CreateUsersAsync(Users users);

        Task<List<Users>> GetAllUsersAsync();

        Task<Users> GetUsersByIdAsync(int id);

        Task<Users> DeleteUsersByIdAsync(int id);

        Task<Users> UpdateUsersAsync(int id, Users users);

        Task<Users> UpdateUserBasicInfoAsync(int id, UsersEditDto dto);
    }
}
