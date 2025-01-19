using Microsoft.EntityFrameworkCore;
using WebCinema.Interfaces;
using WebCinema.Models;
using WebCinema.Models.DTO;

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

        public async Task<HallDisplayDto> DeleteHallsByIdAsync(int id)
        {
            var hall = await _dbContext.Halls
         .Include(h => h.Theater)
         .FirstOrDefaultAsync(x => x.Id == id);

            if (hall != null)
            {
                _dbContext.Halls.Remove(hall);
                await _dbContext.SaveChangesAsync();

                return new HallDisplayDto
                {
                    Id = hall.Id,
                    TheatersID = hall.TheatersID,
                    TheaterName = hall.Theater?.Name,
                    HallName = hall.HallName,
                    Capacity = hall.Capacity,
                    HallType = hall.HallType
                };
            }
            return null;
        }

        public async Task<List<HallDisplayDto>> GetAllHallsAsync()
        {
            return await _dbContext.Halls
        .Include(h => h.Theater)
        .Select(h => new HallDisplayDto
        {
            Id = h.Id,
            TheatersID = h.TheatersID,
            TheaterName = h.Theater.Name, 
            HallName = h.HallName,
            Capacity = h.Capacity,
            HallType = h.HallType
        })
        .ToListAsync();
        }

        public async Task<Halls> GetHallEntityByIdAsync(int id)
        {
           
                return await _dbContext.Halls
                    .Include(h => h.Theater)
                    .FirstOrDefaultAsync(h => h.Id == id);
            
        }

        public async Task<HallDisplayDto> GetHallsByIdAsync(int id)
        {
            return await _dbContext.Halls
       .Include(h => h.Theater)
       .Where(h => h.Id == id)
       .Select(h => new HallDisplayDto
       {
           Id = h.Id,
           TheatersID = h.TheatersID,
           TheaterName = h.Theater.Name, 
           HallName = h.HallName,
           Capacity = h.Capacity,
           HallType = h.HallType
       })
       .FirstOrDefaultAsync();
        }

        public async Task<HallDisplayDto> UpdateHallsAsync(int id, Halls halls)
        {
            var hall = await _dbContext.Halls
         .Include(h => h.Theater)
         .FirstOrDefaultAsync(x => x.Id == id);

            if (hall != null)
            {
                hall.HallName = halls.HallName;
                hall.Capacity = halls.Capacity;
                hall.HallType = halls.HallType;

                _dbContext.Halls.Update(hall);
                await _dbContext.SaveChangesAsync();

                return new HallDisplayDto
                {
                    Id = hall.Id,
                    TheatersID = hall.TheatersID,
                    TheaterName = hall.Theater?.Name,
                    HallName = hall.HallName,
                    Capacity = hall.Capacity,
                    HallType = hall.HallType
                };
            }
            return null;
        }
    }
}
