using Microsoft.EntityFrameworkCore;
using WebCinema.Interfaces;
using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Services
{
    public class UsersImageService : IUsersImageService
    {
        private readonly WebCinemaDBContext _dbContext;
        private readonly ILogger<UsersImageService> _logger;

        public UsersImageService(WebCinemaDBContext dbcontext, ILogger<UsersImageService> logger)
        {
            _dbContext = dbcontext;
            _logger = logger;
        }

        public async Task<bool> CreateUserImageAsync(UsersImageDto imageDto)
        {
            try
            {
                var userExists = await _dbContext.Users.AnyAsync(u => u.Id == imageDto.UserId);
                if (!userExists)
                {
                    _logger.LogWarning($"User with ID {imageDto.UserId} not found");
                    return false;
                }

                if (string.IsNullOrEmpty(imageDto.Image))
                {
                    _logger.LogWarning("Image data is empty");
                    return false;
                }

                int commaIndex = imageDto.Image.IndexOf(',');
                var format = imageDto.Image.Substring(0, commaIndex + 1);
                var imageString = imageDto.Image.Substring(commaIndex + 1);

                // Provera da li korisnik već ima profilnu sliku
                var existingImage = await _dbContext.UsersImages
                    .FirstOrDefaultAsync(x => x.UserId == imageDto.UserId);

                if (existingImage != null)
                {
                    _dbContext.UsersImages.Remove(existingImage);
                }

                var userImage = new UsersImage
                {
                    UserId = imageDto.UserId,
                    ImageByteArray = Convert.FromBase64String(imageString),
                    ImageFormat = format
                };

                await _dbContext.UsersImages.AddAsync(userImage);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user image");
                return false;
            }
        }

        public async Task<bool> DeleteUserImageByIdAsync(int imageId)
        {
            try
            {
                var userImage = await _dbContext.UsersImages.FindAsync(imageId);
                if (userImage == null)
                {
                    _logger.LogWarning($"Image with ID {imageId} not found");
                    return false;
                }

                _dbContext.UsersImages.Remove(userImage);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user image");
                return false;
            }
        }

        public async Task<List<UsersImageDto>> GetAllUserImagesAsync()
        {
            try
            {
                var userImages = await _dbContext.UsersImages
                    .Select(ui => new UsersImageDto
                    {
                        Id = ui.Id,
                        UserId = ui.UserId,
                        Image = ui.ImageFormat + Convert.ToBase64String(ui.ImageByteArray)
                    })
                    .ToListAsync();

                return userImages;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all user images");
                return new List<UsersImageDto>();
            }
        }

        public async Task<UsersImageDto> GetImageByUserIdAsync(int userId)
        {
            try
            {
                var userImage = await _dbContext.UsersImages
                    .Where(ui => ui.UserId == userId)
                    .Select(ui => new UsersImageDto
                    {
                        Id = ui.Id,
                        UserId = ui.UserId,
                        Image = ui.ImageFormat + Convert.ToBase64String(ui.ImageByteArray)
                    })
                    .FirstOrDefaultAsync();

                return userImage;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving image for user ID {userId}");
                return null;
            }
        }

        public async Task<UsersImageDto> GetUserImageByUsernameAsync(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                    return null;

                var userImage = await _dbContext.UsersImages
                    .Include(ui => ui.Users)
                    .Where(ui => ui.Users.Username == username)
                    .Select(ui => new UsersImageDto
                    {
                        Id = ui.Id,
                        UserId = ui.UserId,
                        Image = ui.ImageFormat + Convert.ToBase64String(ui.ImageByteArray)
                    })
                    .FirstOrDefaultAsync();

                return userImage;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving image for username '{username}'");
                return null;
            }
        }
    }
}
