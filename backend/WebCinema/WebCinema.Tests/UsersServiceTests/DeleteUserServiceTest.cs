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
    public class DeleteUserServiceTest
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

            // Seed test user
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
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            _existingUserId = user.Id;
            _usersService = new UsersService(_dbContext);
        }

        [TestMethod]
        public async Task DeleteUsersByIdAsync_ExistingUser_DeletesAndReturnsUser()
        {
            // Act
            var deletedUser = await _usersService.DeleteUsersByIdAsync(_existingUserId);

            // Assert
            Assert.IsNotNull(deletedUser);
            Assert.AreEqual("testuser", deletedUser.Username);

            // Verify user was actually deleted from database
            var userInDb = await _dbContext.Users.FindAsync(_existingUserId);
            Assert.IsNull(userInDb);
        }

        [TestMethod]
        public async Task DeleteUsersByIdAsync_NonExistingUser_ReturnsNull()
        {
            // Arrange
            int nonExistingId = 9999;

            // Act
            var deletedUser = await _usersService.DeleteUsersByIdAsync(nonExistingId);

            // Assert
            Assert.IsNull(deletedUser);
        }

        [TestMethod]
        public async Task DeleteUsersByIdAsync_DeletedUserHasCorrectDisplayDto()
        {
            // Act
            var deletedUser = await _usersService.DeleteUsersByIdAsync(_existingUserId);

            // Assert
            Assert.IsNotNull(deletedUser);
            Assert.IsInstanceOfType(deletedUser, typeof(UserDisplayDto));
            Assert.AreEqual("testuser", deletedUser.Username);
            Assert.AreEqual("test@email.com", deletedUser.Email);
            Assert.AreEqual("User", deletedUser.RoleName);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}