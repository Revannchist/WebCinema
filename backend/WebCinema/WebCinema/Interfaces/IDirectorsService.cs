using WebCinema.Models;

namespace WebCinema.Interfaces
{
    public interface IDirectorsService
    {
        Task<Directors> CreateDirectorAsync(Directors director);

        Task<List<Directors>> GetAllDirectorsAsync();

        Task<Directors> GetDirectorByIdAsync(int id);

        Task<Directors> DeleteDirectorByIdAsync(int id);

        Task<Directors> UpdateDirectorAsync(int id, Directors directors);
    }
}