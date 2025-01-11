using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> AddUser(Users users)
        {
            var createdUsers = await _usersService.CreateUsersAsync(users);
            if (createdUsers == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(createdUsers);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserById(int id)
        {
            var deletedUsers = await _usersService.DeleteUsersByIdAsync(id);
            if (deletedUsers == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(deletedUsers);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(int id, Users users)
        {
            var updatedUsers = await _usersService.UpdateUsersAsync(id, users);
            if (updatedUsers == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(updatedUsers);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserById(int id)
        {
            var users = await _usersService.GetUsersByIdAsync(id);
            if (users == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(users);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _usersService.GetAllUsersAsync();
            if (users == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserBasicInfo(int id, UsersEditDto dto)
        {
            var user = await _usersService.UpdateUserBasicInfoAsync(id, dto);
            if(user == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(user);
        }
    }
}
