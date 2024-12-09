using Microsoft.EntityFrameworkCore;
using WebCinema.Interfaces;
using WebCinema.Models;

namespace WebCinema.Services
{
    public class HallsService : IHallsService
    {
        private readonly WebCinemaDBContext _dbContext;
        private readonly ITheatersService _theatersService;
        public HallsService(WebCinemaDBContext dbContext,ITheatersService theatersservice)
        {
            _dbContext = dbContext;
            _theatersService = theatersservice;
        }
        public async Task<Halls> CreateHallsAsync(Halls halls)
        {
            if (halls == null)
            {
                return null;
            }
            await _dbContext.Halls.AddAsync(halls);
            await _dbContext.SaveChangesAsync();
            return halls;
        }

        public async Task<Halls> DeleteHallsByIdAsync(int id)
        {
            var halls = await GetHallsByIdAsync(id);
            if (halls != null)
            {
                _dbContext.Halls.Remove(halls);
                await _dbContext.SaveChangesAsync();
            }
            return halls;
        }

        public async Task<List<Halls>> GetAllHallsAsync()
        {
            var halls = await _dbContext.Halls.ToListAsync();
            return halls;
        }

        public async Task<Halls> GetHallsByIdAsync(int id)
        {
            var halls = await _dbContext.Halls.FirstOrDefaultAsync(x => x.Id == id);
            halls.Theater=await _theatersService.GetTheatersByIdAsync(halls.TheatersID);
            return halls;
        }

        public async Task<Halls> UpdateHallsAsync(int id, Halls halls)
        {
            var _halls = await GetHallsByIdAsync(id);
            if (halls != null)
            {
                _halls.HallName = halls.HallName;
                _halls.Capacity= halls.Capacity;
                _halls.HallType=halls.HallType;
                _dbContext.Halls.Update(_halls);
                await _dbContext.SaveChangesAsync();
            }
            return _halls;
        }
    }
}
