using System.Threading;
using System.Threading.Tasks;
using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Interfaces
{
    public interface IMoviesImageService
    {
        Task<bool> CreateMoviePosterAsync(MovieCreatePosterDto posterDto, CancellationToken cancellationToken);

        Task<bool> DeleteMoviePosterByIdAsync(int imageId, CancellationToken cancellationToken);

        Task<List<MoviePosterResponseDto>> GetAllMoviePostersAsync(CancellationToken cancellationToken);

        Task<MoviePosterResponseDto?> GetPosterByMovieIdAsync(int id, CancellationToken cancellationToken);

        Task<MoviePosterResponseDto?> GetMoviePosterByTitleAsync(string title, CancellationToken cancellationToken);
    }
}
