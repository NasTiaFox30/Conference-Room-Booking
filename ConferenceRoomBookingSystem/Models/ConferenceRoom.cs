using System;

namespace ConferenceRoomBookingSystem.Models
{
    public class ConferenceRoom
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public int Capacity { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

        // Additional properties for room features:
        public bool HasProjector { get; set; }
        public bool HasWhiteboard { get; set; }
        public bool HasAudioSystem { get; set; }
        public bool HasWiFi { get; set; }
    }
}