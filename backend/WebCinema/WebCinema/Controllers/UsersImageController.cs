using Microsoft.AspNetCore.Mvc;
using WebCinema.Interfaces;
using WebCinema.Models.DTO;

namespace WebCinema.Controllers
{
    
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UsersImageController : ControllerBase
    {
        private readonly IUsersImageService _usersImageService;
        public UsersImageController(IUsersImageService usersImageService)
        {
            _usersImageService = usersImageService;
        }

        [HttpPost]
        public async Task<IActionResult> AddUserImage([FromBody] UsersImageDto imageDto)
        {
            var created = await _usersImageService.CreateUserImageAsync(imageDto);
            if (!created)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(created);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserImageById(int imageId)
        {
            var deleted = await _usersImageService.DeleteUserImageByIdAsync(imageId);
            if (!deleted)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(deleted);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserImages()
        {
            var images = await _usersImageService.GetAllUserImagesAsync();
            if (images == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(images);
        }

        [HttpGet]
        public async Task<IActionResult> GetImageByUserId(int userId)
        {
            var image = await _usersImageService.GetImageByUserIdAsync(userId);
            if (image == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(image);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserImageByUsername(string username)
        {
            var image = await _usersImageService.GetUserImageByUsernameAsync(username);
            if (image == null)
            {
                return BadRequest("Error | Bad Request!");
            }
            return Ok(image);
        }
    }
}

