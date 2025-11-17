using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using ConferenceRoomBookingSystem.Models;
using ConferenceRoomBookingSystem.Data;

namespace ConferenceRoomBookingSystem.Pages
{
    public partial class MyBookings : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMyBookings();
            }
        }

        private void LoadMyBookings()
        {
            var bookingRepo = new BookingRepository();
            var userId = GetCurrentUserId();
            var bookings = bookingRepo.GetUserBookings(userId, ddlStatusFilter.SelectedValue);

            // Bind desktop grid view
            gvMyBookings.DataSource = bookings;
            gvMyBookings.DataBind();
        }

        protected void ddlStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMyBookings();
        }

        protected void gvMyBookings_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CancelBooking")
            {
                int bookingId = Convert.ToInt32(e.CommandArgument);
                CancelBooking(bookingId);
            }
            else if (e.CommandName == "ViewDetails")
            {
                int bookingId = Convert.ToInt32(e.CommandArgument);
                Response.Redirect($"~/Pages/BookingDetails.aspx?bookingId={bookingId}");
            }
        }

        private void CancelBooking(int bookingId)
        {
            var bookingRepo = new BookingRepository();
            if (bookingRepo.CancelBooking(bookingId))
            {
                LoadMyBookings();
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Помилка при скасуванні бронювання');", true);
            }
        }

        public bool CanCancelBooking(object status, object startTime)
        {
            if (status == null || startTime == null)
                return false;

            string statusStr = status.ToString();
            if (statusStr != "Confirmed")
                return false;

            DateTime start = DateTime.Parse(startTime.ToString());
            return start > DateTime.Now.AddHours(24);
        }
        private int GetCurrentUserId()
        {
            if (Session["UserId"] == null)
                Response.Redirect("~/Pages/Login.aspx");
            return (int)Session["UserId"];
        }
    }
}