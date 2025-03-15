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
    public class GetMoviesServiceTest
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
                new Genres { Id = 2, Name = "Sci-Fi" },
                new Genres { Id = 3, Name = "Romance" }
            };
            _dbContext.Genres.AddRange(genres);

            // Add actors
            var actors = new List<Actors>
            {
                new Actors { Id = 1, FirstName = "Leonardo", LastName = "DiCaprio" },
                new Actors { Id = 2, FirstName = "Ellen", LastName = "Page" }
            };
            _dbContext.Actors.AddRange(actors);

            // Add multiple movies with different properties for testing filters
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
                },
                new Movies
                {
                    Id = 3,
                    Title = "La La Land",
                    Description = "A jazz pianist falls for an aspiring actress in Los Angeles.",
                    ReleaseDate = new DateTime(2016, 12, 9),
                    Duration = 128,
                    Language = "English",
                    AgeRating = "PG-13",
                    CountryId = 1
                },
                new Movies
                {
                    Id = 4,
                    Title = "Parasite",
                    Description = "Greed and class discrimination threaten the relationship between a wealthy family and destitute clan.",
                    ReleaseDate = new DateTime(2019, 5, 30),
                    Duration = 132,
                    Language = "Korean",
                    AgeRating = "R",
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
                new MoviesGenres { MovieId = 2, GenreId = 2 },
                new MoviesGenres { MovieId = 3, GenreId = 3 }
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
        public async Task GetAllMoviesAsync_WithFilters_ReturnsCorrectMovies()
        {
            // Act & Assert

            // Test 1: Basic paging without filters
            var basicParams = new MoviesParameters
            {
                PageNumber = 1,
                PageSize = 2
            };

            var basicResult = await _moviesService.GetAllMoviesAsync(basicParams);

            Assert.IsNotNull(basicResult);
            Assert.AreEqual(2, basicResult.Items.Count);
            Assert.AreEqual(4, basicResult.TotalCount);
            Assert.AreEqual(2, basicResult.TotalPages);
            Assert.AreEqual(1, basicResult.PageNumber);
            Assert.AreEqual(2, basicResult.PageSize);

            // Test 2: Search by title
            var searchParams = new MoviesParameters
            {
                SearchTerm = "Inter",
                PageNumber = 1,
                PageSize = 10
            };

            var searchResult = await _moviesService.GetAllMoviesAsync(searchParams);

            Assert.IsNotNull(searchResult);
            Assert.AreEqual(1, searchResult.Items.Count);
            Assert.AreEqual("Interstellar", searchResult.Items[0].Title);

            // Test 3: Filter by date range
            var dateParams = new MoviesParameters
            {
                FromDate = new DateTime(2014, 1, 1),
                ToDate = new DateTime(2018, 12, 31),
                PageNumber = 1,
                PageSize = 10
            };

            var dateResult = await _moviesService.GetAllMoviesAsync(dateParams);

            Assert.IsNotNull(dateResult);
            Assert.AreEqual(2, dateResult.Items.Count);
            Assert.IsTrue(dateResult.Items.Any(m => m.Title == "Interstellar"));
            Assert.IsTrue(dateResult.Items.Any(m => m.Title == "La La Land"));

            // Test 4: Filter by language
            var languageParams = new MoviesParameters
            {
                Language = "Korean",
                PageNumber = 1,
                PageSize = 10
            };

            var languageResult = await _moviesService.GetAllMoviesAsync(languageParams);

            Assert.IsNotNull(languageResult);
            Assert.AreEqual(1, languageResult.Items.Count);
            Assert.AreEqual("Parasite", languageResult.Items[0].Title);

            // Test 5: Filter by age rating
            var ratingParams = new MoviesParameters
            {
                AgeRating = "R",
                PageNumber = 1,
                PageSize = 10
            };

            var ratingResult = await _moviesService.GetAllMoviesAsync(ratingParams);

            Assert.IsNotNull(ratingResult);
            Assert.AreEqual(1, ratingResult.Items.Count);
            Assert.AreEqual("Parasite", ratingResult.Items[0].Title);

            // Test 6: Filter by director
            var directorParams = new MoviesParameters
            {
                DirectorId = 1,
                PageNumber = 1,
                PageSize = 10
            };

            var directorResult = await _moviesService.GetAllMoviesAsync(directorParams);

            Assert.IsNotNull(directorResult);
            Assert.AreEqual(2, directorResult.Items.Count);
            Assert.IsTrue(directorResult.Items.All(m => m.DirectorId?.Id == 1));

            // Test 7: Filter by genre IDs
            var genreParams = new MoviesParameters
            {
                GenreIds = new List<int> { 2 },
                PageNumber = 1,
                PageSize = 10
            };

            var genreResult = await _moviesService.GetAllMoviesAsync(genreParams);

            Assert.IsNotNull(genreResult);
            Assert.AreEqual(2, genreResult.Items.Count);
            Assert.IsTrue(genreResult.Items.Any(m => m.Title == "Inception"));
            Assert.IsTrue(genreResult.Items.Any(m => m.Title == "Interstellar"));

            // Test 8: Filter by actor IDs
            var actorParams = new MoviesParameters
            {
                ActorIds = new List<int> { 2 },
                PageNumber = 1,
                PageSize = 10
            };

            var actorResult = await _moviesService.GetAllMoviesAsync(actorParams);

            Assert.IsNotNull(actorResult);
            Assert.AreEqual(1, actorResult.Items.Count);
            Assert.AreEqual("Inception", actorResult.Items[0].Title);

            // Test 9: Combined filters
            var combinedParams = new MoviesParameters
            {
                DirectorId = 1,
                Language = "English",
                PageNumber = 1,
                PageSize = 10
            };

            var combinedResult = await _moviesService.GetAllMoviesAsync(combinedParams);

            Assert.IsNotNull(combinedResult);
            Assert.AreEqual(2, combinedResult.Items.Count);
            Assert.IsTrue(combinedResult.Items.All(m => m.Language == "English" && m.DirectorId?.Id == 1));

            // Test 10: Test cancellation token
            var cts = new CancellationTokenSource();
            cts.Cancel();

            await Assert.ThrowsExceptionAsync<OperationCanceledException>(async () =>
            {
                await _moviesService.GetAllMoviesAsync(basicParams, cts.Token);
            });
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}