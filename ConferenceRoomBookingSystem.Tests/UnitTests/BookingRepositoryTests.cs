using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConferenceRoomBookingSystem.Data;
using ConferenceRoomBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConferenceRoomBookingSystem.Tests.UnitTests
{
    [TestClass]
    public class BookingRepositoryTests
    {
        private BookingRepository _bookingRepo;
        private ConferenceRoomRepository _roomRepo;
        private UsersRepository _userRepo;

        [TestInitialize]
        public void Setup()
        {
            _bookingRepo = new BookingRepository();
            _roomRepo = new ConferenceRoomRepository();
            _userRepo = new UsersRepository();
        }

        [TestMethod]
        public void IsRoomAvailable_WhenRoomAvailable_ReturnsTrue()
        {
            // Arrange
            var rooms = _roomRepo.GetAllRooms();
            var room = rooms.First();
            DateTime startTime = DateTime.Now.AddDays(1).Date.AddHours(10);
            DateTime endTime = startTime.AddHours(2);

            // Act
            bool result = _bookingRepo.IsRoomAvailable(room.RoomId, startTime, endTime);

            // Assert
            Assert.IsTrue(result, "The room must be available");
        }

        [TestMethod]
        public void IsRoomAvailable_WhenRoomBooked_ReturnsFalse()
        {
            // Arrange
            var rooms = _roomRepo.GetAllRooms();
            var room = rooms.First();
            var user = _userRepo.GetUserByUsername("testuser");

            DateTime startTime = DateTime.Now.AddDays(1).Date.AddHours(10);
            DateTime endTime = startTime.AddHours(2);

            // We create a reservation to block a room
            var booking = new Booking
            {
                RoomId = room.RoomId,
                UserId = user.UserId,
                Title = "Blocking Meeting",
                StartTime = startTime,
                EndTime = endTime,
                Status = "Confirmed"
            };
            _bookingRepo.CreateBooking(booking);

            // Act
            bool result = _bookingRepo.IsRoomAvailable(room.RoomId, startTime, endTime);

            // Assert
            Assert.IsFalse(result, "The room must not be available after booking");
        }
    }
}