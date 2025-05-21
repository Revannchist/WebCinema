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
    public class AddUserServiceTest
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

            // Seed initial test user
            var initialUser = new Users
            {
                Username = "existinguser",
                Email = "existing@email.com",
                Password = "pass123",
                FirstName = "Existing",
                LastName = "User",
                DateOfBirth = new DateTime(1990, 1, 1),
                RegistrationTime = DateTime.Now,
                RoleId = 2
            };
            _dbContext.Users.Add(initialUser);
            await _dbContext.SaveChangesAsync();

            _usersService = new UsersService(_dbContext);
        }

        [TestMethod]
        public async Task CreateUsersAsync_ValidUser_ReturnsSuccessfully()
        {
            // Arrange
            var newUser = new Users
            {
                Username = "newuser",
                Email = "newuser@email.com",
                Password = "pass123",
                FirstName = "New",
                LastName = "User",
                DateOfBirth = new DateTime(1995, 1, 1),
                RegistrationTime = DateTime.Now,
                RoleId = 2
            };

            // Act
            var (createdUser, errorMessage) = await _usersService.CreateUsersAsync(newUser);

            // Assert
            Assert.IsNotNull(createdUser);
            Assert.IsNull(errorMessage);
            Assert.AreEqual(newUser.Username, createdUser.Username);
            Assert.AreEqual(newUser.Email.ToLower(), createdUser.Email);
            Assert.AreEqual(2, createdUser.RoleId); // Default User role
        }

        [TestMethod]
        public async Task CreateUsersAsync_DuplicateUsername_ReturnsError()
        {
            // Arrange
            var newUser = new Users
            {
                Username = "existinguser", // Already exists
                Email = "unique@email.com",
                Password = "pass123",
                FirstName = "Test",
                LastName = "User",
                DateOfBirth = new DateTime(1995, 1, 1),
                RegistrationTime = DateTime.Now,
                RoleId = 2
            };

            // Act
            var (createdUser, errorMessage) = await _usersService.CreateUsersAsync(newUser);

            // Assert
            Assert.IsNull(createdUser);
            Assert.AreEqual("Username already exists", errorMessage);
        }

        [TestMethod]
        public async Task CreateUsersAsync_DuplicateEmail_ReturnsError()
        {
            // Arrange
            var newUser = new Users
            {
                Username = "uniqueuser",
                Email = "existing@email.com", // Already exists
                Password = "pass123",
                FirstName = "Test",
                LastName = "User",
                DateOfBirth = new DateTime(1995, 1, 1),
                RegistrationTime = DateTime.Now,
                RoleId = 2
            };

            // Act
            var (createdUser, errorMessage) = await _usersService.CreateUsersAsync(newUser);

            // Assert
            Assert.IsNull(createdUser);
            Assert.AreEqual("Email already exists", errorMessage);
        }

        [TestMethod]
        public async Task CreateUsersAsync_InvalidPassword_ReturnsError()
        {
            // Arrange
            var newUser = new Users
            {
                Username = "validuser",
                Email = "valid@email.com",
                Password = "weak", // Too short, no numbers
                FirstName = "Test",
                LastName = "User",
                DateOfBirth = new DateTime(1995, 1, 1),
                RegistrationTime = DateTime.Now,
                RoleId = 2
            };

            // Act
            var (createdUser, errorMessage) = await _usersService.CreateUsersAsync(newUser);

            // Assert
            Assert.IsNull(createdUser);
            Assert.AreEqual("Password must be at least 5 characters long and contain at least 1 number", errorMessage);
        }

        [TestMethod]
        public async Task CreateUsersAsync_InvalidRole_ReturnsError()
        {
            // Arrange
            var newUser = new Users
            {
                Username = "validuser",
                Email = "valid@email.com",
                Password = "pass123",
                FirstName = "Test",
                LastName = "User",
                DateOfBirth = new DateTime(1995, 1, 1),
                RegistrationTime = DateTime.Now,
                RoleId = 999 // Non-existent role
            };

            // Act
            var (createdUser, errorMessage) = await _usersService.CreateUsersAsync(newUser);

            // Assert
            Assert.IsNull(createdUser);
            Assert.AreEqual("Invalid role specified", errorMessage);
        }

        [TestMethod]
        public async Task CreateUsersAsync_NullUser_ReturnsError()
        {
            // Act
            var (createdUser, errorMessage) = await _usersService.CreateUsersAsync(null);

            // Assert
            Assert.IsNull(createdUser);
            Assert.AreEqual("User data is required", errorMessage);
        }

        [TestMethod]
        public async Task CreateUsersAsync_DefaultRole_SetsUserRole()
        {
            // Arrange
            var newUser = new Users
            {
                Username = "defaultroleuser",
                Email = "default@email.com",
                Password = "pass123",
                FirstName = "Default",
                LastName = "User",
                DateOfBirth = new DateTime(1995, 1, 1),
                RegistrationTime = DateTime.Now,
                RoleId = 0 // Should be set to default (2 - User)
            };

            // Act
            var (createdUser, errorMessage) = await _usersService.CreateUsersAsync(newUser);

            // Assert
            Assert.IsNotNull(createdUser);
            Assert.IsNull(errorMessage);
            Assert.AreEqual(2, createdUser.RoleId); // Verify default role was set
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}