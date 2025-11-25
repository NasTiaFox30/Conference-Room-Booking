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
    }
}