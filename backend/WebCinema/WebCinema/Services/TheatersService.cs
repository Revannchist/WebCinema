using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using WebCinema.Interfaces;
using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Services
{
    public class TheatersService : ITheatersService
    {
        private readonly WebCinemaDBContext _dbContext;
        private readonly ICitiesService _citiesService;

        public TheatersService(WebCinemaDBContext dbContext, ICitiesService citiesService)
        {
            _dbContext = dbContext;
            _citiesService = citiesService;
        }

        public async Task<Theaters> CreateTheatersAsync(Theaters theaters)
        {
            if (theaters == null)
            {
                return null;
            }
            await _dbContext.Theaters.AddAsync(theaters);
            await _dbContext.SaveChangesAsync();
            return theaters;
        }

        public async Task<Theaters> DeleteTheatersByIdAsync(int id)
        {
            var theaters = await _dbContext.Theaters.FirstOrDefaultAsync(x => x.Id == id);
            if (theaters != null)
            {
                _dbContext.Theaters.Remove(theaters);
                await _dbContext.SaveChangesAsync();
            }
            return theaters;
        }

        public async Task<List<TheaterDto>> GetAllTheatersAsync()
        {
            var theaters = await _dbContext.Theaters
                .Include(t => t.City)
                .Select(t => new TheaterDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    CityId = t.CityId,
                    CityName = t.City.Name,
                    Adress = t.Adress,
                    PostalCode = t.PostalCode,
                    PhoneNumber = t.PhoneNumber
                })
                .ToListAsync();

            return theaters;
        }

        public async Task<TheaterDto> GetTheatersByIdAsync(int id)
        {
            var theater = await _dbContext.Theaters
                .Include(t => t.City)
                .Where(x => x.Id == id)
                .Select(t => new TheaterDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    CityId = t.CityId,
                    CityName = t.City.Name,
                    Adress = t.Adress,
                    PostalCode = t.PostalCode,
                    PhoneNumber = t.PhoneNumber
                })
                .FirstOrDefaultAsync();

            return theater;
        }

        public async Task<Theaters> UpdateTheatersAsync(int id, Theaters theaters)
        {
            var _theaters = await _dbContext.Theaters.FirstOrDefaultAsync(x => x.Id == id);
            if (_theaters != null)
            {
                _theaters.Name = theaters.Name;
                _theaters.Adress = theaters.Adress;
                _theaters.PostalCode = theaters.PostalCode;
                _theaters.PhoneNumber = theaters.PhoneNumber;
                _dbContext.Theaters.Update(_theaters);
                await _dbContext.SaveChangesAsync();
            }
            return _theaters;
        }
    }
}
