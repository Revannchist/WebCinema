using WebCinema.Models;

namespace WebCinema.Interfaces
{
    public interface IActorsService
    {
        Task<Actors> CreateActorAsync(Actors actors);

        Task<List<Actors>> GetAllActorsAsync();

        Task<Actors> GetActorByIdAsync(int id);

        Task<Actors> DeleteActorByIdAsync(int id);

        Task<Actors> UpdateActorsAsync(int id, Actors actors);
    }
}
