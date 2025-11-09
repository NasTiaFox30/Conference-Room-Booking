namespace ConferenceRoomBookingSystem.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public string PasswordHash { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
}