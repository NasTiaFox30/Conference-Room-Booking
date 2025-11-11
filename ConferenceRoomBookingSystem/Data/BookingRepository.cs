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

        public List<Booking> GetUserBookings(int userId, string filter = "Upcoming")
        {
            var bookings = new List<Booking>();
            string query = @"
                SELECT b.*, r.RoomName, u.FirstName + ' ' + u.LastName AS UserName
                FROM Bookings b
                INNER JOIN ConferenceRooms r ON b.RoomId = r.RoomId
                INNER JOIN Users u ON b.UserId = u.UserId
                WHERE b.UserId = @UserId";

            if (filter == "Upcoming")
                query += " AND b.StartTime >= @CurrentTime AND b.Status = 'Confirmed'";
            else if (filter == "Past")
                query += " AND b.StartTime < @CurrentTime";

            query += " ORDER BY b.StartTime DESC";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@CurrentTime", DateTime.Now)
            };

            var dataTable = dbHelper.ExecuteQuery(query, parameters);
            foreach (DataRow row in dataTable.Rows)
            {
                bookings.Add(new Booking
                {
                    BookingId = Convert.ToInt32(row["BookingId"]),
                    RoomId = Convert.ToInt32(row["RoomId"]),
                    UserId = Convert.ToInt32(row["UserId"]),
                    Title = row["Title"].ToString(),
                    Description = row["Description"]?.ToString(),
                    StartTime = Convert.ToDateTime(row["StartTime"]),
                    EndTime = Convert.ToDateTime(row["EndTime"]),
                    Attendees = row["Attendees"]?.ToString(),
                    Status = row["Status"].ToString(),
                    CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                    RoomName = row["RoomName"].ToString(),
                    UserName = row["UserName"].ToString()
                });
            }
            return bookings;
        }

        public Booking GetBookingById(int bookingId)
        {
            var query = @"
                SELECT b.*, r.RoomName, u.FirstName + ' ' + u.LastName AS UserName
                FROM Bookings b
                INNER JOIN ConferenceRooms r ON b.RoomId = r.RoomId
                INNER JOIN Users u ON b.UserId = u.UserId
                WHERE b.BookingId = @BookingId";

            var parameters = new SqlParameter[] { new SqlParameter("@BookingId", bookingId) };
            var dataTable = dbHelper.ExecuteQuery(query, parameters);

            if (dataTable.Rows.Count == 0) return null;

            var row = dataTable.Rows[0];
            return new Booking
            {
                BookingId = Convert.ToInt32(row["BookingId"]),
                RoomId = Convert.ToInt32(row["RoomId"]),
                UserId = Convert.ToInt32(row["UserId"]),
                Title = row["Title"].ToString(),
                Description = row["Description"]?.ToString(),
                StartTime = Convert.ToDateTime(row["StartTime"]),
                EndTime = Convert.ToDateTime(row["EndTime"]),
                Attendees = row["Attendees"]?.ToString(),
                Status = row["Status"].ToString(),
                CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                RoomName = row["RoomName"].ToString(),
                UserName = row["UserName"].ToString()
            };
        }

        public bool CancelBooking(int bookingId)
        {
            var query = "UPDATE Bookings SET Status = 'Cancelled' WHERE BookingId = @BookingId";
            var parameters = new SqlParameter[] { new SqlParameter("@BookingId", bookingId) };
            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }
    }
}