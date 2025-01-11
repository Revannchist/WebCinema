using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Interfaces
{
    public interface IActorsService
    {
        Task<ActorGetDto> CreateActorAsync(ActorCreateDto actorDto);

        Task<List<ActorGetDto>> GetAllActorsAsync();

        Task<ActorGetDto> GetActorByIdAsync(int id);

        Task<ActorGetDto> DeleteActorByIdAsync(int id);

        Task<ActorGetDto> UpdateActorsAsync(int id, ActorUpdateDto actorDto);
    }
}
