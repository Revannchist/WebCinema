using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Interfaces
{
    public interface IMoviesService
    {
        Task<MoviesResponseDto> CreateMovieAsync(MovieCreateDto movieDto);

        Task<MoviesPagedResponse<MoviesGetDto>> GetAllMoviesAsync(MoviesParameters parameters);

        Task<MoviesGetDto> GetMovieByIdAsync(int id);

        Task<Movies> DeleteMovieByIdAsync(int id);

        Task<MoviesResponseDto> UpdateMovieAsync(int id, MoviesUpdateDto movieDto);

        Task<Movies> UpdateMovieBasicInfoAsync(int id, MoviesUpdateBasicDto dto);
    }
}