using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCinema.Interfaces;
using WebCinema.Models;
using WebCinema.Models.DTO;
using WebCinema.Services;

namespace WebCinema.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserCreateDto userDto)
        {
            var users = new Users
            {
                Username = userDto.Username,
                Email = userDto.Email,
                Password = userDto.Password,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                DateOfBirth = userDto.DateOfBirth,
                RegistrationTime = DateTime.Now,
                RoleId = userDto.RoleId
            };

            var (createdUser, errorMessage) = await _usersService.CreateUsersAsync(users);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return BadRequest(errorMessage);
            }
            return Ok(createdUser);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserById(int id)
        {
            var deletedUser = await _usersService.DeleteUsersByIdAsync(id);
            if (deletedUser == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(deletedUser);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(int id, Users users)
        {
            var (updatedUser, errorMessage) = await _usersService.UpdateUsersAsync(id, users);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return BadRequest(errorMessage);
            }
            return Ok(updatedUser);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _usersService.GetUsersByIdForDisplayAsync(id);
            if (user == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(user);
        }

      

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _usersService.GetAllUsersAsync();
            if (users == null || !users.Any())
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserBasicInfo(int id, UsersEditDto dto)
        {
            var (updatedUser, errorMessage) = await _usersService.UpdateUserBasicInfoAsync(id, dto);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return BadRequest(errorMessage);
            }
            return Ok(updatedUser);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersPaged(int page = 1, int pageSize = 3)
        {
            var (users, totalUsers) = await _usersService.GetUsersPagedAsync(page, pageSize);
            if (users == null || !users.Any())
            {
                return BadRequest("No users found.");
            }
            return Ok(new { Users = users, TotalUsers = totalUsers });
        }
    }
}
