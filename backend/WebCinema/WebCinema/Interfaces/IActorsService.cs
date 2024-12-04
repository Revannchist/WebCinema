using WebCinema.Models;

namespace WebCinema.Interfaces
{
    public interface IActorsService
    {
        Actors CreateActor(Actors actors);

        List<Actors> GetAllActors();

        Actors GetActorById(int id);

        Actors DeleteActorById(int id);

        Actors UpdateActor(int id, Actors actors);
    }
}
