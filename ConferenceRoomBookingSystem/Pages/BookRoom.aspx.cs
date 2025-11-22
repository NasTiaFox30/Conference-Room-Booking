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
                    ShowError("Nieprawidłowy identyfikator sali.");
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
                ShowError("Sala nie została znaleziona.");
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
                ShowError("Nieprawidłowy identyfikator sali.");
                return;
            }

            try
            {
                var booking = new Booking
                {
                    RoomId = roomId,
                    UserId = GetCurrentUserId(),
                    Title = txtTitle.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    StartTime = DateTime.Parse(txtBookingDate.Text + " " + txtBookingStart.Text),
                    EndTime = DateTime.Parse(txtBookingDate.Text + " " + txtBookingEnd.Text),
                    Attendees = txtAttendees.Text.Trim(),
                    Status = "Confirmed",
                    CreatedDate = DateTime.Now
                };

                var bookingRepo = new BookingRepository();

                // Check conflict validation
                if (!bookingRepo.IsRoomAvailable(roomId, booking.StartTime, booking.EndTime))
                {
                    ShowError("Sala jest już zarezerwowana na wybrany czas. Proszę wybrać inny termin.");
                    return;
                }

                // Save reservation
                if (bookingRepo.CreateBooking(booking))
                {
                    ShowSuccess("Sala została pomyślnie zarezerwowana!");
                    // FUTURE TO-DO: Można dodać wysyłkę e-maila
                }
                else
                {
                    ShowError("Wystąpił błąd podczas rezerwacji. Spróbuj ponownie.");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Błąd: {ex.Message}");
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

        private void ShowSuccess(string message)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = "alert alert-success";
            lblMessage.Visible = true;
            // Clean Form
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