using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using WebCinema.Interfaces;
using WebCinema.Models;

namespace WebCinema.Services
{
    public class TheatersService : ITheatersService
    {
        private readonly WebCinemaDBContext _dbContext;
        private readonly ICitiesService _citiesService;
        public TheatersService(WebCinemaDBContext dbContext,ICitiesService citiesService)
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
            var theaters = await GetTheatersByIdAsync(id);
            if (theaters != null)
            {
                _dbContext.Theaters.Remove(theaters);
                await _dbContext.SaveChangesAsync();
            }
            return theaters;
        }

        public async Task<List<Theaters>> GetAllTheatersAsync()
        {
            var theaters = await _dbContext.Theaters.ToListAsync();

            return theaters;
        }



        public async Task<Theaters> GetTheatersByIdAsync(int id)
        {
            var theaters = await _dbContext.Theaters.FirstOrDefaultAsync(x => x.Id == id);
            theaters.City = await _citiesService.GetCitiesByIdAsync(theaters.CityId);
            return theaters;
        }

        public async Task<Theaters> UpdateTheatersAsync(int id, Theaters theaters)
        {
            var _theaters = await GetTheatersByIdAsync(id);
            if (theaters != null)
            {
                _theaters.Name = theaters.Name;
                _theaters.Adress=theaters.Adress;
                _theaters.PostalCode = theaters.PostalCode;
                _theaters.PhoneNumber= theaters.PhoneNumber;
                _dbContext.Theaters.Update(_theaters);
                await _dbContext.SaveChangesAsync();
            }
            return _theaters;
        }
    }
}
