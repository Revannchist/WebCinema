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
    public class UpdateMovieServiceTest
    {
        private WebCinemaDBContext _dbContext;
        private Mock<ILogger<MoviesService>> _mockLogger;
        private MoviesService _moviesService;
        private int _existingMovieId;

        [TestInitialize]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<WebCinemaDBContext>()
                .UseInMemoryDatabase(databaseName: $"TestMoviesDb_{Guid.NewGuid()}")
                .Options;

            _dbContext = new WebCinemaDBContext(options);

            await SeedTestData();

            _mockLogger = new Mock<ILogger<MoviesService>>();

            _moviesService = new MoviesService(_dbContext, _mockLogger.Object);
        }

        private async Task SeedTestData()
        {
            // Add a director
            var directors = new List<Directors>
            {
                new Directors { Id = 1, FirstName = "Christopher", LastName = "Nolan" },
                new Directors { Id = 2, FirstName = "Steven", LastName = "Spielberg" }
            };
            _dbContext.Directors.AddRange(directors);

            // Add countries
            var countries = new List<Countries>
            {
                new Countries { Id = 1, Name = "USA" },
                new Countries { Id = 2, Name = "UK" }
            };
            _dbContext.Countries.AddRange(countries);

            // Add genres
            var genres = new List<Genres>
            {
                new Genres { Id = 1, Name = "Action" },
                new Genres { Id = 2, Name = "Sci-Fi" },
                new Genres { Id = 3, Name = "Drama" }
            };
            _dbContext.Genres.AddRange(genres);

            // Add actors
            var actors = new List<Actors>
            {
                new Actors { Id = 1, FirstName = "Leonardo", LastName = "DiCaprio" },
                new Actors { Id = 2, FirstName = "Ellen", LastName = "Page" },
                new Actors { Id = 3, FirstName = "Tom", LastName = "Hardy" }
            };
            _dbContext.Actors.AddRange(actors);

            await _dbContext.SaveChangesAsync();

            // Add an existing movie to update
            var movie = new Movies
            {
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
            await _dbContext.SaveChangesAsync();

            // Store the movie ID for later use
            _existingMovieId = movie.Id;

            // Add movie-genre relationships
            var movieGenres = new List<MoviesGenres>
            {
                new MoviesGenres { MovieId = _existingMovieId, GenreId = 1 },
                new MoviesGenres { MovieId = _existingMovieId, GenreId = 2 }
            };
            _dbContext.MoviesGenres.AddRange(movieGenres);

            // Add movie-actor relationships
            var movieActors = new List<MoviesActors>
            {
                new MoviesActors { MovieId = _existingMovieId, ActorId = 1 },
                new MoviesActors { MovieId = _existingMovieId, ActorId = 2 }
            };
            _dbContext.MoviesActors.AddRange(movieActors);

            await _dbContext.SaveChangesAsync();
        }

        [TestMethod]
        public async Task UpdateMovieAsync_ValidUpdate_ReturnsCorrectResponseDto()
        {
            // Arrange
            var updateDto = new MoviesUpdateDto
            {
                Title = "Inception: Director's Cut",
                Description = "Updated description about dream-sharing technology.",
                ReleaseDate = new DateTime(2010, 8, 1), // Changed release date
                Duration = 160, // Longer duration
                Language = "English",
                AgeRating = "R", // Changed rating
                DirectorId = 1, // Same director
                CountryId = 2, // Changed country
                GenreIds = new List<int> { 2, 3 }, // Changed genres
                ActorIds = new List<int> { 1, 3 } // Changed actors
            };

            // Act
            var result = await _moviesService.UpdateMovieAsync(_existingMovieId, updateDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_existingMovieId, result.Id);
            Assert.AreEqual("Inception: Director's Cut", result.Title);
            Assert.AreEqual("Updated description about dream-sharing technology.", result.Description);
            Assert.AreEqual(new DateTime(2010, 8, 1), result.ReleaseDate);
            Assert.AreEqual(160, result.Duration);
            Assert.AreEqual("English", result.Language);
            Assert.AreEqual("R", result.AgeRating);

            // Check director (unchanged)
            Assert.IsNotNull(result.Director);
            Assert.AreEqual(1, result.Director.Id);
            Assert.AreEqual("Christopher", result.Director.FirstName);
            Assert.AreEqual("Nolan", result.Director.LastName);

            // Check country (changed)
            Assert.IsNotNull(result.Country);
            Assert.AreEqual(2, result.Country.Id);
            Assert.AreEqual("UK", result.Country.Name);

            // Check genres (changed)
            Assert.IsNotNull(result.Genres);
            Assert.AreEqual(2, result.Genres.Count);
            CollectionAssert.Contains(result.Genres.Select(g => g.Id).ToList(), 2);
            CollectionAssert.Contains(result.Genres.Select(g => g.Id).ToList(), 3);
            CollectionAssert.DoesNotContain(result.Genres.Select(g => g.Id).ToList(), 1);

            // Check actors (changed)
            Assert.IsNotNull(result.Actors);
            Assert.AreEqual(2, result.Actors.Count);
            CollectionAssert.Contains(result.Actors.Select(a => a.Id).ToList(), 1);
            CollectionAssert.Contains(result.Actors.Select(a => a.Id).ToList(), 3);
            CollectionAssert.DoesNotContain(result.Actors.Select(a => a.Id).ToList(), 2);

            // Verify database was actually updated
            var updatedMovie = await _dbContext.Movies.FindAsync(_existingMovieId);
            Assert.IsNotNull(updatedMovie);
            Assert.AreEqual("Inception: Director's Cut", updatedMovie.Title);
            Assert.AreEqual(160, updatedMovie.Duration);
            Assert.AreEqual(2, updatedMovie.CountryId);

            // Verify relationships were updated correctly
            var movieGenres = await _dbContext.MoviesGenres
                .Where(mg => mg.MovieId == _existingMovieId)
                .ToListAsync();
            Assert.AreEqual(2, movieGenres.Count);
            CollectionAssert.Contains(movieGenres.Select(mg => mg.GenreId).ToList(), 2);
            CollectionAssert.Contains(movieGenres.Select(mg => mg.GenreId).ToList(), 3);

            var movieActors = await _dbContext.MoviesActors
                .Where(ma => ma.MovieId == _existingMovieId)
                .ToListAsync();
            Assert.AreEqual(2, movieActors.Count);
            CollectionAssert.Contains(movieActors.Select(ma => ma.ActorId).ToList(), 1);
            CollectionAssert.Contains(movieActors.Select(ma => ma.ActorId).ToList(), 3);
        }

        [TestMethod]
        public async Task UpdateMovieAsync_MovieNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            var nonExistentMovieId = 9999;
            var updateDto = new MoviesUpdateDto
            {
                Title = "Nonexistent Movie",
                Description = "This movie doesn't exist",
                ReleaseDate = DateTime.Now,
                Duration = 120,
                Language = "English",
                AgeRating = "PG",
                DirectorId = 1,
                CountryId = 1,
                GenreIds = new List<int> { 1 },
                ActorIds = new List<int> { 1 }
            };

            // Act & Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
                await _moviesService.UpdateMovieAsync(nonExistentMovieId, updateDto));

            // Verify logger was called with appropriate warning
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Movie not found for update: {nonExistentMovieId}")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [TestMethod]
        public async Task UpdateMovieAsync_DuplicateMovie_ThrowsInvalidOperationException()
        {
            // Arrange - First add another movie
            var anotherMovie = new Movies
            {
                Title = "Interstellar",
                Description = "A team of explorers travel through a wormhole in space.",
                ReleaseDate = new DateTime(2014, 11, 7),
                Duration = 169,
                Language = "English",
                AgeRating = "PG-13",
                DirectorId = 1,
                CountryId = 1
            };
            _dbContext.Movies.Add(anotherMovie);
            await _dbContext.SaveChangesAsync();

            // Prepare update DTO that would create a duplicate
            var updateDto = new MoviesUpdateDto
            {
                Title = "Interstellar", // Same title as another movie
                Description = "Updated description",
                ReleaseDate = new DateTime(2014, 11, 7), // Same release date as another movie
                Duration = 160,
                Language = "English",
                AgeRating = "PG-13",
                DirectorId = 1,
                CountryId = 1,
                GenreIds = new List<int> { 1, 2 },
                ActorIds = new List<int> { 1, 2 }
            };

            // Act & Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
                await _moviesService.UpdateMovieAsync(_existingMovieId, updateDto));

            // Verify logger was called with appropriate warning
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Update failed: Movie with same title and release date already exists")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [TestMethod]
        public async Task UpdateMovieAsync_CancellationRequested_ThrowsOperationCanceledException()
        {
            // Arrange
            var updateDto = new MoviesUpdateDto
            {
                Title = "Cancelled Update",
                Description = "This update should be cancelled",
                ReleaseDate = DateTime.Now,
                Duration = 120,
                Language = "English",
                AgeRating = "PG-13",
                DirectorId = 1,
                CountryId = 1,
                GenreIds = new List<int> { 1 },
                ActorIds = new List<int> { 1 }
            };

            // Create a cancelled token
            var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act & Assert
            await Assert.ThrowsExceptionAsync<OperationCanceledException>(async () =>
                await _moviesService.UpdateMovieAsync(_existingMovieId, updateDto, cts.Token));
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}