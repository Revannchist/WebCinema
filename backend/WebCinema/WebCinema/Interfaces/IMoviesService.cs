using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Interfaces
{
    public interface IMoviesService
    {
        Task<Movies> CreateMovieAsync(Movies movie);

        Task<Movies> AddGenreToMovieAsync(int genreId, int movieId);

        Task<Movies> UpdateMovieGenreAsync(int genreId, int movieId, Genres id);

        Task<Movies> AddActorToMovieAsync(int actorId, int movieId);

        Task<Movies> UpdateMovieActorAsync(int actorId, int movieId, Actors id);

        Task<List<MoviesGetDTO>> GetAllMoviesAsync();

        Task<MoviesGetDTO> GetMovieByIdAsync(int id);

        Task<Movies> DeleteMovieByIdAsync(int id);

        Task<Movies> UpdateMovieAsync(int id, Movies movie);

        Task<Movies> UpdateMovieBasicInfoAsync(int id, MoviesEditDTO dto);
    }
}