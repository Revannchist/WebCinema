using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebCinema.Models;
using WebCinema.Services;

namespace WebCinema.Tests.UsersServiceTests
{
    [TestClass]
    public class GetUsersPagedServiceTest
    {
        private WebCinemaDBContext _dbContext;
        private UsersService _usersService;

        [TestInitialize]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<WebCinemaDBContext>()
                .UseInMemoryDatabase(databaseName: $"TestUsersDb_{Guid.NewGuid()}")
                .Options;

            _dbContext = new WebCinemaDBContext(options);

            // Seed roles
            _dbContext.Roles.AddRange(
                new Roles { Id = 1, Name = "Admin" },
                new Roles { Id = 2, Name = "User" }
            );
            await _dbContext.SaveChangesAsync();

            // Seed multiple test users
            for (int i = 1; i <= 15; i++)
            {
                _dbContext.Users.Add(new Users
                {
                    Username = $"user{i}",
                    Email = $"user{i}@email.com",
                    Password = "pass123",
                    FirstName = $"User{i}",
                    LastName = $"Test",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    RegistrationTime = DateTime.Now.AddDays(-i),
                    RoleId = 2
                });
            }

            // Add one admin user
            _dbContext.Users.Add(new Users
            {
                Username = "admin",
                Email = "admin@email.com",
                Password = "admin123",
                FirstName = "Admin",
                LastName = "User",
                DateOfBirth = new DateTime(1985, 1, 1),
                RegistrationTime = DateTime.Now,
                RoleId = 1
            });

            await _dbContext.SaveChangesAsync();
            _usersService = new UsersService(_dbContext);
        }

        [TestMethod]
        public async Task GetUsersPagedAndFilteredAsync_FirstPage_ReturnsCorrectUsers()
        {
            // Act
            var (users, totalUsers) = await _usersService.GetUsersPagedAndFilteredAsync(1, 5, "");

            // Assert
            Assert.IsNotNull(users);
            Assert.AreEqual(5, users.Count);
            Assert.AreEqual(15, totalUsers); // Total regular users (excluding admin)
        }

        [TestMethod]
        public async Task GetUsersPagedAndFilteredAsync_SearchByUsername_ReturnsFilteredResults()
        {
            // Act
            var (users, totalUsers) = await _usersService.GetUsersPagedAndFilteredAsync(1, 10, "user1");

            // Assert
            Assert.IsNotNull(users);
            Assert.IsTrue(users.All(u => u.Username.Contains("user1")));
        }

        [TestMethod]
        public async Task GetUsersPagedAndFilteredAsync_SearchByEmail_ReturnsFilteredResults()
        {
            // Act
            var (users, totalUsers) = await _usersService.GetUsersPagedAndFilteredAsync(1, 10, "user1@email");

            // Assert
            Assert.IsNotNull(users);
            Assert.IsTrue(users.All(u => u.Email.Contains("user1@email")));
        }

        [TestMethod]
        public async Task GetUsersPagedAndFilteredAsync_LastPage_ReturnsRemainingUsers()
        {
            // Act
            var (users, totalUsers) = await _usersService.GetUsersPagedAndFilteredAsync(4, 5, "");

            // Assert
            Assert.IsNotNull(users);
            Assert.AreEqual(15 % 5, users.Count); // Should return remaining users
        }

        [TestMethod]
        public async Task GetUsersPagedAndFilteredAsync_ExcludesAdminUsers()
        {
            // Act
            var (users, totalUsers) = await _usersService.GetUsersPagedAndFilteredAsync(1, 20, "");

            // Assert
            Assert.IsNotNull(users);
            Assert.IsFalse(users.Any(u => u.RoleId == 1));
            Assert.IsFalse(users.Any(u => u.Username == "admin"));
        }

        [TestMethod]
        public async Task GetUsersPagedAndFilteredAsync_NoMatchingResults_ReturnsEmptyList()
        {
            // Act
            var (users, totalUsers) = await _usersService.GetUsersPagedAndFilteredAsync(1, 10, "nonexistent");

            // Assert
            Assert.IsNotNull(users);
            Assert.AreEqual(0, users.Count);
            Assert.AreEqual(0, totalUsers);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}