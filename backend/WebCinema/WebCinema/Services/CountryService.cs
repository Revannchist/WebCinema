using WebCinema.Interfaces;
using WebCinema.Models;

namespace WebCinema.Services
{
    public class CountryService : ICountryService
    {
        private readonly WebCinemaDBContext _dbContext;
        public CountryService(WebCinemaDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Country CreateCountry(Country country)
        {
            if(country == null)
            {
                return null;
            }
            _dbContext.Countries.Add(country);
            _dbContext.SaveChanges();
            return country;
        }

        public List<Country> GetAllCountries()
        {
            var countries =  _dbContext.Countries.ToList();
            return countries;
        }

        public Country GetCountryById(int id) 
        {
            var country = _dbContext.Countries.FirstOrDefault(x => x.Id == id);
            return country;
        }
        public Country DeleteCountryById(int id)
        {
            var country = GetCountryById(id);
            if(country != null)
            {
                _dbContext.Countries.Remove(country);
                _dbContext.SaveChanges();
            }
            return country;
        }

        public Country UpdateCountry(int id, Country country)
        {
            var _country = GetCountryById(id);
            if(country!= null)
            {
                _country.Name = country.Name;
                _dbContext.Countries.Update(_country);
                _dbContext.SaveChanges();
            }
            return _country;
        }
    }
}