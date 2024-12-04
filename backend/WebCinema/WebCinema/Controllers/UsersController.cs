using Microsoft.AspNetCore.Mvc;
using WebCinema.Interfaces;
using WebCinema.Models;
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
        public IActionResult AddUsers(Users users)
        {
            var createdUsers = _usersService.CreateUsers(users);
            if (createdUsers == null)
            {
                return BadRequest("Greska!");
            }
            return Ok(createdUsers);
        }

        [HttpPost]
        public IActionResult DeleteUsersById(int id)
        {
            var deletedUsers = _usersService.DeleteUsersById(id);
            if (deletedUsers == null)
            {
                return BadRequest("Greska!");
            }
            return Ok(deletedUsers);
        }

        [HttpPost]
        public IActionResult UpdateUsers(int id, Users users)
        {
            var updatedUsers= _usersService.UpdateUsers(id, users);
            if (updatedUsers == null)
            {
                return BadRequest("Greska!");
            }
            return Ok(updatedUsers);
        }

        [HttpGet]
        public IActionResult GetUsersById(int id)
        {
            var users = _usersService.GetUsersById(id);
            if (users == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(users);
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _usersService.GetAllUsers();
            if (users == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(users);

        }
    }
}
