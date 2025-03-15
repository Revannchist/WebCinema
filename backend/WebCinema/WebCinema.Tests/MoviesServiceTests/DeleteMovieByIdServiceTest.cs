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
using WebCinema.Services;

namespace WebCinema.Tests.MoviesServiceTests
{
    [TestClass]
    public class DeleteMovieByIdServiceTest
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

            // Add movies
            var movies = new List<Movies>
            {
                new Movies
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
                },
                new Movies
                {
                    Id = 2,
                    Title = "Interstellar",
                    Description = "A team of explorers travel through a wormhole in space.",
                    ReleaseDate = new DateTime(2014, 11, 7),
                    Duration = 169,
                    Language = "English",
                    AgeRating = "PG-13",
                    DirectorId = 1,
                    CountryId = 1
                }
            };

            _dbContext.Movies.AddRange(movies);
            _dbContext.SaveChanges();

            // Add genre and actor relationships
            var movieGenres = new List<MoviesGenres>
            {
                new MoviesGenres { MovieId = 1, GenreId = 1 },
                new MoviesGenres { MovieId = 1, GenreId = 2 },
                new MoviesGenres { MovieId = 2, GenreId = 1 },
                new MoviesGenres { MovieId = 2, GenreId = 2 }
            };

            var movieActors = new List<MoviesActors>
            {
                new MoviesActors { MovieId = 1, ActorId = 1 },
                new MoviesActors { MovieId = 1, ActorId = 2 },
                new MoviesActors { MovieId = 2, ActorId = 1 }
            };

            _dbContext.MoviesGenres.AddRange(movieGenres);
            _dbContext.MoviesActors.AddRange(movieActors);
            _dbContext.SaveChanges();
        }

        [TestMethod]
        public async Task DeleteMovieByIdAsync_ExistingId_DeletesMovieAndReturnsIt()
        {
            // Arrange
            int movieId = 1;

            // Verify movie exists before deletion
            var movieBeforeDeletion = await _dbContext.Movies.FindAsync(movieId);
            Assert.IsNotNull(movieBeforeDeletion);

            // Act
            var result = await _moviesService.DeleteMovieByIdAsync(movieId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(movieId, result.Id);
            Assert.AreEqual("Inception", result.Title);

            // Verify movie was actually deleted from database
            var movieAfterDeletion = await _dbContext.Movies.FindAsync(movieId);
            Assert.IsNull(movieAfterDeletion);

            // Verify related entities were deleted (if you have cascading delete)
            var movieGenres = await _dbContext.MoviesGenres.Where(mg => mg.MovieId == movieId).ToListAsync();
            Assert.AreEqual(0, movieGenres.Count);

            var movieActors = await _dbContext.MoviesActors.Where(ma => ma.MovieId == movieId).ToListAsync();
            Assert.AreEqual(0, movieActors.Count);

            // Verify other movies are still in the database
            var otherMovie = await _dbContext.Movies.FindAsync(2);
            Assert.IsNotNull(otherMovie);

            // Verify logger was called with expected message
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("Movie deleted successfully: 1")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [TestMethod]
        public async Task DeleteMovieByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            int nonExistingId = 999;

            // Act
            var result = await _moviesService.DeleteMovieByIdAsync(nonExistingId);

            // Assert
            Assert.IsNull(result);

            // Verify logger was called with expected warning message
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString().Contains($"Attempt to delete non-existent movie: {nonExistingId}")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        //Delete Movie with Cancellation Request
        /*
        [TestMethod]
        public async Task DeleteMovieByIdAsync_CancellationRequested_ThrowsOperationCanceledException()
        {
            // Arrange
            int movieId = 1;
            var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act & Assert
            await Assert.ThrowsExceptionAsync<OperationCanceledException>(async () =>
            {
                await _moviesService.DeleteMovieByIdAsync(movieId, cts.Token);
            });
        }
        */

        //Delete Movie DB exception
        /*
        [TestMethod]
        public async Task DeleteMovieByIdAsync_DatabaseException_LogsErrorAndRethrows()
        {
            // Arrange
            int movieId = 1;

            // Create a mock DbContext that throws an exception during SaveChanges
            var mockDbContext = new Mock<WebCinemaDBContext>(new DbContextOptions<WebCinemaDBContext>());

            // Setup the Movies DbSet to return our test movie
            var testMovie = new Movies { Id = movieId, Title = "Test Movie" };
            var mockMoviesDbSet = new Mock<DbSet<Movies>>();
            mockMoviesDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(testMovie);

            mockDbContext.Setup(db => db.Movies).Returns(mockMoviesDbSet.Object);
            mockDbContext.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new DbUpdateException("Test database exception", new Exception()));

            // Create a service with the mocked context
            var serviceWithMockedDb = new MoviesService(mockDbContext.Object, _mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<DbUpdateException>(async () =>
            {
                await serviceWithMockedDb.DeleteMovieByIdAsync(movieId);
            });

            // Verify error was logged
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString().Contains($"Error deleting movie with ID: {movieId}")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
        */

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}