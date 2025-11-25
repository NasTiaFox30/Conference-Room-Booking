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

        [TestMethod]
        public void CreateBooking_WithValidData_ReturnsTrue()
        {
            // Arrange
            var rooms = _roomRepo.GetAllRooms();
            var room = rooms.First();
            var user = _userRepo.GetUserByUsername("testuser");

            var booking = new Booking
            {
                RoomId = room.RoomId,
                UserId = user.UserId,
                Title = "Test Meeting " + Guid.NewGuid().ToString().Substring(0, 8),
                Description = "Test Description",
                StartTime = DateTime.Now.AddDays(2).Date.AddHours(14),
                EndTime = DateTime.Now.AddDays(2).Date.AddHours(16),
                Attendees = "user1@test.com,user2@test.com",
                Status = "Confirmed"
            };

            // Act
            bool result = _bookingRepo.CreateBooking(booking);

            // Assert
            Assert.IsTrue(result, "The reservation must have been created successfully");
        }

        [TestMethod]
        public void CreateBooking_WithTimeConflict_ReturnsFalse()
        {
            // Arrange
            var rooms = _roomRepo.GetAllRooms();
            var room = rooms.First();
            var user1 = _userRepo.GetUserByUsername("testuser");
            var user2 = _userRepo.GetUserByUsername("adminuser");

            DateTime startTime = DateTime.Now.AddDays(3).Date.AddHours(10);
            DateTime endTime = startTime.AddHours(2);

            // First booking
            var booking1 = new Booking
            {
                RoomId = room.RoomId,
                UserId = user1.UserId,
                Title = "First Meeting",
                StartTime = startTime,
                EndTime = endTime,
                Status = "Confirmed"
            };
            _bookingRepo.CreateBooking(booking1);

            // Second booking with time conflict
            var booking2 = new Booking
            {
                RoomId = room.RoomId,
                UserId = user2.UserId,
                Title = "Second Meeting",
                StartTime = startTime.AddHours(1), // conflict
                EndTime = endTime.AddHours(1),
                Status = "Confirmed"
            };

            // Act
            bool result = _bookingRepo.CreateBooking(booking2);

            // Assert
            Assert.IsFalse(result, "A booking with a time conflict should not be created");
        }

        [TestMethod]
        public void GetUserBookings_WithValidUserId_ReturnsBookings()
        {
            // Arrange
            var user = _userRepo.GetUserByUsername("testuser");

            // Act
            var bookings = _bookingRepo.GetUserBookings(user.UserId);

            // Assert
            Assert.IsNotNull(bookings);
            Assert.IsInstanceOfType(bookings, typeof(List<Booking>));
        }
    }
}