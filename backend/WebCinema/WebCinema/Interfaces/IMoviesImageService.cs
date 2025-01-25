using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Interfaces
{
    public interface IMoviesImageService
    {
        Task<bool> CreateMoviePosterAsync(MovieCreatePosterDto posterDto);

        Task<bool> DeleteMoviePosterByIdAsync(int imageId);

        Task<List<MoviePosterResponseDto>> GetAllMoviePostersAsync();

        Task<MoviePosterResponseDto> GetPosterByMovieIdAsync(int id);

        Task<MoviePosterResponseDto> GetMoviePosterByTitleAsync(string title);
    }
}
