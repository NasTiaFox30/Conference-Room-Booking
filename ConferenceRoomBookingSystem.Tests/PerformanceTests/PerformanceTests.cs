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
    }
}