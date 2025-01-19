using WebCinema.Models;

namespace WebCinema.Interfaces
{
    public interface IRatingsService
    {
        Task<Ratings> CreateRatingsAsync(Ratings ratings);

        Task<List<Ratings>> GetAllRatingsAsync();

        Task<Ratings> GetRatingsByIdAsync(int id);

        Task<Ratings> DeleteRatingsByIdAsync(int id);

        Task<Ratings> UpdateRatingsAsync(int id, Ratings ratings);
    }
}
