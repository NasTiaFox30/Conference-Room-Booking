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
                        // Checking availabel
                        var checkCmd = new SqlCommand(@"
                            SELECT COUNT(*) FROM Bookings 
                            WHERE RoomId = @RoomId 
                              AND Status != 'Cancelled'
                              AND (StartTime < @EndTime AND EndTime > @StartTime)", connection, transaction);
                        checkCmd.Parameters.AddWithValue("@RoomId", booking.RoomId);
                        checkCmd.Parameters.AddWithValue("@StartTime", booking.StartTime);
                        checkCmd.Parameters.AddWithValue("@EndTime", booking.EndTime);

                        if (Convert.ToInt32(checkCmd.ExecuteScalar()) > 0)
                            return false;
                        
                        // INSERT
                        var insertCmd = new SqlCommand(@"
                            INSERT INTO Bookings 
                            (RoomId, UserId, Title, Description, StartTime, EndTime, Attendees, Status, CreatedDate)
                            VALUES 
                            (@RoomId, @UserId, @Title, @Description, @StartTime, @EndTime, @Attendees, @Status, @CreatedDate)",
                            connection, transaction);

                        insertCmd.Parameters.AddWithValue("@RoomId", booking.RoomId);
                        insertCmd.Parameters.AddWithValue("@UserId", booking.UserId);
                        insertCmd.Parameters.AddWithValue("@Title", booking.Title);
                        insertCmd.Parameters.AddWithValue("@Description", (object)booking.Description ?? DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@StartTime", booking.StartTime);
                        insertCmd.Parameters.AddWithValue("@EndTime", booking.EndTime);
                        insertCmd.Parameters.AddWithValue("@Attendees", (object)booking.Attendees ?? DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@Status", booking.Status);
                        insertCmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

                        insertCmd.ExecuteNonQuery();
                        transaction.Commit();
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