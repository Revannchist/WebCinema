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

        public MoviesService(WebCinemaDBContext dbContext, ILogger<MoviesService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<MovieResponseDto> CreateMovieAsync(MovieCreateDto movieDto)
        {
            var movie = new Movies
            {
                Title = movieDto.Title,
                Description = movieDto.Description,
                ReleaseDate = movieDto.ReleaseDate,
                Duration = movieDto.Duration,
                Language = movieDto.Language,
                AgeRating = movieDto.AgeRating,
                DirectorId = movieDto.DirectorId,
                CountryId = movieDto.CountryId,
                MoviesGenres = movieDto.GenreIds?.Select(genreId => new MoviesGenres { GenreId = genreId }).ToList(),
                MoviesActors = movieDto.ActorIds?.Select(actorId => new MoviesActors { ActorId = actorId }).ToList()
            };

            await _dbContext.Movies.AddAsync(movie);
            await _dbContext.SaveChangesAsync();

            // Load related data for response
            await _dbContext.Entry(movie)
                .Reference(m => m.Director)
                .LoadAsync();

            await _dbContext.Entry(movie)
                .Reference(m => m.Country)
                .LoadAsync();

            await _dbContext.Entry(movie)
                .Collection(m => m.MoviesGenres)
                .Query()
                .Include(mg => mg.Genre)
                .LoadAsync();

            await _dbContext.Entry(movie)
                .Collection(m => m.MoviesActors)
                .Query()
                .Include(ma => ma.Actor)
                .LoadAsync();

            return new MovieResponseDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                ReleaseDate = movie.ReleaseDate,
                Duration = movie.Duration,
                Language = movie.Language,
                AgeRating = movie.AgeRating,
                Director = movie.Director != null ? new DirectorDto
                {
                    Id = movie.Director.Id,
                    FirstName = movie.Director.FirstName,
                    LastName = movie.Director.LastName
                } : null,

                Country = movie.Country != null ? new CountryDto
                {
                    Id = movie.Country.Id,
                    Name = movie.Country.Name
                } : null,

                Genres = movie.MoviesGenres?.Select(mg => new GenreDto
                {
                    Id = mg.Genre.Id,
                    Name = mg.Genre.Name
                }).ToList(),

                Actors = movie.MoviesActors?.Select(ma => new ActorDto
                {
                    Id = ma.Actor.Id,
                    FirstName = ma.Actor.FirstName, 
                    LastName = ma.Actor.LastName
                }).ToList()
            };
        }

        public async Task<MoviesGetDto> GetMovieByIdAsync(int id)
        {
            try
            {
                var movie = await _dbContext.Movies
                    .AsNoTracking()
                    .Where(m => m.Id == id)
                    .Select(m => new MoviesGetDto
                    {
                        Id = m.Id,
                        Title = m.Title,
                        Description = m.Description,
                        ReleaseDate = m.ReleaseDate,
                        Duration = m.Duration,
                        Language = m.Language,
                        AgeRating = m.AgeRating,
                        DirectorId = m.Director != null ? new DirectorDto 
                        {                                                 
                            Id = m.Director.Id,
                            FirstName = m.Director.FirstName,
                            LastName = m.Director.LastName
                        } : null,
                        CountryId = new CountryDto
                        {
                            Id = m.Country.Id,
                            Name = m.Country.Name
                        },
                        MoviesGenresIds = m.MoviesGenres
                            .Select(mg => mg.GenreId)
                            .ToList(),
                        MoviesActorsIds = m.MoviesActors
                            .Select(ma => ma.ActorId)
                            .ToList()
                    })
                    .FirstOrDefaultAsync();

                if (movie == null)
                {
                    throw new KeyNotFoundException($"Movie with ID {id} not found");
                }

                return movie;
            }
            catch (Exception ex) when (ex is not KeyNotFoundException)
            {
                _logger.LogError(ex, "Error retrieving movie with ID {MovieId}", id);
                throw;
            }
        }

        public async Task<List<MoviesGetDto>> GetAllMoviesAsync()
        {
            try
            {
                var movies = await _dbContext.Movies
                    .AsNoTracking()
                    .Select(m => new MoviesGetDto
                    {
                        Id = m.Id,
                        Title = m.Title,
                        Description = m.Description,
                        ReleaseDate = m.ReleaseDate,
                        Duration = m.Duration,
                        Language = m.Language,
                        AgeRating = m.AgeRating,
                        DirectorId = m.Director != null ? new DirectorDto //U slucaju da korisnik izbrise direktora
                        {                                                 //ovo omogucava da film bude prikazan iako nema direktora
                            Id = m.Director.Id,
                            FirstName = m.Director.FirstName,
                            LastName = m.Director.LastName
                        } : null,
                        CountryId = new CountryDto
                        {
                            Id = m.Country.Id,
                            Name = m.Country.Name
                        },
                        MoviesGenresIds = m.MoviesGenres
                            .Select(mg => mg.GenreId)
                            .ToList(),
                        MoviesActorsIds = m.MoviesActors
                            .Select(ma => ma.ActorId)
                            .ToList()
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

        public async Task<MovieResponseDto> UpdateMovieAsync(int id, MoviesUpdateDto movieDto)
        {
            var existingMovie = await _dbContext.Movies
                .Include(m => m.MoviesGenres)
                .Include(m => m.MoviesActors)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (existingMovie == null)
            {
                _logger.LogWarning($"Movie not found for update: {id}");
                throw new InvalidOperationException($"Movie with ID {id} not found");
            }

            // Check for duplicate movie
            var duplicateExists = await _dbContext.Movies
                .AnyAsync(m => m.Title == movieDto.Title
                    && m.ReleaseDate == movieDto.ReleaseDate
                    && m.Id != id);

            if (duplicateExists)
            {
                _logger.LogWarning($"Update failed: Movie with same title and release date already exists");
                throw new InvalidOperationException("Movie with same title and release date already exists");
            }

            // Update basic properties
            existingMovie.Title = movieDto.Title;
            existingMovie.Description = movieDto.Description;
            existingMovie.ReleaseDate = movieDto.ReleaseDate;
            existingMovie.Duration = movieDto.Duration;
            existingMovie.Language = movieDto.Language;
            existingMovie.AgeRating = movieDto.AgeRating;
            existingMovie.DirectorId = movieDto.DirectorId;
            existingMovie.CountryId = movieDto.CountryId;

            // Update genres
            if (movieDto.GenreIds != null)
            {
                // Remove existing genres
                existingMovie.MoviesGenres.Clear();

                // Add new genres
                existingMovie.MoviesGenres = movieDto.GenreIds
                    .Select(genreId => new MoviesGenres { MovieId = id, GenreId = genreId })
                    .ToList();
            }

            // Update actors
            if (movieDto.ActorIds != null)
            {
                // Remove existing actors
                existingMovie.MoviesActors.Clear();

                // Add new actors
                existingMovie.MoviesActors = movieDto.ActorIds
                    .Select(actorId => new MoviesActors { MovieId = id, ActorId = actorId })
                    .ToList();
            }

            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Movie updated successfully: {id}");

            // Load related data for response
            await _dbContext.Entry(existingMovie)
                .Reference(m => m.Director)
                .LoadAsync();

            await _dbContext.Entry(existingMovie)
                .Reference(m => m.Country)
                .LoadAsync();

            await _dbContext.Entry(existingMovie)
                .Collection(m => m.MoviesGenres)
                .Query()
                .Include(mg => mg.Genre)
                .LoadAsync();

            await _dbContext.Entry(existingMovie)
                .Collection(m => m.MoviesActors)
                .Query()
                .Include(ma => ma.Actor)
                .LoadAsync();

            // Map to response DTO
            return new MovieResponseDto
            {
                Id = existingMovie.Id,
                Title = existingMovie.Title,
                Description = existingMovie.Description,
                ReleaseDate = existingMovie.ReleaseDate,
                Duration = existingMovie.Duration,
                Language = existingMovie.Language,
                AgeRating = existingMovie.AgeRating,
                Director = existingMovie.Director != null ? new DirectorDto
                {
                    Id = existingMovie.Director.Id,
                    FirstName = existingMovie.Director.FirstName,
                    LastName = existingMovie.Director.LastName,
                } : null,

                Country = existingMovie.Country != null ? new CountryDto
                {
                    Id = existingMovie.Country.Id,
                    Name = existingMovie.Country.Name
                } : null,

                Genres = existingMovie.MoviesGenres?.Select(mg => new GenreDto
                {
                    Id = mg.Genre.Id,
                    Name = mg.Genre.Name
                }).ToList(),

                Actors = existingMovie.MoviesActors?.Select(ma => new ActorDto
                {
                    Id = ma.Actor.Id,
                    FirstName = ma.Actor.FirstName,
                    LastName = ma.Actor.LastName
                }).ToList()
            };
        }
        public async Task<Movies> UpdateMovieBasicInfoAsync(int id, MoviesUpdateBasicDto dto)
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

    }
}