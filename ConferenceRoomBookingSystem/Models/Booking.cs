using System;

namespace ConferenceRoomBookingSystem.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Attendees { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}