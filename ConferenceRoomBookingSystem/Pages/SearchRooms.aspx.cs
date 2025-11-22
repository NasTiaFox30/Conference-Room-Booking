using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ConferenceRoomBookingSystem.Data;
using ConferenceRoomBookingSystem.Models;

namespace ConferenceRoomBookingSystem.Pages
{
    public partial class SearchRooms : Page
    {
        private DateTime _minBookingTime;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                InitializeSearchForm();
        }

        private void InitializeSearchForm()
        {
            DateTime now = DateTime.Now;
            DateTime nowTime = now.AddHours(6); // 6 hours before start

            // Set min date (today)
            txtDate.Text = now.ToString("yyyy-MM-dd");
            txtDate.Attributes["min"] = now.ToString("yyyy-MM-dd");

            // If current time + 6 is more than 22:30 (today and next days)
            if ((nowTime.Date == now.Date && nowTime.TimeOfDay > new TimeSpan(22, 30, 0)) || nowTime.Date > now.Date)
            {
                // Move to next day
                DateTime nextDay = (nowTime.Date > now.Date) ? nowTime.Date : now.AddDays(1).Date;
                txtDate.Text = nextDay.ToString("yyyy-MM-dd");
                txtStartTime.Text = "05:00"; 
                txtEndTime.Text = "05:30";
            }
            else
            {
                // Not earlier than  5:00
                if (nowTime.Hour < 5)
                    txtStartTime.Text = "05:00";
                else
                    txtStartTime.Text = nowTime.ToString("HH:mm");

                // Automatically set end time +30 minutes
                UpdateEndTime();
            }

            SetTimeConstraints();
        }

        private void SetTimeConstraints()
        {
            // Time reservation rules:
            txtStartTime.Attributes["min"] = "05:00";
            txtStartTime.Attributes["max"] = "22:30";
            txtEndTime.Attributes["min"] = "05:30";
            txtEndTime.Attributes["max"] = "23:00";
        }

        private void UpdateEndTime()
        {
            if (!string.IsNullOrEmpty(txtStartTime.Text))
            {
                TimeSpan startTime = TimeSpan.Parse(txtStartTime.Text);
                TimeSpan endTime = startTime.Add(new TimeSpan(0, 30, 0));

                // Check EndTime not later than 23:00
                if (endTime > new TimeSpan(23, 0, 0))
                    endTime = new TimeSpan(23, 0, 0); 

                txtEndTime.Text = endTime.ToString(@"hh\:mm");
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchAvailableRooms();
        }

        private void SearchAvailableRooms()
        {
            // CLEAN: messages and results
            ClearPreviousResults();
            ClearMessages();

            if (!ValidateSearchParameters()) { return; }

            DateTime searchDate = DateTime.Parse(txtDate.Text);
            TimeSpan startTime = TimeSpan.Parse(txtStartTime.Text);
            TimeSpan endTime = TimeSpan.Parse(txtEndTime.Text);

            var startDateTime = searchDate.Add(startTime);
            var endDateTime = searchDate.Add(endTime);

            var roomRepo = new ConferenceRoomRepository();
            var bookingRepo = new BookingRepository();

            var allRooms = roomRepo.GetAllRooms();
            var availableRooms = new List<ConferenceRoom>();

            foreach (var room in allRooms)
            {
                // Filtration by capacity
                if (!string.IsNullOrEmpty(txtCapacity.Text) &&
                    int.TryParse(txtCapacity.Text, out int minCapacity) &&
                    room.Capacity < minCapacity)
                    continue;

                // Filtration by equipment
                if (chkProjector.Checked && !room.HasProjector) continue;
                if (chkWhiteboard.Checked && !room.HasWhiteboard) continue;
                if (chkAudio.Checked && !room.HasAudioSystem) continue;
                if (chkWifi.Checked && !room.HasWiFi) continue;

                // Checking available
                if (!bookingRepo.IsRoomAvailable(room.RoomId, startDateTime, endDateTime))
                    continue;

                availableRooms.Add(room);
            }

            DisplaySearchResults(availableRooms);
        }

        private bool ValidateSearchParameters()
        {
            // Check: Date
            if (!DateTime.TryParse(txtDate.Text, out DateTime searchDate))
            {
                ShowMessage("Nieprawidłowy format daty.", "danger");
                return false;
            }

            // Check: StartTime
            if (!TimeSpan.TryParse(txtStartTime.Text, out TimeSpan startTime))
            {
                ShowMessage("Nieprawidłowy format godziny rozpoczęcia.", "danger");
                return false;
            }

            // Check: EndTime
            if (!TimeSpan.TryParse(txtEndTime.Text, out TimeSpan endTime))
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
                ShowMessage("Nie można wyszukiwać sal w przeszłości. Proszę wybrać przyszłą datę i godzinę.", "danger");
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
                // Next day 5:00 AM
                DateTime nextDayMinTime = nowTime.Date.AddHours(5);
                return $"code:2 Rezerwacja musi być dokonana co najmniej 6 godzin przed rozpoczęciem. Najwcześniejsza możliwa rezerwacja to {nextDayMinTime:dd.MM.yyyy} 05:00.";
            }
            else
            {
                // Today with calculated time
                return $"code:3 Rezerwacja musi być dokonana co najmniej 6 godzin przed rozpoczęciem. Najwcześniejsza możliwa rezerwacja to {nowTime:dd.MM.yyyy HH:mm}.";
            }
        }

        private void DisplaySearchResults(List<ConferenceRoom> availableRooms)
        {
            if (availableRooms.Any())
            {
                // Bind desktop grid view
                gvAvailableRooms.DataSource = availableRooms;
                gvAvailableRooms.DataBind();
                gvAvailableRooms.Visible = true;

                // Bind mobile repeater
                rptMobileRooms.DataSource = availableRooms;
                rptMobileRooms.DataBind();

                ShowMessage($"Znaleziono {availableRooms.Count} dostępnych sal.", "success");
            }
            else
            {
                // No rooms found:
                ClearPreviousResults();
                ShowMessage("Brak dostępnych sal spełniających podane kryteria.", "info");
            }
        }

        private void ShowMessage(string message, string type)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = $"alert alert-{type}";
            lblMessage.Visible = true;
        }

        private void ClearMessages()
        {
            lblMessage.Visible = false;
            lblMessage.Text = string.Empty;
        }

        private void ClearPreviousResults()
        {
            // Desktop
            gvAvailableRooms.DataSource = null;
            gvAvailableRooms.DataBind();
            gvAvailableRooms.Visible = false;

            // Mobile
            rptMobileRooms.DataSource = null;
            rptMobileRooms.DataBind();
        }

        // Method for desktop view (badges)
        public string GetEquipmentBadges(object hasProjector, object hasWhiteboard, object hasAudioSystem, object hasWiFi)
        {
            var equipment = new List<string>();

            if (hasProjector != null && (bool)hasProjector)
                equipment.Add("<span class='equipment-badge'>Projektor</span>");
            if (hasWhiteboard != null && (bool)hasWhiteboard)
                equipment.Add("<span class='equipment-badge'>Tablica</span>");
            if (hasAudioSystem != null && (bool)hasAudioSystem)
                equipment.Add("<span class='equipment-badge'>System audio</span>");
            if (hasWiFi != null && (bool)hasWiFi)
                equipment.Add("<span class='equipment-badge'>Wi-Fi</span>");

            return string.Join(" ", equipment);
        }

        // Method for mobile view (text)
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

        protected void gvAvailableRooms_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            HandleBookCommand(e.CommandName, e.CommandArgument);
        }

        protected void rptMobileRooms_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            HandleBookCommand(e.CommandName, e.CommandArgument);
        }

        private int GetCurrentUserId()
        {
            if (Session["UserId"] == null)
                Response.Redirect("~/Pages/Login.aspx");
            return (int)Session["UserId"];
        }

        private void HandleBookCommand(string commandName, object commandArgument)
        {
            //Checking auth
            GetCurrentUserId();

            if (commandName == "BookRoom")
            {
                int roomId = Convert.ToInt32(commandArgument);
                Response.Redirect($"~/Pages/BookRoom.aspx?roomId={roomId}&date={txtDate.Text}&start={txtStartTime.Text}&end={txtEndTime.Text}");
            }
        }
    }
}