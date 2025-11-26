using ConferenceRoomBookingSystem.Data;
using ConferenceRoomBookingSystem.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ConferenceRoomBookingSystem.Tests
{
    [TestClass]
    public class PerformanceTests
    {
        [TestMethod]
        public void GetAllRooms_PerformanceTest_ShouldCompleteInReasonableTime()
        {
            // Arrange
            var roomRepo = new ConferenceRoomRepository();
            var stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();
            var rooms = roomRepo.GetAllRooms();
            stopwatch.Stop();

            // Assert
            Assert.IsTrue(stopwatch.ElapsedMilliseconds < 1000,
                $"GetAllRooms took {stopwatch.ElapsedMilliseconds}ms, expected less than 1000ms");
        }

        [TestMethod]
        public void ConcurrentBookings_ShouldHandleMultipleRequests()
        {
            // Arrange
            var bookingRepo = new BookingRepository();
            int concurrentRequests = 10;
            var tasks = new Task<bool>[concurrentRequests];

            DateTime startTime = DateTime.Now.AddDays(7).Date.AddHours(10);

            // Act
            for (int i = 0; i < concurrentRequests; i++)
            {
                int roomId = (i % 5) + 1; // 5 rooms for testing
                tasks[i] = Task.Run(() =>
                {
                    var booking = new Booking
                    {
                        RoomId = roomId,
                        UserId = 1,
                        Title = $"Concurrent Test {Guid.NewGuid()}",
                        StartTime = startTime.AddMinutes(i * 30), // Stagger start times
                        EndTime = startTime.AddMinutes(i * 30 + 60),
                        Status = "Confirmed"
                    };
                    return bookingRepo.CreateBooking(booking);
                });
            }

            Task.WaitAll(tasks);

            // Assert
            int successfulBookings = 0;
            foreach (var task in tasks)
            {
                if (task.Result) successfulBookings++;
            }

            Assert.IsTrue(successfulBookings > 0, "At least some bookings should succeed");
        }
    }
}