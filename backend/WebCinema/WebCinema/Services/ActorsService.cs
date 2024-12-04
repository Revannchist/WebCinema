using WebCinema.Interfaces;
using WebCinema.Models;

namespace WebCinema.Services
{
    public class ActorsService : IActorsService
    {
        private readonly WebCinemaDBContext _dbContext;

        public ActorsService(WebCinemaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Actors CreateActor(Actors actor)
        {
            if (actor == null)
            {
                return null;
            }
            _dbContext.Actors.Add(actor);
            _dbContext.SaveChanges();
            return actor;
        }

        public List<Actors> GetAllActors()
        {
            var actors = _dbContext.Actors.ToList();
            return actors;
        }

        public Actors GetActorById(int id)
        {
            var actor = _dbContext.Actors.FirstOrDefault(x => x.Id == id);
            return actor;
        }

        public Actors DeleteActorById(int id)
        {
            var actor = GetActorById(id);
            if (actor != null)
            {
                _dbContext.Actors.Remove(actor);
                _dbContext.SaveChanges();
            }
            return actor;
        }

        public Actors UpdateActor(int id, Actors actor)
        {
            var _actor = GetActorById(id);
            if (actor != null)
            {
                _actor.FirstName = actor.FirstName;
                _actor.LastName = actor.LastName;
                _dbContext.Actors.Update(_actor);
                _dbContext.SaveChanges();
            }
            return _actor;
        }
    }
}
