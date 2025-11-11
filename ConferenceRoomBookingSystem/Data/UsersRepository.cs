using System;
using System.Data;
using System.Data.SqlClient;
using ConferenceRoomBookingSystem.Models;
using BCrypt.Net;

namespace ConferenceRoomBookingSystem.Data
{
    public class UsersRepository
    {
        private readonly DatabaseHelper dbHelper;
        public UsersRepository() => dbHelper = new DatabaseHelper();

        public User GetUserByUsername(string username)
        {
            var query = "SELECT * FROM Users WHERE Username = @Username AND IsActive = 1";
            var parameters = new SqlParameter[] { new SqlParameter("@Username", username) };
            var dt = dbHelper.ExecuteQuery(query, parameters);

            if (dt.Rows.Count == 0) return null;

            var row = dt.Rows[0];
            return new User
            {
                UserId = Convert.ToInt32(row["UserId"]),
                Username = row["Username"].ToString(),
                Email = row["Email"].ToString(),
                FirstName = row["FirstName"].ToString(),
                LastName = row["LastName"].ToString(),
                Department = row["Department"]?.ToString(),
                PasswordHash = row["PasswordHash"]?.ToString(),
                IsAdmin = Convert.ToBoolean(row["IsAdmin"]),
                IsActive = Convert.ToBoolean(row["IsActive"])
            };
        }

    }
}