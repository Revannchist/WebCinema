using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Interfaces
{
    public interface IUsersService
    {
        Users CreateUsers(Users users);

        List<Users> GetAllUsers();

        Users GetUsersById(int id);

        Users DeleteUsersById(int id);

        Users UpdateUsers(int id, Users users);

        Users UpdateUserBasicInfo(int id, UsersEditDTO dto);
    }
}
