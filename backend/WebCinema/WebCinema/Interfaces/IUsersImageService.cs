using WebCinema.Models.DTO;

namespace WebCinema.Interfaces
{
    public interface IUsersImageService
    {
        Task<bool> CreateUserImageAsync(UsersImageDto imageDto);
        Task<bool> DeleteUserImageByIdAsync(int imageId);
        Task<List<UsersImageDto>> GetAllUserImagesAsync();
        Task<UsersImageDto> GetImageByUserIdAsync(int userId);
        Task<UsersImageDto> GetUserImageByUsernameAsync(string username);
    }
}
