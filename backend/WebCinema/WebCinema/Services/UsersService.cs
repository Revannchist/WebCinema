using System.Diagnostics.Metrics;
using WebCinema.Interfaces;
using WebCinema.Models;

namespace WebCinema.Services
{
    public class UsersService : IUsersService
    {
        private readonly WebCinemaDBContext _dbContext;

        public UsersService(WebCinemaDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Users CreateUsers(Users users)
        {
            if (users == null)
            {
                return null;
            }
            _dbContext.Users.Add(users);
            _dbContext.SaveChanges();
            return users;
        }

        public List<Users> GetAllUsers()
        {
            var users = _dbContext.Users.ToList();
            return users;
        }

        public Users GetUsersById(int id)
        {
            var users = _dbContext.Users.FirstOrDefault(x => x.Id == id);
            return users;
        }
        public Users DeleteUsersById(int id)
        {
            var users = GetUsersById(id);
            if (users != null)
            {
                _dbContext.Users.Remove(users);
                _dbContext.SaveChanges();
            }
            return users;
        }

        public Users UpdateUsers(int id, Users users)
        {
            var _users = GetUsersById(id);
            if (users != null)
            {
                _users.Username = users.Username;
                _users.Email = users.Email;
                _users.Password = users.Password;
                _users.FirstName = users.FirstName;
                _users.LastName = users.LastName;
                _users.DateOfBirth = users.DateOfBirth;
                _users.RegistrationTime = users.RegistrationTime;
                _dbContext.Users.Update(_users);
                _dbContext.SaveChanges();
            }
            return _users;
        }
    }
}
