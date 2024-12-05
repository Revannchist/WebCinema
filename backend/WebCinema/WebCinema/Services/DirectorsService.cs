using Microsoft.EntityFrameworkCore;
using WebCinema.Interfaces;
using WebCinema.Models;

namespace WebCinema.Services
{
    public class DirectorsService : IDirectorsService
    {
        private readonly WebCinemaDBContext _dbContext;

        public DirectorsService(WebCinemaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Directors> CreateDirectorAsync(Directors director)
        {
            if (director == null)
            {
                return null;
            }
            await _dbContext.Directors.AddAsync(director);
            await _dbContext.SaveChangesAsync();
            return director;
        }

        public async Task<List<Directors>> GetAllDirectorsAsync()
        {
            var directors = await _dbContext.Directors.ToListAsync();
            return directors;
        }

        public async Task<Directors> GetDirectorByIdAsync(int id)
        {
            var director = await _dbContext.Directors.FirstOrDefaultAsync(x => x.Id == id);
            return director;
        }
        public async Task<Directors> DeleteDirectorByIdAsync(int id)
        {
            var director = await GetDirectorByIdAsync(id);
            if (director != null)
            {
                _dbContext.Directors.Remove(director);
                await _dbContext.SaveChangesAsync();
            }
            return director;
        }

        public async Task<Directors> UpdateDirectorAsync(int id, Directors director)
        {
            var _director = await GetDirectorByIdAsync(id);
            if (director != null)
            {
                _director.FirstName = director.FirstName;
                _director.LastName = director.LastName;
                _dbContext.Directors.Update(_director);
                await _dbContext.SaveChangesAsync();
            }
            return _director;
        }
    }
}
