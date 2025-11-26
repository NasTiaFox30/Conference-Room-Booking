using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConferenceRoomBookingSystem.Data;
using ConferenceRoomBookingSystem.Models;
using System.Collections.Generic;

namespace ConferenceRoomBookingSystem.Tests
{
    [TestClass]
    public class ConferenceRoomRepositoryTests
    {
        private ConferenceRoomRepository _roomRepo;

        [TestInitialize]
        public void Setup()
        {
            _roomRepo = new ConferenceRoomRepository();
        }

        [TestMethod]
        public void GetAllRooms_ReturnsActiveRooms()
        {
            // Act
            var rooms = _roomRepo.GetAllRooms();

            // Assert
            Assert.IsNotNull(rooms);
            Assert.IsInstanceOfType(rooms, typeof(List<ConferenceRoom>));

            // Сheck that all rooms are active
            foreach (var room in rooms)
                Assert.IsTrue(room.IsActive);
        }

        [TestMethod]
        public void GetRoomById_WithValidId_ReturnsRoom()
        {
            // Arrange
            int roomId = 1;

            // Act
            var room = _roomRepo.GetRoomById(roomId);

            // Assert
            Assert.IsNotNull(room);
            Assert.AreEqual(roomId, room.RoomId);
        }

        [TestMethod]
        public void GetRoomById_WithInvalidId_ReturnsNull()
        {
            // Arrange
            int invalidRoomId = 999;

            // Act
            var room = _roomRepo.GetRoomById(invalidRoomId);

            // Assert
            Assert.IsNull(room);
        }

        [TestMethod]
        public void CreateRoom_WithValidData_ReturnsTrue()
        {
            // Arrange
            var room = new ConferenceRoom
            {
                RoomName = "Test Room",
                Capacity = 15,
                Location = "Test Location",
                Description = "Test Description",
                HasProjector = true,
                HasWhiteboard = true,
                HasAudioSystem = false,
                HasWiFi = true,
                IsActive = true
            };

            // Act
            bool result = _roomRepo.CreateRoom(room);

            // Assert
            Assert.IsTrue(result);
        }
    }
}