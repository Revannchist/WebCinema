using Microsoft.EntityFrameworkCore;
using WebCinema.Interfaces;
using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Services
{
    public class MoviesService : IMoviesService
    {

        private readonly WebCinemaDBContext _dbContext;
        private readonly ILogger<MoviesService> _logger;
        private readonly ICountriesService _countriesService;
        private readonly IDirectorsService _directorsService;

        public MoviesService(WebCinemaDBContext dbContext, ILogger<MoviesService> logger,
            ICountriesService countriesService, IDirectorsService directorsService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _countriesService = countriesService;
            _directorsService = directorsService;
        }

        public async Task<Movies> CreateMovieAsync(Movies movie)
        {
            try
            {
                if (movie == null)//validacija inputa
                {
                    _logger.LogWarning("Attempt to add NULL Movie!");
                    return null;
                }

                // Validira foreign key reference za Directors 
                if (movie.DirectorId != 0 && !await _dbContext.Directors.AnyAsync(d => d.Id == movie.DirectorId))
                {
                    _logger.LogWarning($"Invalid DirectorId: {movie.DirectorId}");
                    throw new InvalidOperationException("Invalid DirectorId");
                }

                //Validira foreign key referenc za Countries
                if (movie.CountryId != 0 && !await _dbContext.Countries.AnyAsync(c => c.Id == movie.CountryId))
                {
                    _logger.LogWarning($"Invalid CountryId: {movie.CountryId}");
                    throw new InvalidOperationException("Invalid CountryId");
                }

                var existingMovie = await _dbContext.Movies
                    .FirstOrDefaultAsync(m => m.Title == movie.Title && m.ReleaseDate == movie.ReleaseDate);
                //Ova validacija za release date vjv nije potrebna jer moze bit vise filmova da izadju na isti datum

                if (existingMovie != null)
                {
                    _logger.LogWarning($"Movie with title: '{movie.Title}' already exists");
                    throw new InvalidOperationException("Movie with the same title already exists!");
                }

                await _dbContext.Movies.AddAsync(movie);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Movie created successfully: {movie.Id} - {movie.Title}");
                return movie;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating movie: {ex.Message}");
                throw;
            }
        }

        public async Task<List<MoviesGetDTO>> GetAllMoviesAsync()
        {
            try
            {
                var movies = await _dbContext.Movies
                    .AsNoTracking()
                    .Select(m => new MoviesGetDTO
                    {
                        Id = m.Id,
                        Title = m.Title,
                        Description = m.Description,
                        ReleaseDate = m.ReleaseDate,
                        Duration = m.Duration,
                        Language = m.Language,
                        AgeRating = m.AgeRating,
                        DirectorId = m.Director != null ? m.Director.Id : 0,
                        Director = m.Director,
                        CountryId = m.Country != null ? m.Country.Id : 0,
                        Country = m.Country,
                        MoviesGenresIds = m.MoviesGenres.Select(mg => mg.GenreId).ToList(),
                        MoviesActorsIds = m.MoviesActors.Select(ma => ma.ActorId).ToList()
                    })
                    .ToListAsync();

                return movies;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all movies");
                throw;
            }
        }

        public async Task<MoviesGetDTO?> GetMovieByIdAsync(int id)
        {
            try
            {
                var movie = await _dbContext.Movies
                   //.Include(m => m.Ratings) --Kada napravimo Ratings model ovo cemo dodat
                   .AsNoTracking()
                   .Where(x =>  x.Id == id)
                   .Select(m => new MoviesGetDTO
                   {
                       Id = m.Id,
                       Title = m.Title,
                       Description = m.Description,
                       ReleaseDate = m.ReleaseDate,
                       Duration = m.Duration,
                       Language = m.Language,
                       AgeRating = m.AgeRating,
                       DirectorId = m.Director != null ? m.Director.Id : 0,
                       Director = m.Director,
                       CountryId = m.Country != null ? m.Country.Id : 0,
                       Country = m.Country,
                       MoviesGenresIds = m.MoviesGenres.Select(mg => mg.GenreId).ToList(),
                       MoviesActorsIds = m.MoviesActors.Select(ma => ma.ActorId).ToList()
                   })
                   .FirstOrDefaultAsync();

                if (movie == null)
                {
                    _logger.LogWarning($"Movie not found with ID: {id}");
                }

                return movie;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving movie with ID: {id}");
                throw;
            }
        }

        public async Task<Movies?> DeleteMovieByIdAsync(int id)
        {
            try
            {
                var movie = await _dbContext.Movies.FindAsync(id);

                if(movie == null)
                {
                    _logger.LogWarning($"Attempt to delete non-existent movie: {id}");
                    return null;
                }
                _dbContext.Movies.Remove(movie);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Movie deleted successfully: {id}");
                return movie;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting movie with ID: {id}");
                throw;
            }
        }

        public async Task<Movies> UpdateMovieAsync(int id, Movies movie)
        {
            try
            {
                var existingMovie = await _dbContext.Movies.FindAsync(id);

                if (existingMovie == null)
                {
                    _logger.LogWarning($"Movie not found for update: {id}");
                    return null;
                }

                if (movie.DirectorId != 0 && !await _dbContext.Directors.AnyAsync(d => d.Id == movie.DirectorId))
                {
                    _logger.LogWarning($"Invalid DirectorId during update: {movie.DirectorId}");
                    throw new InvalidOperationException("Invalid DirectorId");
                }

                if (movie.CountryId != 0 && !await _dbContext.Countries.AnyAsync(c => c.Id == movie.CountryId))
                {
                    _logger.LogWarning($"Invalid CountryId during update: {movie.CountryId}");
                    throw new InvalidOperationException("Invalid CountryId");
                }

                var duplicateMovie = await _dbContext.Movies
                    .FirstOrDefaultAsync(m =>
                        m.Title == movie.Title &&
                        m.ReleaseDate == movie.ReleaseDate &&
                        m.Id != id);

                if (duplicateMovie != null)
                {
                    _logger.LogWarning($"Update failed: Movie with same title and release date already exists");
                    throw new InvalidOperationException("Movie with same title and release date already exists");
                }

                existingMovie.Title = movie.Title;
                existingMovie.Description = movie.Description;
                existingMovie.ReleaseDate = movie.ReleaseDate;
                existingMovie.Duration = movie.Duration;
                existingMovie.Language = movie.Language;
                existingMovie.AgeRating = movie.AgeRating;
                existingMovie.DirectorId = movie.DirectorId;
                existingMovie.CountryId = movie.CountryId;

                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Movie updated successfully: {id}");
                return existingMovie;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating movie with ID: {id}");
                throw;
            }
        }

        public async Task<Movies> UpdateMovieBasicInfoAsync(int id, MoviesEditDTO dto)
        {
            try
            {
                var movie = await _dbContext.Movies.FindAsync(id);

                if (movie == null)
                {
                    _logger.LogWarning($"Movie not found for basic info update: {id}");
                    return null;
                }

                var duplicateMovie = await _dbContext.Movies
                    .FirstOrDefaultAsync(m =>
                        m.Title == dto.Title &&
                        m.ReleaseDate == dto.ReleaseDate &&
                        m.Id != id);

                if (duplicateMovie != null)
                {
                    _logger.LogWarning($"Basic info update failed: Movie with same title and release date already exists");
                    throw new InvalidOperationException("Movie with same title and release date already exists");
                }

                movie.Title = dto.Title;
                movie.Description = dto.Description;
                movie.ReleaseDate = dto.ReleaseDate;

                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Movie basic info updated successfully: {id}");
                return movie;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating basic movie info for ID: {id}");
                throw;
            }
        }

        public async Task<Movies> AddGenreToMovieAsync(int genreId, int movieId)
        {
            try
            {
                // Find the movie with its existing genres
                var movie = await _dbContext.Movies
                    .Include(m => m.MoviesGenres)
                    .FirstOrDefaultAsync(m => m.Id == movieId); 

                if (movie == null)
                {
                    throw new InvalidOperationException($"Movie with ID {movieId} not found.");
                }

                // Find the genre
                var genre = await _dbContext.Genres
                    .FirstOrDefaultAsync(g => g.Id == genreId);

                if (genre == null)
                {
                    throw new InvalidOperationException($"Genre with ID {genreId} not found.");
                }

                // Check if the genre is already associated with the movie
                if (movie.MoviesGenres == null)
                {
                    movie.MoviesGenres = new List<MoviesGenres>();
                }

                if (movie.MoviesGenres.Any(mg => mg.GenreId == genreId))
                {
                    throw new InvalidOperationException($"Genre {genreId} is already associated with this movie.");
                }

                // Create and add the new MoviesGenres relationship
                var movieGenre = new MoviesGenres
                {
                    MovieId = movieId,
                    GenreId = genreId
                };

                movie.MoviesGenres.Add(movieGenre);

                await _dbContext.SaveChangesAsync();

                return movie;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding genre to movie. MovieId: {movieId}, GenreId: {genreId}");
                throw;
            }
        }

        public async Task<Movies> UpdateMovieGenreAsync(int genreId, int movieId, Genres genre)
        {
            try
            {
                // Find the movie with its existing genres
                var movie = await _dbContext.Movies
                    .Include(m => m.MoviesGenres)
                    .FirstOrDefaultAsync(m => m.Id == movieId);

                if (movie == null)
                {
                    throw new InvalidOperationException($"Movie with ID {movieId} not found.");
                }

                // Find the existing movie-genre relationship
                var existingMovieGenre = movie.MoviesGenres?
                    .FirstOrDefault(mg => mg.GenreId == genreId);

                if (existingMovieGenre == null)
                {
                    throw new InvalidOperationException($"Genre {genreId} is not associated with this movie.");
                }

                // Validate the new genre
                var newGenre = await _dbContext.Genres
                    .FirstOrDefaultAsync(g => g.Id == genre.Id);

                if (newGenre == null)
                {
                    throw new InvalidOperationException($"New genre with ID {genre.Id} not found.");
                }

                // Check if the new genre is already associated with the movie
                if (movie.MoviesGenres.Any(mg => mg.GenreId == genre.Id))
                {
                    throw new InvalidOperationException($"Genre {genre.Id} is already associated with this movie.");
                }

                // Remove the old genre-movie relationship
                _dbContext.MoviesGenres.Remove(existingMovieGenre);

                // Add the new genre-movie relationship
                var newMovieGenre = new MoviesGenres
                {
                    MovieId = movieId,
                    GenreId = genre.Id
                };

                movie.MoviesGenres.Remove(existingMovieGenre);
                movie.MoviesGenres.Add(newMovieGenre);

                // Save changes
                await _dbContext.SaveChangesAsync();

                return movie;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating genre for movie ID: {movieId}");
                throw;
            }
        }

        public async Task<Movies> AddActorToMovieAsync(int actorId, int movieId)
        {
            try
            {
                var movie = await _dbContext.Movies
                    .Include(m => m.MoviesActors)
                    .FirstOrDefaultAsync(m => m.Id == movieId);

                if (movie == null)
                {
                    throw new InvalidOperationException($"Movie with ID {movieId} not found.");
                }

                var actor = await _dbContext.Actors
                    .FirstOrDefaultAsync(a => a.Id == actorId);

                if (actor == null)
                {
                    throw new InvalidOperationException($"Actor with ID {actorId} not found.");
                }

                if (movie.MoviesActors == null)
                {
                    movie.MoviesActors = new List<MoviesActors>();
                }

                if (movie.MoviesActors.Any(ma => ma.ActorId == actorId))
                {
                    throw new InvalidOperationException($"Actor {actorId} is already associated with this movie.");
                }

                var movieActor = new MoviesActors
                {
                    MovieId = movieId,
                    ActorId = actorId
                };

                movie.MoviesActors.Add(movieActor);

                await _dbContext.SaveChangesAsync();

                return movie;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding actor to movie. MovieId: {movieId}, ActorId: {actorId}");
                throw;
            }
        }

        public async Task<Movies> UpdateMovieActorAsync(int actorId, int movieId, Actors actor)
        {
            try
            {
                var movie = await _dbContext.Movies
                    .Include(m => m.MoviesActors)
                    .FirstOrDefaultAsync(m => m.Id == movieId);

                if (movie == null)
                {
                    throw new InvalidOperationException($"Movie with ID {movieId} not found.");
                }

                var existingMovieActor = movie.MoviesActors?
                    .FirstOrDefault(ma => ma.ActorId == actorId);

                if (existingMovieActor == null)
                {
                    throw new InvalidOperationException($"Actor {actorId} is not associated with this movie.");
                }

                var newActor = await _dbContext.Actors
                    .FirstOrDefaultAsync(a => a.Id == actor.Id);

                if (newActor == null)
                {
                    throw new InvalidOperationException($"New actor with ID {actor.Id} not found.");
                }

                if (movie.MoviesActors.Any(ma => ma.ActorId == actor.Id))
                {
                    throw new InvalidOperationException($"Actor {actor.Id} is already associated with this movie.");
                }

                _dbContext.MoviesActors.Remove(existingMovieActor);

                var newMovieActor = new MoviesActors
                {
                    MovieId = movieId,
                    ActorId = actor.Id
                };

                movie.MoviesActors.Remove(existingMovieActor);
                movie.MoviesActors.Add(newMovieActor);

                // Save changes
                await _dbContext.SaveChangesAsync();

                return movie;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating actor for movie ID: {movieId}");
                throw;
            }
        }
    }
}