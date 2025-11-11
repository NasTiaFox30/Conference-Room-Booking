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

        public bool IsRoomAvailable(int roomId, DateTime startTime, DateTime endTime)
        {
            var query = @"
                SELECT COUNT(*) 
                FROM Bookings 
                WHERE RoomId = @RoomId 
                  AND Status != 'Cancelled'
                  AND (StartTime < @EndTime AND EndTime > @StartTime)";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@RoomId", roomId),
                new SqlParameter("@StartTime", startTime),
                new SqlParameter("@EndTime", endTime)
            };

            var result = dbHelper.ExecuteScalar(query, parameters);
            return Convert.ToInt32(result) == 0;
        }

        public bool CreateBooking(Booking booking)
        {
            using (var connection = dbHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

    }
}