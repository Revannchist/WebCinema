using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Interfaces
{
    public interface IRatingsService
    {
        Task<RatingsResponseDto> CreateRatingAsync(RatingCreateDto ratings);

        Task<List<Ratings>> GetAllRatingsAsync();

        Task<Ratings> GetRatingsByIdAsync(int id);

        Task<Ratings> DeleteRatingsByIdAsync(int id);

        Task<Ratings> UpdateRatingsAsync(int id, Ratings ratings);
    }
}
