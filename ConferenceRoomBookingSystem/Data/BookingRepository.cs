using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using ConferenceRoomBookingSystem.Models;

namespace ConferenceRoomBookingSystem.Data
{
    public class BookingRepository
    {
        private readonly DatabaseHelper dbHelper;
        public BookingRepository() => dbHelper = new DatabaseHelper();

    }
}