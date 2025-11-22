using ConferenceRoomBookingSystem.Data;
using System;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ConferenceRoomBookingSystem.Admin
{
    public partial class Bookings : Page
    {
        private BookingRepository bookingRepo = new BookingRepository();

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

                InitializeFilters();
                LoadBookings();
            }
        }

        private void InitializeFilters()
        {
            txtDateFrom.Text = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd");
            txtDateTo.Text = DateTime.Today.AddDays(30).ToString("yyyy-MM-dd");
        }

        private void LoadBookings()
        {
            DateTime? dateFrom = string.IsNullOrEmpty(txtDateFrom.Text) ? (DateTime?)null : DateTime.Parse(txtDateFrom.Text);
            DateTime? dateTo = string.IsNullOrEmpty(txtDateTo.Text) ? (DateTime?)null : DateTime.Parse(txtDateTo.Text);

            var bookings = bookingRepo.GetAllBookings(ddlStatusFilter.SelectedValue, dateFrom, dateTo);
            gvBookings.DataSource = bookings;
            gvBookings.DataBind();

            gvBookings.Visible = bookings.Count > 0;
            if (bookings.Count == 0)
                ShowMessage("Brak rezerwacji do wyświetlenia.", "info");
        }
        protected void btnClearFilters_Click(object sender, EventArgs e)
        {
            ddlStatusFilter.SelectedValue = "All";
            txtDateFrom.Text = "";
            txtDateTo.Text = "";
            LoadBookings();
        }

        protected void gvBookings_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int bookingId = Convert.ToInt32(e.CommandArgument);

            switch (e.CommandName)
            {
                case "ConfirmBooking":
                    UpdateBookingStatus(bookingId, "Confirmed");
                    break;
                case "CancelBooking":
                    UpdateBookingStatus(bookingId, "Cancelled");
                    break;
                case "ViewDetails":
                    Response.Redirect($"~/Pages/BookingDetails.aspx?bookingId={bookingId}");
                    break;
            }
        }


        private void ShowMessage(string message, string type)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = $"alert alert-{type}";
            lblMessage.Visible = true;
        }
    }
}
