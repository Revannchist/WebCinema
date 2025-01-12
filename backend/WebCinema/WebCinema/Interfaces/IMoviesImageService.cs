using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Interfaces
{
    public interface IMoviesImageService
    {
        Task<bool> CreateMovieImageAsync(MoviesImageDto imageDto);

        Task<bool> DeleteMovieImageByIdAsync(int imageId);

        Task<List<MoviesImageDto>> GetAllMovieImagesAsync();

        Task<List<MoviesImageDto>> GetImagesByMovieIdAsync(int movieId);

        Task<List<MoviesImageDto>> GetMovieImagesByMovieTitleAsync(string title);
    }
}
