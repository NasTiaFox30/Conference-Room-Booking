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

        public bool ValidateUser(string username, string password)
        {
            var user = GetUserByUsername(username);
            if (user == null || string.IsNullOrEmpty(user.PasswordHash)) return false;
            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }

        public bool CreateUser(User user, string plainPassword)
        {
            var query = @"
                INSERT INTO Users (Username, Email, FirstName, LastName, Department, PasswordHash, IsAdmin, IsActive)
                VALUES (@Username, @Email, @FirstName, @LastName, @Department, @PasswordHash, @IsAdmin, @IsActive)";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Username", user.Username),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@FirstName", user.FirstName),
                new SqlParameter("@LastName", user.LastName),
                new SqlParameter("@Department", (object)user.Department ?? DBNull.Value),
                new SqlParameter("@PasswordHash", BCrypt.Net.BCrypt.HashPassword(plainPassword)),
                new SqlParameter("@IsAdmin", user.IsAdmin),
                new SqlParameter("@IsActive", user.IsActive)
            };

            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public User GetUserById(int userId)
        {
            var query = "SELECT * FROM Users WHERE UserId = @UserId";
            var parameters = new SqlParameter[] { new SqlParameter("@UserId", userId) };
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