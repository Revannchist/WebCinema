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
    public class GetAllMoviePostersServiceTest
    {
        private WebCinemaDBContext _dbContext;
        private Mock<ILogger<MoviePosterService>> _mockLogger;
        private MoviePosterService _moviesPosterService;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<WebCinemaDBContext>()
                .UseInMemoryDatabase(databaseName: $"TestGetAllPostersDb_{Guid.NewGuid()}")
                .Options;

            _dbContext = new WebCinemaDBContext(options);
            _mockLogger = new Mock<ILogger<MoviePosterService>>();
            _moviesPosterService = new MoviePosterService(_dbContext, _mockLogger.Object);
        }

        [TestMethod]
        public async Task GetAllMoviePostersAsync_EmptyDatabase_ReturnsEmptyList()
        {
            // Arrange - database is empty by default

            // Act
            var result = await _moviesPosterService.GetAllMoviePostersAsync(CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count, "Should return an empty list when no posters exist");
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}