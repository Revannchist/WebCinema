using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Interfaces
{
    public interface IRatingsService
    {
        Task<RatingsResponseDto> CreateRatingAsync(RatingCreateDto ratingDto);

        Task<List<RatingsGetDto>> GetAllRatingsAsync();

        Task<RatingsGetDto> GetRatingsByIdAsync(int id);

        Task<Ratings?> DeleteRatingByIdAsync(int id);

        Task<RatingsResponseDto> UpdateRatingsAsync(int id, RatingUpdateDto ratingDto);
    }
}
