using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using WebCinema.Models;
using WebCinema.Models.DTO;
using WebCinema.Services;

namespace WebCinema.Tests.UsersServiceTests
{
    [TestClass]
    public class UpdateUserServiceTest
    {
        private WebCinemaDBContext _dbContext;
        private UsersService _usersService;
        private int _existingUserId;

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
            var user = new Users
            {
                Username = "testuser",
                Email = "test@email.com",
                Password = "pass123",
                FirstName = "Test",
                LastName = "User",
                DateOfBirth = new DateTime(1990, 1, 1),
                RegistrationTime = DateTime.Now,
                RoleId = 2
            };

            var secondUser = new Users
            {
                Username = "seconduser",
                Email = "second@email.com",
                Password = "pass123",
                FirstName = "Second",
                LastName = "User",
                DateOfBirth = new DateTime(1995, 1, 1),
                RegistrationTime = DateTime.Now,
                RoleId = 2
            };

            _dbContext.Users.AddRange(user, secondUser);
            await _dbContext.SaveChangesAsync();

            _existingUserId = user.Id;
            _usersService = new UsersService(_dbContext);
        }

        [TestMethod]
        public async Task UpdateUsersAsync_ValidUpdate_UpdatesSuccessfully()
        {
            // Arrange
            var updatedUser = new Users
            {
                Username = "updateduser",
                Email = "updated@email.com",
                Password = "newpass123",
                FirstName = "Updated",
                LastName = "User",
                DateOfBirth = new DateTime(1990, 1, 1),
                RegistrationTime = DateTime.Now,
                RoleId = 2
            };

            // Act
            var (result, errorMessage) = await _usersService.UpdateUsersAsync(_existingUserId, updatedUser);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(errorMessage);
            Assert.AreEqual("updateduser", result.Username);
            Assert.AreEqual("updated@email.com", result.Email);
        }

        [TestMethod]
        public async Task UpdateUsersAsync_NonExistingUser_ReturnsError()
        {
            // Arrange
            var updatedUser = new Users
            {
                Username = "updateduser",
                Email = "updated@email.com",
                Password = "newpass123",
                RoleId = 2
            };

            // Act
            var (result, errorMessage) = await _usersService.UpdateUsersAsync(9999, updatedUser);

            // Assert
            Assert.IsNull(result);
            Assert.AreEqual("User not found", errorMessage);
        }

        [TestMethod]
        public async Task UpdateUsersAsync_DuplicateUsername_ReturnsError()
        {
            // Arrange
            var updatedUser = new Users
            {
                Username = "seconduser", // Already exists
                Email = "unique@email.com",
                Password = "pass123",
                RoleId = 2
            };

            // Act
            var (result, errorMessage) = await _usersService.UpdateUsersAsync(_existingUserId, updatedUser);

            // Assert
            Assert.IsNull(result);
            Assert.AreEqual("Username already exists", errorMessage);
        }

        [TestMethod]
        public async Task UpdateUserBasicInfoAsync_ValidUpdate_UpdatesSuccessfully()
        {
            // Arrange
            var dto = new UsersEditDto
            {
                Username = "basicupdate",
                Email = "basic@email.com",
                Password = "pass123",
                RoleId = 2
            };

            // Act
            var (result, errorMessage) = await _usersService.UpdateUserBasicInfoAsync(_existingUserId, dto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(errorMessage);
            Assert.AreEqual("basicupdate", result.Username);
            Assert.AreEqual("basic@email.com", result.Email);
        }

        [TestMethod]
        public async Task UpdateUserBasicInfoAsync_InvalidPassword_ReturnsError()
        {
            // Arrange
            var dto = new UsersEditDto
            {
                Username = "basicupdate",
                Email = "basic@email.com",
                Password = "weak", // Invalid password
                RoleId = 2
            };

            // Act
            var (result, errorMessage) = await _usersService.UpdateUserBasicInfoAsync(_existingUserId, dto);

            // Assert
            Assert.IsNull(result);
            Assert.AreEqual("Password must be at least 5 characters long and contain at least 1 number", errorMessage);
        }

        [TestMethod]
        public async Task UpdateUserBasicInfoAsync_NonExistingUser_ReturnsError()
        {
            // Arrange
            var dto = new UsersEditDto
            {
                Username = "basicupdate",
                Email = "basic@email.com",
                Password = "pass123",
                RoleId = 2
            };

            // Act
            var (result, errorMessage) = await _usersService.UpdateUserBasicInfoAsync(9999, dto);

            // Assert
            Assert.IsNull(result);
            Assert.AreEqual("User not found", errorMessage);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}