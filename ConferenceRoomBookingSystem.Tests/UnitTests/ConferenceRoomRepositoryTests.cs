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

        [TestMethod]
        public void UpdateRoom_WithValidData_ReturnsTrue()
        {
            // Arrange
            var room = _roomRepo.GetRoomById(1);
            string originalName = room.RoomName;
            room.RoomName = "Updated Room Name";

            // Act
            bool result = _roomRepo.UpdateRoom(room);

            // Assert
            Assert.IsTrue(result);

            // Check that the data has been updated
            var updatedRoom = _roomRepo.GetRoomById(1);
            Assert.AreEqual("Updated Room Name", updatedRoom.RoomName);

            // Restore the original name
            room.RoomName = originalName;
            _roomRepo.UpdateRoom(room);
        }

        [TestMethod]
        public void DeleteRoom_WithValidId_ReturnsTrue()
        {
            // Arrange
            // Create a test room
            var room = new ConferenceRoom
            {
                RoomName = "Room to Delete",
                Capacity = 10,
                Location = "Test Location",
                IsActive = true
            };
            _roomRepo.CreateRoom(room);

            // Get the ID of the created room
            var rooms = _roomRepo.GetAllRooms();
            int roomId = rooms.Find(r => r.RoomName == "Room to Delete").RoomId;

            // Act
            bool result = _roomRepo.DeleteRoom(roomId);

            // Assert
            Assert.IsTrue(result);

            // Check that the room is inactive
            var deletedRoom = _roomRepo.GetRoomById(roomId);
            Assert.IsFalse(deletedRoom.IsActive);
        }
    }
}