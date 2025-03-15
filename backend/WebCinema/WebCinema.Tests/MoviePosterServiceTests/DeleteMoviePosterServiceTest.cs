using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebCinema.Models;
using WebCinema.Services;

namespace WebCinema.Tests.MoviePosterServiceTests
{
    [TestClass]
    public class DeleteMoviePosterServiceTest
    {
        private WebCinemaDBContext _dbContext;
        private Mock<ILogger<MoviePosterService>> _mockLogger;
        private MoviePosterService _moviesPosterService;
        private int _existingPosterId;
        private int _existingMovieId;

        [TestInitialize]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<WebCinemaDBContext>()
                .UseInMemoryDatabase(databaseName: $"TestDeletePosterDb_{Guid.NewGuid()}")
                .Options;

            _dbContext = new WebCinemaDBContext(options);

            await SeedTestData();

            _mockLogger = new Mock<ILogger<MoviePosterService>>();

            _moviesPosterService = new MoviePosterService(_dbContext, _mockLogger.Object);
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

            // Add a poster
            var poster = new MoviePoster
            {
                MovieId = _existingMovieId,
                PosterImage = Convert.FromBase64String("dGVzdGltYWdl"), // "testimage" in base64
                ImageFormat = "data:image/jpeg;base64,"
            };
            _dbContext.MoviePoster.Add(poster);
            await _dbContext.SaveChangesAsync();

            _existingPosterId = poster.Id;
        }

        [TestMethod]
        public async Task DeleteMoviePosterByIdAsync_ExistingPoster_ReturnsTrue()
        {
            // Arrange
            var posterId = _existingPosterId;

            // Act
            var result = await _moviesPosterService.DeleteMoviePosterByIdAsync(posterId, CancellationToken.None);

            // Assert
            Assert.IsTrue(result);

            // Verify the poster was actually deleted from the database
            var poster = await _dbContext.MoviePoster.FindAsync(posterId);
            Assert.IsNull(poster, "Poster should have been deleted from the database");
        }

        [TestMethod]
        public async Task DeleteMoviePosterByIdAsync_NonExistentPoster_ReturnsFalse()
        {
            // Arrange
            var nonExistentPosterId = 9999;

            // Act
            var result = await _moviesPosterService.DeleteMoviePosterByIdAsync(nonExistentPosterId, CancellationToken.None);

            // Assert
            Assert.IsFalse(result);

            // Verify that the warning was logged
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Image with ID {nonExistentPosterId} not found")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [TestMethod]
        public async Task DeleteMoviePosterByIdAsync_CancellationRequested_ReturnsFalse()
        {
            // Arrange
            var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act
            var result = await _moviesPosterService.DeleteMoviePosterByIdAsync(_existingPosterId, cts.Token);

            // Assert
            Assert.IsFalse(result);

            // Verify error was logged
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);

            // Verify poster still exists in database since deletion was cancelled
            var poster = await _dbContext.MoviePoster.FindAsync(_existingPosterId);
            Assert.IsNotNull(poster, "Poster should still exist since deletion was cancelled");
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}