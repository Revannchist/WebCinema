
using Microsoft.EntityFrameworkCore;
using WebCinema.Interfaces;
//using WebCinema.Migrations;
using WebCinema.Models;

namespace WebCinema.Services
{
    public class CitiesService : ICitiesService
    {
        private readonly WebCinemaDBContext _dbContext;

        public CitiesService(WebCinemaDBContext dbContext)
        {
            _dbContext = dbContext;
        }
       

        public async Task<Cities> GetCitiesByIdAsync(int id)
        {
            var cities = await _dbContext.Cities.FirstOrDefaultAsync(c => c.Id == id);
            return cities;
        }

        public async Task<List<Cities>> GetCitiesAsync()
        {
            var cities=await _dbContext.Cities.ToListAsync();
            return cities;
        }
    }
}
