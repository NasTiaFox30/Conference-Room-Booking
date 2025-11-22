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
            DateTime now = DateTime.Now;
            DateTime nowTime = now.AddHours(6); // 6 hours before start

            // If there are parameters from SearchRooms - use them
            if (!string.IsNullOrEmpty(Request.QueryString["date"]))
            {
                txtBookingDate.Text = Request.QueryString["date"];
                txtBookingStart.Text = Request.QueryString["start"];
                txtBookingEnd.Text = Request.QueryString["end"];
        }
            else
            {
                // Set min date (today)
                txtBookingDate.Text = now.ToString("yyyy-MM-dd");
                txtBookingDate.Attributes["min"] = now.ToString("yyyy-MM-dd");

                // If current time + 6 is more than 22:30 (today and next days)
                if ((nowTime.Date == now.Date && nowTime.TimeOfDay > new TimeSpan(22, 30, 0)) || nowTime.Date > now.Date)
                {
                    // Move to next day
                    DateTime nextDay = (nowTime.Date > now.Date) ? nowTime.Date : now.AddDays(1).Date;
                    txtBookingDate.Text = nextDay.ToString("yyyy-MM-dd");
                    txtBookingStart.Text = "05:00";
                    txtBookingEnd.Text = "05:30";
                }
                else
                {
                    // Not earlier than  5:00
                    if (nowTime.Hour < 5)
                        txtBookingStart.Text = "05:00";
                    else
                        txtBookingStart.Text = nowTime.ToString("HH:mm");

                    // Automatically set end time +30 minutes
                    UpdateEndTime();
                }
            }

            SetTimeConstraints();
        }

        private void SetTimeConstraints()
        {
            // Time reservation rules:
            txtBookingStart.Attributes["min"] = "05:00";
            txtBookingStart.Attributes["max"] = "22:30";
            txtBookingEnd.Attributes["min"] = "05:30";
            txtBookingEnd.Attributes["max"] = "23:00";
        }

        private void UpdateEndTime()
        {
            if (!string.IsNullOrEmpty(txtBookingStart.Text))
            {
                TimeSpan startTime = TimeSpan.Parse(txtBookingStart.Text);
                TimeSpan endTime = startTime.Add(new TimeSpan(0, 30, 0));

                // Check EndTime not later than 23:00
                if (endTime > new TimeSpan(23, 0, 0))
                    endTime = new TimeSpan(23, 0, 0);

                txtBookingEnd.Text = endTime.ToString(@"hh\:mm");
            }
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

        private bool ValidateBookingParameters()
        {
            // Check required fields
            if (string.IsNullOrEmpty(txtTitle.Text.Trim()))
            {
                ShowMessage("Nazwa wydarzenia jest wymagana.", "danger");
                return false;
            }

            // Check: Date
            if (!DateTime.TryParse(txtBookingDate.Text, out DateTime searchDate))
            {
                ShowMessage("Nieprawidłowy format daty.", "danger");
                return false;
            }

            // Check: StartTime
            if (!TimeSpan.TryParse(txtBookingStart.Text, out TimeSpan startTime))
            {
                ShowMessage("Nieprawidłowy format godziny rozpoczęcia.", "danger");
                return false;
            }

            // Check: EndTime
            if (!TimeSpan.TryParse(txtBookingEnd.Text, out TimeSpan endTime))
            {
                ShowMessage("Nieprawidłowy format godziny zakończenia.", "danger");
                return false;
            }

            DateTime now = DateTime.Now;

            var startDateTime = searchDate.Add(startTime);
            var endDateTime = searchDate.Add(endTime);

            // Check 1: Not in the past
            if (startDateTime <= now)
            {
                ShowMessage("Nie można zarezerwować sali w przeszłości. Proszę wybrać przyszłą datę i godzinę.", "danger");
                return false;
            }

            // Check 2: at least 6 hours before
            DateTime nowTime = now.AddHours(6);
            if (startDateTime < nowTime)
            {
                string correctMessage = GetCorrectMinBookingTimeMessage(nowTime);
                ShowMessage(correctMessage, "danger");
                return false;
            }

            // Check 3: StartTime not earlier than 5:00
            if (startTime < new TimeSpan(5, 0, 0))
            {
                ShowMessage("Rezerwacja może rozpocząć się najwcześniej o 5:00 rano.", "danger");
                return false;
            }

            // Check 4: EndTime not later than 23:00
            if (endTime > new TimeSpan(23, 0, 0))
            {
                ShowMessage("Rezerwacja musi zakończyć się najpóźniej o 23:00.", "danger");
                return false;
            }

            // Check 5: EndTime must be later than StartTime
            if (endDateTime <= startDateTime)
            {
                ShowMessage("Godzina zakończenia musi być późniejsza niż godzina rozpoczęcia.", "danger");
                return false;
            }

            // Check 6: min reservation - 30 min
            if ((endDateTime - startDateTime).TotalMinutes < 30)
            {
                ShowMessage("Minimalny czas rezerwacji to 30 minut.", "danger");
                return false;
            }

            return true;
        }

        private string GetCorrectMinBookingTimeMessage(DateTime nowTime)
        {
            DateTime now = DateTime.Now;

            // If now time + 6 hours >  than 22:30 (Bringing to the Next Day)
            if (nowTime.Date > now.Date)
            {
                // Next day at 5:00
                DateTime nextDayMinTime = nowTime.Date.AddHours(5);
                return $"Rezerwacja musi być dokonana co najmniej 6 godzin przed rozpoczęciem. Najwcześniejsza możliwa rezerwacja to {nextDayMinTime:dd.MM.yyyy} 05:00.";
            }
            else
            {
                // Today with calculated time
                return $"Rezerwacja musi być dokonana co najmniej 6 godzin przed rozpoczęciem. Najwcześniejsza możliwa rezerwacja to {nowTime:dd.MM.yyyy HH:mm}.";
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