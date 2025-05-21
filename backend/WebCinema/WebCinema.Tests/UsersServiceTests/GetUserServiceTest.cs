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
    public class GetUserServiceTest
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

            // Seed test users
            var users = new[]
            {
                new Users
                {
                    Username = "user1",
                    Email = "user1@email.com",
                    Password = "pass123",
                    FirstName = "User",
                    LastName = "One",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    RegistrationTime = DateTime.Now.AddDays(-10),
                    RoleId = 2
                },
                new Users
                {
                    Username = "user2",
                    Email = "user2@email.com",
                    Password = "pass123",
                    FirstName = "User",
                    LastName = "Two",
                    DateOfBirth = new DateTime(1995, 1, 1),
                    RegistrationTime = DateTime.Now.AddDays(-5),
                    RoleId = 2
                },
                new Users
                {
                    Username = "admin",
                    Email = "admin@email.com",
                    Password = "admin123",
                    FirstName = "Admin",
                    LastName = "User",
                    DateOfBirth = new DateTime(1985, 1, 1),
                    RegistrationTime = DateTime.Now.AddDays(-20),
                    RoleId = 1
                }
            };

            _dbContext.Users.AddRange(users);
            await _dbContext.SaveChangesAsync();

            _usersService = new UsersService(_dbContext);
        }

        [TestMethod]
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            // Act
            var users = await _usersService.GetAllUsersAsync();

            // Assert
            Assert.IsNotNull(users);
            Assert.AreEqual(3, users.Count);
            Assert.IsTrue(users.Any(u => u.Username == "user1"));
            Assert.IsTrue(users.Any(u => u.Username == "user2"));
            Assert.IsTrue(users.Any(u => u.Username == "admin"));
        }

        [TestMethod]
        public async Task GetUsersByIdForDisplayAsync_ExistingUser_ReturnsCorrectUser()
        {
            // Arrange
            var existingUser = await _dbContext.Users.FirstAsync();

            // Act
            var user = await _usersService.GetUsersByIdForDisplayAsync(existingUser.Id);

            // Assert
            Assert.IsNotNull(user);
            Assert.AreEqual(existingUser.Username, user.Username);
            Assert.AreEqual(existingUser.Email, user.Email);
            Assert.IsNotNull(user.RoleName);
        }

        [TestMethod]
        public async Task GetUsersByIdForDisplayAsync_NonExistingUser_ReturnsNull()
        {
            // Act
            var user = await _usersService.GetUsersByIdForDisplayAsync(9999);

            // Assert
            Assert.IsNull(user);
        }

        [TestMethod]
        public async Task GetUsersPagedAndFilteredAsync_ReturnsCorrectPageSize()
        {
            // Act
            var (users, totalUsers) = await _usersService.GetUsersPagedAndFilteredAsync(1, 2, "");

            // Assert
            Assert.IsNotNull(users);
            Assert.AreEqual(2, users.Count);
            Assert.AreEqual(2, totalUsers); // Only regular users (RoleId = 2)
        }

        [TestMethod]
        public async Task GetUsersPagedAndFilteredAsync_SearchTermFiltersCorrectly()
        {
            // Act
            var (users, totalUsers) = await _usersService.GetUsersPagedAndFilteredAsync(1, 10, "user1");

            // Assert
            Assert.IsNotNull(users);
            Assert.AreEqual(1, users.Count);
            Assert.AreEqual("user1", users[0].Username);
        }

        [TestMethod]
        public async Task GetUsersPagedAndFilteredAsync_EmptySearchReturnsAllRegularUsers()
        {
            // Act
            var (users, totalUsers) = await _usersService.GetUsersPagedAndFilteredAsync(1, 10, "");

            // Assert
            Assert.IsNotNull(users);
            Assert.AreEqual(2, users.Count); // Only regular users (RoleId = 2)
            Assert.IsFalse(users.Any(u => u.RoleId == 1)); // No admin users
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}