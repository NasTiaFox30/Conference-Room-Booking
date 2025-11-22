using System;
using System.Web.UI;
using ConferenceRoomBookingSystem.Data;

namespace ConferenceRoomBookingSystem.Admin
{
    public partial class AdminDashboard : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if user is admin
                if (Session["IsAdmin"] == null || !(bool)Session["IsAdmin"])
                {
                    Response.Redirect("~/Pages/Default.aspx");
                    return;
                }

                LoadStatistics();
            }
        }

        private void LoadStatistics()
        {
            var roomRepo = new ConferenceRoomRepository();
            var bookingRepo = new BookingRepository();

            // Get total rooms
            var rooms = roomRepo.GetAllRooms();
            lblTotalRooms.Text = rooms.Count.ToString();

            // Get active bookings (today and future)
            var allBookings = bookingRepo.GetAllBookings();
            var activeBookings = allBookings.FindAll(b =>
                b.Status == "Confirmed" && b.StartTime >= DateTime.Today);
            lblActiveBookings.Text = activeBookings.Count.ToString();

            // Get today's bookings
            var todayBookings = allBookings.FindAll(b =>
                b.Status == "Confirmed" && b.StartTime.Date == DateTime.Today);
            lblTodayBookings.Text = todayBookings.Count.ToString();
        }
    }
}