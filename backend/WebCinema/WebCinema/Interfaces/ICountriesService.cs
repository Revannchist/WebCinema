using WebCinema.Models;

namespace WebCinema.Interfaces
{
    public interface ICountriesService
    {
        Countries CreateCountry(Countries country);

        List<Countries> GetAllCountries();

        Countries GetCountryById(int id);

        Countries DeleteCountryById(int id);

        Countries UpdateCountry(int id, Countries country);
    }
}