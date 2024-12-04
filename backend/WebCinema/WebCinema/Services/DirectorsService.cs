using WebCinema.Interfaces;
using WebCinema.Models;

namespace WebCinema.Services
{
    public class DirectorsService : IDirectorsService
    {
        private readonly WebCinemaDBContext _dbContext;

        public DirectorsService(WebCinemaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Directors CreateDirector(Directors director)
        {
            if (director == null)
            {
                return null;
            }
            _dbContext.Directors.Add(director);
            _dbContext.SaveChanges();
            return director;
        }

        public List<Directors> GetAllDirectors()
        {
            var directors = _dbContext.Directors.ToList();
            return directors;
        }

        public Directors GetDirectorById(int id)
        {
            var director = _dbContext.Directors.FirstOrDefault(x => x.Id == id);
            return director;
        }
        public Directors DeleteDirectorById(int id)
        {
            var director = GetDirectorById(id);
            if (director != null)
            {
                _dbContext.Directors.Remove(director);
                _dbContext.SaveChanges();
            }
            return director;
        }

        public Directors UpdateDirector(int id, Directors director)
        {
            var _director = GetDirectorById(id);
            if (director != null)
            {
                _director.FirstName = director.FirstName;
                _director.LastName = director.LastName;
                _dbContext.Directors.Update(_director);
                _dbContext.SaveChanges();
            }
            return _director;
        }
    }
}
