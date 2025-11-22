using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using ConferenceRoomBookingSystem.Models;
using ConferenceRoomBookingSystem.Data;
using System.Collections.Generic;

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

            // Bind mobile repeater
            rptMobileBookings.DataSource = bookings;
            rptMobileBookings.DataBind();

            // Show/hide empty message
            bool hasBookings = (bookings != null && bookings.Count > 0);
            gvMyBookings.Visible = hasBookings;
            lblNoBookings.Visible = !hasBookings;
        }

        protected void ddlStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMyBookings();
        }

        protected void gvMyBookings_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            HandleBookingCommand(e.CommandName, e.CommandArgument);
        }

        protected void rptMobileBookings_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            HandleBookingCommand(e.CommandName, e.CommandArgument);
        }

        private void HandleBookingCommand(string commandName, object commandArgument)
        {
            if (commandName == "CancelBooking")
            {
                int bookingId = Convert.ToInt32(commandArgument);
                CancelBooking(bookingId);
            }
            else if (commandName == "ViewDetails")
            {
                int bookingId = Convert.ToInt32(commandArgument);
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

        // Helper method for formatting dates in mobile view
        public string FormatDate(object dateTime)
        {
            if (dateTime == null) return string.Empty;
            return Convert.ToDateTime(dateTime).ToString("dd.MM.yyyy HH:mm");
        }

        private void ShowMessage(string message, string type)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = $"alert alert-{type}";
            lblMessage.Visible = true;
        }

        private int GetCurrentUserId()
        {
            if (Session["UserId"] == null)
                Response.Redirect("~/Pages/Login.aspx");
            return (int)Session["UserId"];
        }
    }
}