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
    }
}