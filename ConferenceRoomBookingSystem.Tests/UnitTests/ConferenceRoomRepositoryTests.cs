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
    }
}