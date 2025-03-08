using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Interfaces
{
    public interface IMoviesService
    {
        Task<MoviesResponseDto> CreateMovieAsync(MovieCreateDto movieDto, CancellationToken cancellationToken = default);

        Task<MoviesPagedResponse<MoviesGetDto>> GetAllMoviesAsync(MoviesParameters parameters, CancellationToken cancellationToken = default);

        Task<MoviesGetDto> GetMovieByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<Movies> DeleteMovieByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<MoviesResponseDto> UpdateMovieAsync(int id, MoviesUpdateDto movieDto, CancellationToken cancellationToken = default);

        Task<Movies> UpdateMovieBasicInfoAsync(int id, MoviesUpdateBasicDto dto, CancellationToken cancellationToken = default);
    }
}