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

        private async Task<bool> IsUsernameUniqueAsync(string username, int? excludeUserId = null)
        {
            return !await _dbContext.Users
                .AnyAsync(u => u.Username == username && (!excludeUserId.HasValue || u.Id != excludeUserId));
        }

        private async Task<bool> IsEmailUniqueAsync(string email, int? excludeUserId = null)
        {
            return !await _dbContext.Users
                .AnyAsync(u => u.Email == email && (!excludeUserId.HasValue || u.Id != excludeUserId));
        }

        private bool IsPasswordValid(string password)
        {
            return password.Length >= 5 && password.Any(char.IsDigit);
        }

        private async Task<(bool isValid, string errorMessage)> ValidateUserAsync(Users users, int? excludeUserId = null)
        {
            if (!await IsUsernameUniqueAsync(users.Username, excludeUserId))
            {
                return (false, "Username already exists");
            }

            if (!await IsEmailUniqueAsync(users.Email, excludeUserId))
            {
                return (false, "Email already exists");
            }

            if (!IsPasswordValid(users.Password))
            {
                return (false, "Password must be at least 5 characters long and contain at least 1 number");
            }

            // Provera role
            var roleExists = await _dbContext.Roles.AnyAsync(r => r.Id == users.RoleId);
            if (!roleExists)
            {
                return (false, "Invalid role specified");
            }

            return (true, string.Empty);
        }

        public async Task<(Users user, string errorMessage)> CreateUsersAsync(Users users)
        {
            if (users == null)
            {
                return (null, "User data is required");
            }

            // Ako nije prosleđena rola, postavi default rolu (User - ID 2)
            if (users.RoleId == 0)
            {
                users.RoleId = 2; // ID za User rolu
            }

            var validation = await ValidateUserAsync(users);
            if (!validation.isValid)
            {
                return (null, validation.errorMessage);
            }

            await _dbContext.Users.AddAsync(users);
            await _dbContext.SaveChangesAsync();
            return (users, string.Empty);
        }

        public async Task<List<UserDisplayDto>> GetAllUsersAsync()
        {
            return await _dbContext.Users
                .Include(u => u.Roles)
                .Select(u => new UserDisplayDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    Password = u.Password,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    DateOfBirth = u.DateOfBirth,
                    RegistrationTime = u.RegistrationTime,
                    RoleId = u.RoleId,
                    RoleName = u.Roles.Name
                })
                .ToListAsync();
        }

        public async Task<UserDisplayDto> GetUsersByIdForDisplayAsync(int id)
        {
            return await _dbContext.Users
                .Include(u => u.Roles)
                .Where(x => x.Id == id)
                .Select(u => new UserDisplayDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    Password = u.Password,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    DateOfBirth = u.DateOfBirth,
                    RegistrationTime = u.RegistrationTime,
                    RoleId = u.RoleId,
                    RoleName = u.Roles.Name
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Users> GetUsersByIdAsync(int id)
        {
            return await _dbContext.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(x => x.Id == id);
        }




        public async Task<UserDisplayDto> DeleteUsersByIdAsync(int id)
        {
            var user = await GetUsersByIdAsync(id);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();

                return new UserDisplayDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Password = user.Password,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DateOfBirth = user.DateOfBirth,
                    RegistrationTime = user.RegistrationTime,
                    RoleId = user.RoleId,
                    RoleName = user.Roles?.Name
                };
            }
            return null;
        }

        public async Task<(Users user, string errorMessage)> UpdateUsersAsync(int id, Users users)
        {
            var existingUser = await GetUsersByIdAsync(id);
            if (existingUser == null)
            {
                return (null, "User not found");
            }

            var validation = await ValidateUserAsync(users, id);
            if (!validation.isValid)
            {
                return (null, validation.errorMessage);
            }

            existingUser.Username = users.Username;
            existingUser.Email = users.Email;
            existingUser.Password = users.Password;
            existingUser.FirstName = users.FirstName;
            existingUser.LastName = users.LastName;
            existingUser.DateOfBirth = users.DateOfBirth;
            existingUser.RegistrationTime = users.RegistrationTime;
            existingUser.RoleId = users.RoleId;

            _dbContext.Users.Update(existingUser);
            await _dbContext.SaveChangesAsync();
            return (existingUser, string.Empty);
        }

        public async Task<(Users user, string errorMessage)> UpdateUserBasicInfoAsync(int id, UsersEditDto dto)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return (null, "User not found");
            }

            var tempUser = new Users
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password,
                RoleId = dto.RoleId
            };

            var validation = await ValidateUserAsync(tempUser, id);
            if (!validation.isValid)
            {
                return (null, validation.errorMessage);
            }

            user.Username = dto.Username;
            user.Email = dto.Email;
            user.Password = dto.Password;
            user.RoleId = dto.RoleId;

            await _dbContext.SaveChangesAsync();
            return (user, string.Empty);
        }

        public async Task<(List<UserDisplayDto> users, int totalUsers)> GetUsersPagedAsync(int page, int pageSize)
        {
            

            var query = _dbContext.Users.Where(u => u.RoleId == 2);

            var totalUsers = await query.CountAsync();

            var users = await query
                .Include(u => u.Roles)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserDisplayDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    Password = u.Password,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    DateOfBirth = u.DateOfBirth,
                    RegistrationTime = u.RegistrationTime,
                    RoleId = u.RoleId,
                    RoleName = u.Roles.Name
                })
                .ToListAsync();

            return (users, totalUsers);
        }


    }

   
}