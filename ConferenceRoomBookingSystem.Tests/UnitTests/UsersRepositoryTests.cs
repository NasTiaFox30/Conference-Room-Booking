using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConferenceRoomBookingSystem.Data;
using ConferenceRoomBookingSystem.Models;
using System.Linq;

namespace ConferenceRoomBookingSystem.Tests
{
    [TestClass]
    public class UsersRepositoryTests
    {
        private UsersRepository _userRepo;
        private User _testUser;

        [TestInitialize]
        public void Setup()
        {
            _userRepo = new UsersRepository();

            // Create a test user for tests
            _testUser = new User
            {
                Username = "testuser_" + System.Guid.NewGuid().ToString().Substring(0, 8),
                Email = "test_" + System.Guid.NewGuid().ToString().Substring(0, 8) + "@company.com",
                FirstName = "Test",
                LastName = "User",
                Department = "IT",
                IsAdmin = false,
                IsActive = true
            };

            string testPassword = "testpassword123";
            _userRepo.CreateUser(_testUser, testPassword);
        }

        [TestMethod]
        public void GetUserByUsername_WithValidUsername_ReturnsUser()
        {
            // Arrange & Act
            var user = _userRepo.GetUserByUsername(_testUser.Username);

            // Assert
            Assert.IsNotNull(user);
            Assert.AreEqual(_testUser.Username, user.Username);
        }

        [TestMethod]
        public void GetUserByUsername_WithInvalidUsername_ReturnsNull()
        {
            // Arrange
            string invalidUsername = "nonexistentuser_" + System.Guid.NewGuid().ToString();

            // Act
            var user = _userRepo.GetUserByUsername(invalidUsername);

            // Assert
            Assert.IsNull(user);
        }

        [TestMethod]
        public void ValidateUser_WithValidCredentials_ReturnsTrue()
        {
            // Arrange
            string username = _testUser.Username;
            string password = "testpassword123";

            // Act
            bool result = _userRepo.ValidateUser(username, password);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateUser_WithInvalidPassword_ReturnsFalse()
        {
            // Arrange
            string username = _testUser.Username;
            string wrongPassword = "wrongpassword";

            // Act
            bool result = _userRepo.ValidateUser(username, wrongPassword);

            // Assert
            Assert.IsFalse(result);
        }
    }
}