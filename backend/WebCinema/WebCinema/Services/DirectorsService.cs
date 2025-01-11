using Microsoft.EntityFrameworkCore;
using WebCinema.Interfaces;
using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Services
{
    public class DirectorsService : IDirectorsService
    {
        private readonly WebCinemaDBContext _dbContext;
        private readonly ILogger<DirectorsService> _logger;

        public DirectorsService(WebCinemaDBContext dbContext, ILogger<DirectorsService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<DirectorGetDto> CreateDirectorAsync(DirectorCreateDto directorDto)
        {
            try
            {
                if (directorDto == null)
                {
                    throw new ArgumentNullException(nameof(directorDto));
                }

                var director = new Directors
                {
                    FirstName = directorDto.FirstName,
                    LastName = directorDto.LastName
                };

                await _dbContext.Directors.AddAsync(director);
                await _dbContext.SaveChangesAsync();

                return new DirectorGetDto
                {
                    Id = director.Id,
                    FirstName = director.FirstName,
                    LastName = director.LastName,
                    Movies = new List<MovieBriefDto>()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating director");
                throw;
            }
        }

        public async Task<List<DirectorGetDto>> GetAllDirectorsAsync()
        {
            try
            {
                return await _dbContext.Directors
                    .AsNoTracking()
                    .Select(d => new DirectorGetDto
                    {
                        Id = d.Id,
                        FirstName = d.FirstName,
                        LastName = d.LastName,
                        Movies = d.Movie.Select(m => new MovieBriefDto
                        {
                            Id = m.Id,
                            Title = m.Title
                        }).ToList()
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all directors");
                throw;
            }
        }

        public async Task<DirectorGetDto> GetDirectorByIdAsync(int id)
        {
            try
            {
                var director = await _dbContext.Directors
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .Select(d => new DirectorGetDto
                    {
                        Id = d.Id,
                        FirstName = d.FirstName,
                        LastName = d.LastName,
                        Movies = d.Movie.Select(m => new MovieBriefDto
                        {
                            Id = m.Id,
                            Title = m.Title
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

                if (director == null)
                {
                    throw new KeyNotFoundException($"Director with ID {id} not found");
                }

                return director;
            }
            catch (Exception ex) when (ex is not KeyNotFoundException)
            {
                _logger.LogError(ex, "Error retrieving director with ID {DirectorId}", id);
                throw;
            }
        }

        public async Task<DirectorGetDto> DeleteDirectorByIdAsync(int id)
        {
            try
            {
                var director = await _dbContext.Directors
                    .Include(d => d.Movie)
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (director == null)
                {
                    throw new KeyNotFoundException($"Director with ID {id} not found");
                }

                _dbContext.Directors.Remove(director);
                await _dbContext.SaveChangesAsync();

                return new DirectorGetDto
                {
                    Id = director.Id,
                    FirstName = director.FirstName,
                    LastName = director.LastName,
                    Movies = director.Movie.Select(m => new MovieBriefDto
                    {
                        Id = m.Id,
                        Title = m.Title
                    }).ToList()
                };
            }
            catch (Exception ex) when (ex is not KeyNotFoundException)
            {
                _logger.LogError(ex, "Error deleting director with ID {DirectorId}", id);
                throw;
            }
        }

        public async Task<DirectorGetDto> UpdateDirectorAsync(int id, DirectorUpdateDto directorDto)
        {
            try
            {
                if (directorDto == null)
                {
                    throw new ArgumentNullException(nameof(directorDto));
                }

                var director = await _dbContext.Directors
                    .Include(d => d.Movie)
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (director == null)
                {
                    throw new KeyNotFoundException($"Director with ID {id} not found");
                }

                director.FirstName = directorDto.FirstName;
                director.LastName = directorDto.LastName;

                _dbContext.Directors.Update(director);
                await _dbContext.SaveChangesAsync();

                return new DirectorGetDto
                {
                    Id = director.Id,
                    FirstName = director.FirstName,
                    LastName = director.LastName,
                    Movies = director.Movie.Select(m => new MovieBriefDto
                    {
                        Id = m.Id,
                        Title = m.Title
                    }).ToList()
                };
            }
            catch (Exception ex) when (ex is not KeyNotFoundException && ex is not ArgumentNullException)
            {
                _logger.LogError(ex, "Error updating director with ID {DirectorId}", id);
                throw;
            }
        }
    }
}