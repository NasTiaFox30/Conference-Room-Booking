using ConferenceRoomBookingSystem.Data;
using System;
using System.Data.SqlClient;

namespace ConferenceRoomBookingSystem.Tests.TestHelpers
{
    public static class TestDatabaseInitializer
    {
        private static readonly string ConnectionString = new DatabaseHelper().GetConnection().ConnectionString;
    }
}