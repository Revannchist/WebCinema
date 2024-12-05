using Microsoft.EntityFrameworkCore;
using WebCinema.Interfaces;
using WebCinema.Models;

namespace WebCinema.Services
{
    public class ActorsService : IActorsService
    {
        private readonly WebCinemaDBContext _dbContext;
        public ActorsService(WebCinemaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Actors> CreateActorAsync(Actors actor)
        {
            if (actor == null)
            {
                return null;
            }
            await _dbContext.Actors.AddAsync(actor);
            await _dbContext.SaveChangesAsync();
            return actor;
        }

        public async Task<List<Actors>> GetAllActorsAsync()
        {
            var actors = await _dbContext.Actors.ToListAsync();
            return actors;
        }

        public async Task<Actors> GetActorByIdAsync(int id)
        {
            var actor = await _dbContext.Actors.FirstOrDefaultAsync(x => x.Id == id);
            return actor;
        }

        public async Task<Actors> DeleteActorByIdAsync(int id)
        {
            var actor = await GetActorByIdAsync(id);
            if (actor != null)
            {
                _dbContext.Actors.Remove(actor);
                await _dbContext.SaveChangesAsync();
            }
            return actor;
        }

        public async Task<Actors> UpdateActorsAsync(int id, Actors actor)
        {
            var _actor = await GetActorByIdAsync(id);
            if (actor != null)
            {
                _actor.FirstName = actor.FirstName;
                _actor.LastName = actor.LastName;
                _dbContext.Actors.Update(_actor);
                await _dbContext.SaveChangesAsync();
            }
            return _actor;
        }
    }
}
