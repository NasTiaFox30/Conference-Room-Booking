using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using ConferenceRoomBookingSystem.Models;

namespace ConferenceRoomBookingSystem.Data
{
    public class ConferenceRoomRepository
    {
        private readonly DatabaseHelper dbHelper;
        public ConferenceRoomRepository() => dbHelper = new DatabaseHelper();

        public List<ConferenceRoom> GetAllRooms()
        {
            var rooms = new List<ConferenceRoom>();
            var query = @"SELECT * FROM ConferenceRooms WHERE IsActive = 1 ORDER BY RoomName";
            var dataTable = dbHelper.ExecuteQuery(query);
            
            foreach (DataRow row in dataTable.Rows)
            {
                rooms.Add(new ConferenceRoom
                {
                    RoomId = Convert.ToInt32(row["RoomId"]),
                    RoomName = row["RoomName"].ToString(),
                    Capacity = Convert.ToInt32(row["Capacity"]),
                    Location = row["Location"].ToString(),
                    Description = row["Description"].ToString(),
                    HasProjector = Convert.ToBoolean(row["HasProjector"]),
                    HasWhiteboard = Convert.ToBoolean(row["HasWhiteboard"]),
                    HasAudioSystem = Convert.ToBoolean(row["HasAudioSystem"]),
                    HasWiFi = Convert.ToBoolean(row["HasWiFi"]),
                    IsActive = Convert.ToBoolean(row["IsActive"]),
                    CreatedDate = Convert.ToDateTime(row["CreatedDate"])
                });
            }
            return rooms;
        }

        public ConferenceRoom GetRoomById(int roomId)
        {
            var query = @"SELECT * FROM ConferenceRooms WHERE RoomId = @RoomId";
            var parameters = new SqlParameter[] { new SqlParameter("@RoomId", roomId) };
            var dataTable = dbHelper.ExecuteQuery(query, parameters);

            if (dataTable.Rows.Count == 0) return null;

            var row = dataTable.Rows[0];
            return new ConferenceRoom
            {
                RoomId = Convert.ToInt32(row["RoomId"]),
                RoomName = row["RoomName"].ToString(),
                Capacity = Convert.ToInt32(row["Capacity"]),
                Location = row["Location"].ToString(),
                Description = row["Description"].ToString(),
                HasProjector = Convert.ToBoolean(row["HasProjector"]),
                HasWhiteboard = Convert.ToBoolean(row["HasWhiteboard"]),
                HasAudioSystem = Convert.ToBoolean(row["HasAudioSystem"]),
                HasWiFi = Convert.ToBoolean(row["HasWiFi"]),
                CreatedDate = Convert.ToDateTime(row["CreatedDate"])
            };
        }

        public bool CreateRoom(ConferenceRoom room)
        {
            var query = @"
                INSERT INTO ConferenceRooms 
                (RoomName, Capacity, Location, Description, HasProjector, HasWhiteboard, HasAudioSystem, HasWiFi, IsActive, CreatedDate)
                VALUES 
                (@RoomName, @Capacity, @Location, @Description, @HasProjector, @HasWhiteboard, @HasAudioSystem, @HasWiFi, @IsActive, @CreatedDate)";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@RoomName", room.RoomName),
                new SqlParameter("@Capacity", room.Capacity),
                new SqlParameter("@Location", (object)room.Location ?? DBNull.Value),
                new SqlParameter("@Description", (object)room.Description ?? DBNull.Value),
                new SqlParameter("@HasProjector", room.HasProjector),
                new SqlParameter("@HasWhiteboard", room.HasWhiteboard),
                new SqlParameter("@HasAudioSystem", room.HasAudioSystem),
                new SqlParameter("@HasWiFi", room.HasWiFi),
                new SqlParameter("@IsActive", room.IsActive),
                new SqlParameter("@CreatedDate", DateTime.Now)
            };

            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool UpdateRoom(ConferenceRoom room)
        {
            var query = @"
                UPDATE ConferenceRooms 
                SET RoomName = @RoomName, Capacity = @Capacity, Location = @Location, 
                    Description = @Description, HasProjector = @HasProjector, 
                    HasWhiteboard = @HasWhiteboard, HasAudioSystem = @HasAudioSystem, 
                    HasWiFi = @HasWiFi
                WHERE RoomId = @RoomId";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@RoomId", room.RoomId),
                new SqlParameter("@RoomName", room.RoomName),
                new SqlParameter("@Capacity", room.Capacity),
                new SqlParameter("@Location", (object)room.Location ?? DBNull.Value),
                new SqlParameter("@Description", (object)room.Description ?? DBNull.Value),
                new SqlParameter("@HasProjector", room.HasProjector),
                new SqlParameter("@HasWhiteboard", room.HasWhiteboard),
                new SqlParameter("@HasAudioSystem", room.HasAudioSystem),
                new SqlParameter("@HasWiFi", room.HasWiFi)
            };

            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteRoom(int roomId)
        {
            var query = "UPDATE ConferenceRooms SET IsActive = 0 WHERE RoomId = @RoomId";
            var parameters = new SqlParameter[] { new SqlParameter("@RoomId", roomId) };
            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }
    }
}