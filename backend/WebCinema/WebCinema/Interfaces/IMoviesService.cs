using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Interfaces
{
    public interface IMoviesService
    {
        Task<MovieResponseDto> CreateMovieAsync(MovieCreateDto movieDto);

        Task<List<MoviesGetDto>> GetAllMoviesAsync();

        Task<MoviesGetDto> GetMovieByIdAsync(int id);

        Task<Movies> DeleteMovieByIdAsync(int id);

        Task<MovieResponseDto> UpdateMovieAsync(int id, MoviesUpdateDto movieDto);

        Task<Movies> UpdateMovieBasicInfoAsync(int id, MoviesUpdateBasicDto dto);
    }
}