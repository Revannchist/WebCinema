using Microsoft.EntityFrameworkCore;
using WebCinema.Interfaces;
using WebCinema.Models;

namespace WebCinema.Services
{
    public class CountriesService : ICountriesService
    {
        private readonly WebCinemaDBContext _dbContext;
        public CountriesService(WebCinemaDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Countries> CreateCountryAsync(Countries country)
        {
            if(country == null)
            {
                return null;
            }
            await _dbContext.Countries.AddAsync(country);
            await _dbContext.SaveChangesAsync();
            return country;
        }

        public async Task<List<Countries>> GetAllCountriesAsync()
        {
            var countries =  await _dbContext.Countries.ToListAsync();
            return countries;
        }

        public async Task<Countries> GetCountryByIdAsync(int id) 
        {
            var country = await _dbContext.Countries.FirstOrDefaultAsync(x => x.Id == id);
            return country;
        }
        public async Task<Countries> DeleteCountryByIdAsync(int id)
        {
            var country = await GetCountryByIdAsync(id);
            if(country != null)
            {
                _dbContext.Countries.Remove(country);
                await _dbContext.SaveChangesAsync();
            }
            return country;
        }

        public async Task<Countries> UpdateCountryAsync(int id, Countries country)
        {
            var _country = await GetCountryByIdAsync(id);
            if(country!= null)
            {
                _country.Name = country.Name;
                _dbContext.Countries.Update(_country);
                await _dbContext.SaveChangesAsync();
            }
            return _country;
        }
    }
}