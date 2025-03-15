using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using WebCinema.Models;
using WebCinema.Models.DTO;
using WebCinema.Services;

namespace WebCinema.Tests.MoviesServiceTests
{
    [TestClass]
    public class AddMovieServiceTest
    {
        private WebCinemaDBContext _dbContext;
        private Mock<ILogger<MoviesService>> _mockLogger;
        private MoviesService _moviesService;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<WebCinemaDBContext>()
                .UseInMemoryDatabase(databaseName: $"TestMoviesDb_{Guid.NewGuid()}")
                .Options;

            _dbContext = new WebCinemaDBContext(options);

            SeedTestData();

            _mockLogger = new Mock<ILogger<MoviesService>>();

            _moviesService = new MoviesService(_dbContext, _mockLogger.Object);
        }

        private void SeedTestData()
        {
            // Add a director
            var director = new Directors
            {
                Id = 1,
                FirstName = "Christopher",
                LastName = "Nolan"
            };
            _dbContext.Directors.Add(director);

            // Add a country
            var country = new Countries
            {
                Id = 1,
                Name = "USA"
            };
            _dbContext.Countries.Add(country);

            // Add genres
            var genres = new List<Genres>
            {
                new Genres { Id = 1, Name = "Action" },
                new Genres { Id = 2, Name = "Sci-Fi" }
            };
            _dbContext.Genres.AddRange(genres);

            // Add actors
            var actors = new List<Actors>
            {
                new Actors { Id = 1, FirstName = "Leonardo", LastName = "DiCaprio" },
                new Actors { Id = 2, FirstName = "Ellen", LastName = "Page" }
            };
            _dbContext.Actors.AddRange(actors);

            _dbContext.SaveChanges();
        }

        [TestMethod]
        public async Task CreateMovieAsync_ValidMovie_ReturnsCorrectResponseDto()
        {
            // Arrange
            var movieDto = new MovieCreateDto
            {
                Title = "Inception",
                Description = "A thief who steals corporate secrets through dream-sharing technology.",
                ReleaseDate = new DateTime(2010, 7, 16),
                Duration = 148,
                Language = "English",
                AgeRating = "PG-13",
                DirectorId = 1,
                CountryId = 1,
                GenreIds = new List<int> { 1, 2 },
                ActorIds = new List<int> { 1, 2 }
            };

            // Act
            var result = await _moviesService.CreateMovieAsync(movieDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Inception", result.Title);
            Assert.AreEqual("A thief who steals corporate secrets through dream-sharing technology.", result.Description);
            Assert.AreEqual(new DateTime(2010, 7, 16), result.ReleaseDate);
            Assert.AreEqual(148, result.Duration);
            Assert.AreEqual("English", result.Language);
            Assert.AreEqual("PG-13", result.AgeRating);

            // Check director
            Assert.IsNotNull(result.Director);
            Assert.AreEqual(1, result.Director.Id);

            // Check country
            Assert.IsNotNull(result.Country);
            Assert.AreEqual(1, result.Country.Id);

            // Check genres
            Assert.IsNotNull(result.Genres);
            Assert.AreEqual(2, result.Genres.Count);
            CollectionAssert.Contains(result.Genres.Select(g => g.Id).ToList(), 1);
            CollectionAssert.Contains(result.Genres.Select(g => g.Id).ToList(), 2);

            // Check actors
            Assert.IsNotNull(result.Actors);
            Assert.AreEqual(2, result.Actors.Count);
            CollectionAssert.Contains(result.Actors.Select(a => a.Id).ToList(), 1);
            CollectionAssert.Contains(result.Actors.Select(a => a.Id).ToList(), 2);

            // Verify movie was actually added to database
            var savedMovie = await _dbContext.Movies.FindAsync(result.Id);
            Assert.IsNotNull(savedMovie);
            Assert.AreEqual("Inception", savedMovie.Title);

            // Verify relationships were created correctly
            var movieGenres = await _dbContext.MoviesGenres
                .Where(mg => mg.MovieId == result.Id)
                .ToListAsync();
            Assert.AreEqual(2, movieGenres.Count);

            var movieActors = await _dbContext.MoviesActors
                .Where(ma => ma.MovieId == result.Id)
                .ToListAsync();
            Assert.AreEqual(2, movieActors.Count);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}