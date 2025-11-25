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

        [TestMethod]
        public void CancelBooking_WithValidId_ReturnsTrue()
        {
            // Arrange
            var rooms = _roomRepo.GetAllRooms();
            var room = rooms.First();
            var user = _userRepo.GetUserByUsername("testuser");

            // create a test reservation
            var booking = new Booking
            {
                RoomId = room.RoomId,
                UserId = user.UserId,
                Title = "Meeting to Cancel " + Guid.NewGuid().ToString().Substring(0, 8),
                StartTime = DateTime.Now.AddDays(1).Date.AddHours(15),
                EndTime = DateTime.Now.AddDays(1).Date.AddHours(16),
                Status = "Confirmed"
            };
            _bookingRepo.CreateBooking(booking);

            // Get the ID of the created reservation
            var userBookings = _bookingRepo.GetUserBookings(user.UserId);
            var createdBooking = userBookings.First(b => b.Title.StartsWith("Meeting to Cancel"));
            int bookingId = createdBooking.BookingId;

            // Act
            bool result = _bookingRepo.CancelBooking(bookingId);

            // Assert
            Assert.IsTrue(result, "The cancellation must be successful");

            // Check that the status has changed
            var cancelledBooking = _bookingRepo.GetBookingById(bookingId);
            Assert.AreEqual("Cancelled", cancelledBooking.Status, "Booking status must be 'Cancelled'");
        }

        [TestMethod]
        public void CompleteBookingFlow_IntegrationTest()
        {
            // Arrange
            var rooms = _roomRepo.GetAllRooms();
            var room = rooms.First();
            var user = _userRepo.GetUserByUsername("testuser");

            DateTime startTime = DateTime.Now.AddDays(1).Date.AddHours(10);
            DateTime endTime = startTime.AddHours(1);

            // Act & Assert
            // 1. check the availability of the room
            bool isAvailable = _bookingRepo.IsRoomAvailable(room.RoomId, startTime, endTime);
            Assert.IsTrue(isAvailable, "The room must be available before booking");

            // 2. Making booking
            var booking = new Booking
            {
                RoomId = room.RoomId,
                UserId = user.UserId,
                Title = "Complete Flow Test " + Guid.NewGuid().ToString().Substring(0, 8),
                StartTime = startTime,
                EndTime = endTime,
                Status = "Confirmed"
            };

            bool createResult = _bookingRepo.CreateBooking(booking);
            Assert.IsTrue(createResult, "The reservation must have been created successfully");

            // 3. check that the room is no longer available
            bool isAvailableAfterBooking = _bookingRepo.IsRoomAvailable(room.RoomId, startTime, endTime);
            Assert.IsFalse(isAvailableAfterBooking, "Кімната не повинна бути доступна після бронювання");

            // 4. Check that the reservation has appeared in the user's list
            var userBookings = _bookingRepo.GetUserBookings(user.UserId);
            Assert.IsTrue(userBookings.Any(b => b.Title.StartsWith("Complete Flow Test")),
                "The reservation should appear in the user's list");

            // 5. cancel the reservation
            var createdBooking = userBookings.First(b => b.Title.StartsWith("Complete Flow Test"));
            bool cancelResult = _bookingRepo.CancelBooking(createdBooking.BookingId);
            Assert.IsTrue(cancelResult, "The cancellation must be successful");

            // 6. We are checking that the room is available again
            bool isAvailableAfterCancel = _bookingRepo.IsRoomAvailable(room.RoomId, startTime, endTime);
            Assert.IsTrue(isAvailableAfterCancel, "The room must be available again after cancellation");
        }
    }
}