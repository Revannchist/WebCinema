using WebCinema.Models;

namespace WebCinema.Interfaces
{
    public interface ICountriesService
    {
        Task<Countries> CreateCountryAsync(Countries country);

        Task<List<Countries>> GetAllCountriesAsync();

        Task<Countries> GetCountryByIdAsync(int id);

        Task<Countries> DeleteCountryByIdAsync(int id);

        Task<Countries> UpdateCountryAsync(int id, Countries country);
    }
}