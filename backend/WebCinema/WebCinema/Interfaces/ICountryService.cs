using WebCinema.Models;

namespace WebCinema.Interfaces
{
    public interface ICountryService
    {
        Country CreateCountry(Country country);

        List<Country> GetAllCountries();

        Country GetCountryById(int id);

        Country DeleteCountryById(int id);

        Country UpdateCountry(int id, Country country);
    }
}