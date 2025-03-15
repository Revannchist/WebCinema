using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebCinema.Models;
using WebCinema.Models.DTO;
using WebCinema.Services;

namespace WebCinema.Tests.MoviesServiceTests
{
    [TestClass]
    public class GetMovieByIdServiceTest
    {
        private WebCinemaDBContext _dbContext;
        private Mock<ILogger<MoviesService>> _mockLogger;
        private MoviesService _moviesService;

        [TestInitialize]
        public void Setup()
        {
            // Setup in-memory database
            var options = new DbContextOptionsBuilder<WebCinemaDBContext>()
                .UseInMemoryDatabase(databaseName: $"TestMoviesDb_{Guid.NewGuid()}")
                .Options;

            _dbContext = new WebCinemaDBContext(options);

            // Setup test data
            SeedTestData();

            // Setup logger
            _mockLogger = new Mock<ILogger<MoviesService>>();

            // Create the service
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

            // Add a movie with relationships
            var movie = new Movies
            {
                Id = 1,
                Title = "Inception",
                Description = "A thief who steals corporate secrets through dream-sharing technology.",
                ReleaseDate = new DateTime(2010, 7, 16),
                Duration = 148,
                Language = "English",
                AgeRating = "PG-13",
                DirectorId = 1,
                CountryId = 1
            };

            _dbContext.Movies.Add(movie);
            _dbContext.SaveChanges();

            // Add genre and actor relationships
            var movieGenres = new List<MoviesGenres>
            {
                new MoviesGenres { MovieId = 1, GenreId = 1 },
                new MoviesGenres { MovieId = 1, GenreId = 2 }
            };

            var movieActors = new List<MoviesActors>
            {
                new MoviesActors { MovieId = 1, ActorId = 1 },
                new MoviesActors { MovieId = 1, ActorId = 2 }
            };

            _dbContext.MoviesGenres.AddRange(movieGenres);
            _dbContext.MoviesActors.AddRange(movieActors);
            _dbContext.SaveChanges();
        }

        [TestMethod]
        public async Task GetMovieByIdAsync_ExistingId_ReturnsCorrectMovie()
        {
            // Arrange
            int movieId = 1;

            // Act
            var result = await _moviesService.GetMovieByIdAsync(movieId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(movieId, result.Id);
            Assert.AreEqual("Inception", result.Title);
            Assert.AreEqual("A thief who steals corporate secrets through dream-sharing technology.", result.Description);
            Assert.AreEqual(new DateTime(2010, 7, 16), result.ReleaseDate);
            Assert.AreEqual(148, result.Duration);
            Assert.AreEqual("English", result.Language);
            Assert.AreEqual("PG-13", result.AgeRating);

            // Check director
            Assert.IsNotNull(result.DirectorId);
            Assert.AreEqual(1, result.DirectorId.Id);
            Assert.AreEqual("Christopher", result.DirectorId.FirstName);
            Assert.AreEqual("Nolan", result.DirectorId.LastName);

            // Check country
            Assert.IsNotNull(result.CountryId);
            Assert.AreEqual(1, result.CountryId.Id);
            Assert.AreEqual("USA", result.CountryId.Name);

            // Check genres
            Assert.IsNotNull(result.MoviesGenresIds);
            Assert.AreEqual(2, result.MoviesGenresIds.Count);
            CollectionAssert.Contains(result.MoviesGenresIds, 1);
            CollectionAssert.Contains(result.MoviesGenresIds, 2);

            // Check actors
            Assert.IsNotNull(result.MoviesActorsIds);
            Assert.AreEqual(2, result.MoviesActorsIds.Count);
            CollectionAssert.Contains(result.MoviesActorsIds, 1);
            CollectionAssert.Contains(result.MoviesActorsIds, 2);
        }

        [TestMethod]
        public async Task GetMovieByIdAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            // Arrange
            int nonExistingId = 999;

            // Act & Assert
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () =>
            {
                await _moviesService.GetMovieByIdAsync(nonExistingId);
            });
        }

        [TestMethod]
        public async Task GetMovieByIdAsync_CancellationRequested_ThrowsOperationCanceledException()
        {
            // Arrange
            int movieId = 1;
            var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act & Assert
            await Assert.ThrowsExceptionAsync<OperationCanceledException>(async () =>
            {
                await _moviesService.GetMovieByIdAsync(movieId, cts.Token);
            });
        }

        [TestMethod]
        public async Task GetMovieByIdAsync_MovieWithoutDirector_ReturnsNullDirector()
        {
            // Arrange
            // Add a movie without a director
            var movieWithoutDirector = new Movies
            {
                Id = 2,
                Title = "Independent Film",
                Description = "A film with no director specified.",
                ReleaseDate = new DateTime(2020, 1, 1),
                Duration = 90,
                Language = "English",
                AgeRating = "PG",
                DirectorId = null,
                CountryId = 1
            };

            _dbContext.Movies.Add(movieWithoutDirector);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _moviesService.GetMovieByIdAsync(2);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.IsNull(result.DirectorId);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}