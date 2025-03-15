using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebCinema.Models;
using WebCinema.Models.DTO;
using WebCinema.Services;

namespace WebCinema.Tests.MoviePosterServiceTests
{
    [TestClass]
    public class CreateMoviePosterServiceTest
    {
        private WebCinemaDBContext _dbContext;
        private Mock<ILogger<MoviePosterService>> _mockLogger;
        private MoviePosterService _moviePosterService;
        private int _existingMovieId;

        [TestInitialize]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<WebCinemaDBContext>()
                .UseInMemoryDatabase(databaseName: $"TestMoviesPosterDb_{Guid.NewGuid()}")
                .Options;

            _dbContext = new WebCinemaDBContext(options);

            await SeedTestData();

            _mockLogger = new Mock<ILogger<MoviePosterService>>();

            _moviePosterService = new MoviePosterService(_dbContext, _mockLogger.Object);
        }

        private async Task SeedTestData()
        {
            // Add a movie
            var movie = new Movies
            {
                Title = "Test Movie",
                Description = "Test Description",
                ReleaseDate = new DateTime(2022, 1, 1),
                Duration = 120,
                Language = "English",
                AgeRating = "PG-13"
            };
            _dbContext.Movies.Add(movie);
            await _dbContext.SaveChangesAsync();

            _existingMovieId = movie.Id;

            // Add an existing poster for one test case
            var existingPoster = new MoviePoster
            {
                MovieId = _existingMovieId,
                PosterImage = Convert.FromBase64String("dGVzdGltYWdl"), // "testimage" in base64
                ImageFormat = "data:image/jpeg;base64,"
            };
            _dbContext.MoviePoster.Add(existingPoster);
            await _dbContext.SaveChangesAsync();
        }

        [TestMethod]
        public async Task CreateMoviePosterAsync_NewPoster_ReturnsTrue()
        {
            // Arrange
            var newMovieId = _existingMovieId + 1;

            // Add another movie without a poster
            var newMovie = new Movies
            {
                Id = newMovieId,
                Title = "Another Test Movie",
                Description = "Another Test Description",
                ReleaseDate = new DateTime(2022, 2, 1),
                Duration = 120,
                Language = "English",
                AgeRating = "PG-13"
            };
            _dbContext.Movies.Add(newMovie);
            await _dbContext.SaveChangesAsync();

            var posterDto = new MovieCreatePosterDto
            {
                MovieId = newMovieId,
                Image = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg=="
            };

            // Act
            var result = await _moviePosterService.CreateMoviePosterAsync(posterDto, CancellationToken.None);

            // Assert
            Assert.IsTrue(result);

            // Verify poster was added to database
            var poster = await _dbContext.MoviePoster.FirstOrDefaultAsync(p => p.MovieId == newMovieId);
            Assert.IsNotNull(poster);
            Assert.AreEqual(newMovieId, poster.MovieId);
            Assert.AreEqual("data:image/png;base64,", poster.ImageFormat);

            // Check if the base64 image was correctly converted to bytes
            var expectedBytes = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg==");
            CollectionAssert.AreEqual(expectedBytes, poster.PosterImage);
        }

        [TestMethod]
        public async Task CreateMoviePosterAsync_ReplacesExistingPoster_ReturnsTrue()
        {
            // Arrange
            var posterDto = new MovieCreatePosterDto
            {
                MovieId = _existingMovieId,
                Image = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg=="
            };

            // Act
            var result = await _moviePosterService.CreateMoviePosterAsync(posterDto, CancellationToken.None);

            // Assert
            Assert.IsTrue(result);

            // Verify the existing poster was replaced
            var posters = await _dbContext.MoviePoster.Where(p => p.MovieId == _existingMovieId).ToListAsync();
            Assert.AreEqual(1, posters.Count, "There should only be one poster for the movie");

            var poster = posters.First();
            Assert.AreEqual("data:image/png;base64,", poster.ImageFormat);

            // Check if the new base64 image was correctly saved
            var expectedBytes = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg==");
            CollectionAssert.AreEqual(expectedBytes, poster.PosterImage);
        }

        [TestMethod]
        public async Task CreateMoviePosterAsync_InvalidBase64_ReturnsFalse()
        {
            // Arrange
            var posterDto = new MovieCreatePosterDto
            {
                MovieId = _existingMovieId,
                Image = "data:image/png;base64,NOT_A_VALID_BASE64_STRING"
            };

            // Act
            var result = await _moviePosterService.CreateMoviePosterAsync(posterDto, CancellationToken.None);

            // Assert
            Assert.IsFalse(result);

            // Verify logger was called with error
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [TestMethod]
        public async Task CreateMoviePosterAsync_CancellationRequested_ReturnsFalse()
        {
            // Arrange
            var posterDto = new MovieCreatePosterDto
            {
                MovieId = _existingMovieId,
                Image = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg=="
            };

            // Create a cancelled token
            var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act
            var result = await _moviePosterService.CreateMoviePosterAsync(posterDto, cts.Token);

            // Assert
            Assert.IsFalse(result);

            // Verify logger was called with error
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [TestMethod]
        public async Task CreateMoviePosterAsync_NonExistentMovie_StillSavesPoster()
        {
            // Arrange
            var nonExistentMovieId = 9999;
            var posterDto = new MovieCreatePosterDto
            {
                MovieId = nonExistentMovieId,
                Image = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg=="
            };

            // Act
            var result = await _moviePosterService.CreateMoviePosterAsync(posterDto, CancellationToken.None);

            // Assert
            Assert.IsTrue(result);

            // Verify poster was saved despite non-existent movie (testing FK constraints aren't enforced in test)
            var poster = await _dbContext.MoviePoster.FirstOrDefaultAsync(p => p.MovieId == nonExistentMovieId);
            Assert.IsNotNull(poster);
            Assert.AreEqual(nonExistentMovieId, poster.MovieId);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}