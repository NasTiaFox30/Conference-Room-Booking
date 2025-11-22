using ConferenceRoomBookingSystem.Data;
using ConferenceRoomBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace ConferenceRoomBookingSystem.Pages
{
    public partial class BookRoom : Page
    {
        private int roomId;
        private ConferenceRoom room;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!int.TryParse(Request.QueryString["roomId"], out roomId))
                {
                    ShowMessage("Nieprawidłowy identyfikator sali.", "danger");
                    return;
                }
                LoadRoomDetails();
                InitializeBookingForm();
            }
        }

        private void LoadRoomDetails()
        {
            var roomRepo = new ConferenceRoomRepository();
            room = roomRepo.GetRoomById(roomId);
            if (room == null)
            {
                ShowMessage("Sala nie została znaleziona.", "danger");
                return;
            }
            fvRoomDetails.DataSource = new[] { room };
            fvRoomDetails.DataBind();
        }

        private void InitializeBookingForm()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["date"]))
                txtBookingDate.Text = Request.QueryString["date"];
            else
                txtBookingDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            if (!string.IsNullOrEmpty(Request.QueryString["start"]))
                txtBookingStart.Text = Request.QueryString["start"];
            else
                txtBookingStart.Text = "09:00";

            if (!string.IsNullOrEmpty(Request.QueryString["end"]))
                txtBookingEnd.Text = Request.QueryString["end"];
            else
                txtBookingEnd.Text = "10:00";
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(Request.QueryString["roomId"], out roomId))
            {
                ShowMessage("Nieprawidłowy identyfikator sali.", "danger");
                return;
            }

            try
            {
                if (!ValidateBookingParameters())
                {
                    return;
                }

                DateTime startTime = DateTime.Parse(txtBookingDate.Text + " " + txtBookingStart.Text);
                DateTime endTime = DateTime.Parse(txtBookingDate.Text + " " + txtBookingEnd.Text);

                var booking = new Booking
                {
                    RoomId = roomId,
                    UserId = GetCurrentUserId(),
                    Title = txtTitle.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    StartTime = startTime,
                    EndTime = endTime,
                    Attendees = txtAttendees.Text.Trim(),
                    Status = "Confirmed",
                    CreatedDate = DateTime.Now
                };

                var bookingRepo = new BookingRepository();

                // Check conflict validation
                if (!bookingRepo.IsRoomAvailable(roomId, booking.StartTime, booking.EndTime))
                {
                    ShowMessage("Sala jest już zarezerwowana na wybrany czas. Proszę wybrać inny termin.", "danger");
                    return;
                }

                // Save reservation
                if (bookingRepo.CreateBooking(booking))
                {
                    ShowMessage("Sala została pomyślnie zarezerwowana!", "success");
                    ClearForm();
                    // FUTURE TO-DO: Można dodać wysyłkę e-maila
                }
                else
                {
                    ShowMessage("Wystąpił błąd podczas rezerwacji. Spróbuj ponownie.", "danger");
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Błąd: {ex.Message}", "danger");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/SearchRooms.aspx");
        }

        private void ShowMessage(string message, string type)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = $"alert alert-{type}";
            lblMessage.Visible = true;
        }

        private void ClearForm()
        {
            txtTitle.Text = "";
            txtDescription.Text = "";
            txtAttendees.Text = "";
        }

        private int GetCurrentUserId()
        {
            if (Session["UserId"] == null)
                Response.Redirect("~/Pages/Login.aspx");
            return (int)Session["UserId"];
        }

        public string GetEquipmentText(object hasProjector, object hasWhiteboard, object hasAudioSystem, object hasWiFi)
        {
            var equipment = new List<string>();
            if (hasProjector != null && (bool)hasProjector)
                equipment.Add("Projektor");
            if (hasWhiteboard != null && (bool)hasWhiteboard)
                equipment.Add("Tablica");
            if (hasAudioSystem != null && (bool)hasAudioSystem)
                equipment.Add("System audio");
            if (hasWiFi != null && (bool)hasWiFi)
                equipment.Add("Wi-Fi");
            return string.Join(", ", equipment);
        }
    }
}