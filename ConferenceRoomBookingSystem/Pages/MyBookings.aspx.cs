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
            rptMobileBookings.Visible = hasBookings;
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

        protected void gvMyBookings_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Get info about reservation
                var startTimeData = DataBinder.Eval(e.Row.DataItem, "StartTime");
                var statusData = DataBinder.Eval(e.Row.DataItem, "Status");

                if (startTimeData != null && statusData != null)
                {
                    DateTime startTime = DateTime.Parse(startTimeData.ToString());
                    string status = statusData.ToString();

                    // Important rezervations (less than 2h before)
                    if (status == "Confirmed" && IsUrgentBooking(startTime))
                        e.Row.CssClass += " urgent-booking";
                    }
                }
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
            try
            {
            var bookingRepo = new BookingRepository();

                // Get info about reservation (for cancel)
                var booking = bookingRepo.GetBookingById(bookingId);
                if (booking == null)
                {
                    ShowMessage("Rezerwacja nie została znaleziona.", "danger");
                    return;
                }

                // Check if user can cancel
                if (!CanCancelBooking(booking.Status, booking.StartTime))
                {
                    ShowMessage("Nie można anulować tej rezerwacji. Możliwość anulowania wygasa na 2 godziny przed rozpoczęciem wydarzenia.", "danger");
                    return;
                }

            if (bookingRepo.CancelBooking(bookingId))
            {
                    ShowMessage($"Rezerwacja sali {booking.RoomName} na {booking.StartTime:dd.MM.yyyy HH:mm} została anulowana.", "success");
                LoadMyBookings();
            }
            else
            {
                    ShowMessage("Wystąpił błąd podczas anulowania rezerwacji. Spróbuj ponownie.", "danger");
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Błąd: {ex.Message}", "danger");
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
            // Check: less than 2 hours before start
            return start > DateTime.Now.AddHours(2);
        }

        public string GetTimeBadgeClass(object startTime, object endTime, object status)
        {
            if (status == null)
                return "time-badge-neutral";

            string statusStr = status.ToString();

            if (statusStr == "Cancelled")
                return "time-badge-cancelled";

            if (startTime == null || endTime == null)
                return "time-badge-neutral";

            DateTime start = DateTime.Parse(startTime.ToString());
            DateTime end = DateTime.Parse(endTime.ToString());
            DateTime now = DateTime.Now;

            if (now >= start && now <= end)
                return "time-badge-now"; // Right now
            else if (start > now && start <= now.AddHours(2))
                return "time-badge-soon"; // Less than 2h before
            else if (start > now.AddHours(2) && start <= now.AddHours(24))
                return "time-badge-upcoming"; // More than 24 h
            else
                return "time-badge-neutral"; // Later
        }

        public string GetTimeBadgeText(object startTime, object endTime, object status)
        {
            if (status == null)
                return "";

            string statusStr = status.ToString();

            if (statusStr == "Cancelled")
                return "Anulowana";

            if (startTime == null || endTime == null)
                return "";

            DateTime start = DateTime.Parse(startTime.ToString());
            DateTime end = DateTime.Parse(endTime.ToString());
            DateTime now = DateTime.Now;

            if (now >= start && now <= end)
                return "TERAZ";
            else if (start > now)
            {
                TimeSpan timeLeft = start - now;

                if (timeLeft.TotalHours < 1)
                    return $"za {timeLeft.Minutes}m";
                else if (timeLeft.TotalHours < 2)
                    return $"za {timeLeft.Hours}h {timeLeft.Minutes}m";
                else if (timeLeft.TotalHours < 24)
                    return $"za {timeLeft.Hours}h";
                else
                    return $"za {timeLeft.Days}d";
            }
            else
            {
                return "Zakończona";
            }
        }
        // Confirm canceling
        public string GetCancelConfirmation(object roomName, object startTime)
        {
            if (roomName == null || startTime == null)
                return "return confirm('Czy na pewno chcesz anulować tę rezerwację?');";

            string room = roomName.ToString();
            DateTime start = DateTime.Parse(startTime.ToString());
            string formattedDate = start.ToString("dd.MM.yyyy HH:mm");

            return $"return confirm('Czy na pewno chcesz anulować rezerwację sali {room} na {formattedDate}?');";
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