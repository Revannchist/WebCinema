using Microsoft.EntityFrameworkCore;
using WebCinema.Interfaces;
using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Services
{
    public class ActorsService : IActorsService
    {
        private readonly WebCinemaDBContext _dbContext;
        private readonly ILogger<DirectorsService> _logger;

        public ActorsService(WebCinemaDBContext dbContext, ILogger<DirectorsService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ActorGetDto> CreateActorAsync(ActorCreateDto actorDto)
        {
            try
            {
                if (actorDto == null)
                {
                    throw new ArgumentNullException(nameof(actorDto));
                }

                var existingActor = await _dbContext.Actors
                    .FirstOrDefaultAsync(a =>
                        a.FirstName.ToLower() == actorDto.FirstName.ToLower() &&
                        a.LastName.ToLower() == actorDto.LastName.ToLower());

                if (existingActor != null)
                {
                    throw new InvalidOperationException($"Actor with name {actorDto.FirstName} {actorDto.LastName} already exists");
                }

                var actor = new Actors
                {
                    FirstName = actorDto.FirstName,
                    LastName = actorDto.LastName
                };

                await _dbContext.Actors.AddAsync(actor);
                await _dbContext.SaveChangesAsync();

                return new ActorGetDto
                {
                    Id = actor.Id,
                    FirstName = actor.FirstName,
                    LastName = actor.LastName,
                    Movies = new List<MovieBriefDto>()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating actor");
                throw;
            }
        }

        public async Task<List<ActorGetDto>> GetAllActorsAsync()
        {
            try
            {
                return await _dbContext.Actors
                    .AsNoTracking()
                    .Select(a => new ActorGetDto
                    {
                        Id = a.Id,
                        FirstName = a.FirstName,
                        LastName = a.LastName,
                        Movies = a.MoviesActors.Select(ma => new MovieBriefDto
                        {
                            Id = ma.Movie.Id,
                            Title = ma.Movie.Title
                        }).ToList()
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all actors");
                throw;
            }
        }

        public async Task<ActorGetDto> GetActorByIdAsync(int id)
        {
            try
            {
                var actor = await _dbContext.Actors
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .Select(a => new ActorGetDto
                    {
                        Id = a.Id,
                        FirstName = a.FirstName,
                        LastName = a.LastName,
                        Movies = a.MoviesActors.Select(ma => new MovieBriefDto
                        {
                            Id = ma.Movie.Id,
                            Title = ma.Movie.Title
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

                if (actor == null)
                {
                    throw new KeyNotFoundException($"Actor with ID {id} not found");
                }

                return actor;
            }
            catch (Exception ex) when (ex is not KeyNotFoundException)
            {
                _logger.LogError(ex, "Error retrieving actor with ID {ActorId}", id);
                throw;
            }
        }

        public async Task<ActorGetDto> DeleteActorByIdAsync(int id)
        {
            try
            {
                var actor = await _dbContext.Actors
                    .Include(a => a.MoviesActors)
                        .ThenInclude(ma => ma.Movie)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (actor == null)
                {
                    throw new KeyNotFoundException($"Actor with ID {id} not found");
                }

                _dbContext.Actors.Remove(actor);
                await _dbContext.SaveChangesAsync();

                return new ActorGetDto
                {
                    Id = actor.Id,
                    FirstName = actor.FirstName,
                    LastName = actor.LastName,
                    Movies = actor.MoviesActors.Select(ma => new MovieBriefDto
                    {
                        Id = ma.Movie.Id,
                        Title = ma.Movie.Title
                    }).ToList()
                };
            }
            catch (Exception ex) when (ex is not KeyNotFoundException)
            {
                _logger.LogError(ex, "Error deleting actor with ID {ActorId}", id);
                throw;
            }
        }

        public async Task<ActorGetDto> UpdateActorsAsync(int id, ActorUpdateDto actorDto)
        {
            try
            {
                if (actorDto == null)
                {
                    throw new ArgumentNullException(nameof(actorDto));
                }

                var actor = await _dbContext.Actors
                    .Include(a => a.MoviesActors)
                        .ThenInclude(ma => ma.Movie)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (actor == null)
                {
                    throw new KeyNotFoundException($"Actor with ID {id} not found");
                }

                // Check for existing actor with same name, excluding the current actor
                var existingActor = await _dbContext.Actors
                    .FirstOrDefaultAsync(a =>
                        a.Id != id &&
                        a.FirstName.ToLower() == actorDto.FirstName.ToLower() &&
                        a.LastName.ToLower() == actorDto.LastName.ToLower());

                if (existingActor != null)
                {
                    throw new InvalidOperationException($"Actor with name {actorDto.FirstName} {actorDto.LastName} already exists");
                }

                actor.FirstName = actorDto.FirstName;
                actor.LastName = actorDto.LastName;

                _dbContext.Actors.Update(actor);
                await _dbContext.SaveChangesAsync();

                return new ActorGetDto
                {
                    Id = actor.Id,
                    FirstName = actor.FirstName,
                    LastName = actor.LastName,
                    Movies = actor.MoviesActors.Select(ma => new MovieBriefDto
                    {
                        Id = ma.Movie.Id,
                        Title = ma.Movie.Title
                    }).ToList()
                };
            }
            catch (Exception ex) when (ex is not KeyNotFoundException && ex is not ArgumentNullException)
            {
                _logger.LogError(ex, "Error updating actor with ID {ActorId}", id);
                throw;
            }
        }
    }
}
