using WebCinema.Models;

namespace WebCinema.Interfaces
{
    public interface IUsersService
    {
        Users CreateUsers(Users users);
        List<Users> GetAllUsers();

        Users GetUsersById(int id);

        Users DeleteUsersById(int id);

        Users UpdateUsers(int id, Users users);
    }
}
