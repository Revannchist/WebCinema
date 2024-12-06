using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using WebCinema.Interfaces;
using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Services
{
    public class UsersService : IUsersService
    {
        private readonly WebCinemaDBContext _dbContext;

        public UsersService(WebCinemaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Users> CreateUsersAsync(Users users)
        {
            if (users == null)
            {
                return null;
            }
            await _dbContext.Users.AddAsync(users);
            await _dbContext.SaveChangesAsync();
            return users;
        }

        public async Task<List<Users>> GetAllUsersAsync()
        {
            var users = await _dbContext.Users.ToListAsync();
            return users;
        }

        public async Task<Users> GetUsersByIdAsync(int id)
        {
            var users = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            return users;
        }

        public async Task<Users> DeleteUsersByIdAsync(int id)
        {
            var users = await GetUsersByIdAsync(id);
            if (users != null)
            {
                _dbContext.Users.Remove(users);
                await _dbContext.SaveChangesAsync();
            }
            return users;
        }

        public async Task<Users> UpdateUsersAsync(int id, Users users)
        {
            var _users = await GetUsersByIdAsync(id);
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
                await _dbContext.SaveChangesAsync();
            }
            return _users;
        }

        public async Task<Users> UpdateUserBasicInfoAsync(int id, UsersEditDTO dto) //DTO for editing
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user != null)
            {
                user.Username = dto.Username;
                user.Email = dto.Email;
                user.Password = dto.Password;
            }
            await _dbContext.SaveChangesAsync();
            return user;
        }
    }
}