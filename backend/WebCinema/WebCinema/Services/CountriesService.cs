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
        public Countries CreateCountry(Countries country)
        {
            if(country == null)
            {
                return null;
            }
            _dbContext.Countries.Add(country);
            _dbContext.SaveChanges();
            return country;
        }

        public List<Countries> GetAllCountries()
        {
            var countries =  _dbContext.Countries.ToList();
            return countries;
        }

        public Countries GetCountryById(int id) 
        {
            var country = _dbContext.Countries.FirstOrDefault(x => x.Id == id);
            return country;
        }
        public Countries DeleteCountryById(int id)
        {
            var country = GetCountryById(id);
            if(country != null)
            {
                _dbContext.Countries.Remove(country);
                _dbContext.SaveChanges();
            }
            return country;
        }

        public Countries UpdateCountry(int id, Countries country)
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