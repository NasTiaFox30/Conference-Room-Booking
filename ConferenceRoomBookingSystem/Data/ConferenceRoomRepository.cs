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

    }
}