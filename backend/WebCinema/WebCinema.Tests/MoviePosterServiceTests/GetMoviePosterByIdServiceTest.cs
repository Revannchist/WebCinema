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

namespace WebCinema.Tests.MoviePosterServiceTests
{
    [TestClass]
    public class GetPosterByMovieIdServiceTest
    {
        private WebCinemaDBContext _dbContext;
        private Mock<ILogger<MoviePosterService>> _mockLogger;
        private MoviePosterService _moviesPosterService;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<WebCinemaDBContext>()
                .UseInMemoryDatabase(databaseName: $"TestGetPosterByMovieIdDb_{Guid.NewGuid()}")
                .Options;
            _dbContext = new WebCinemaDBContext(options);
            _mockLogger = new Mock<ILogger<MoviePosterService>>();
            _moviesPosterService = new MoviePosterService(_dbContext, _mockLogger.Object);
        }

        [TestMethod]
        public async Task GetPosterByMovieIdAsync_NonExistentMovieId_ReturnsNull()
        {
            // Arrange - database is empty by default
            const int nonExistentMovieId = 9999;

            // Act
            var result = await _moviesPosterService.GetPosterByMovieIdAsync(nonExistentMovieId, CancellationToken.None);

            // Assert
            Assert.IsNull(result, "Should return null when movie poster does not exist");
        }

        [TestMethod]
        public async Task GetPosterByMovieIdAsync_ExistingMovieId_ReturnsPoster()
        {
            // Arrange
            const int movieId = 1;
            byte[] testImageData = new byte[] { 1, 2, 3, 4, 5 };
            const string imageFormat = "data:image/jpeg;base64,";

            _dbContext.MoviePoster.Add(new MoviePoster
            {
                Id = 1,
                MovieId = movieId,
                PosterImage = testImageData,
                ImageFormat = imageFormat
            });
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _moviesPosterService.GetPosterByMovieIdAsync(movieId, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result, "Should return a poster when movie ID exists");
            Assert.AreEqual(1, result.Id, "Should return correct poster ID");
            Assert.AreEqual(imageFormat + Convert.ToBase64String(testImageData), result.Image, "Should return correct image data");
            Assert.AreEqual(imageFormat, result.ImageFormat, "Should return correct image format");
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}