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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtStartTime.Text = "09:00";
                txtEndTime.Text = "10:00";
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchAvailableRooms();
        }

        private void SearchAvailableRooms()
        {
            if (!DateTime.TryParse(txtDate.Text, out DateTime searchDate))
                searchDate = DateTime.Now;

            if (!TimeSpan.TryParse(txtStartTime.Text, out TimeSpan startTime))
                startTime = new TimeSpan(9, 0, 0);

            if (!TimeSpan.TryParse(txtEndTime.Text, out TimeSpan endTime))
                endTime = new TimeSpan(10, 0, 0);
           
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

            if (availableRooms.Any())
            {
                gvAvailableRooms.DataSource = availableRooms;
                gvAvailableRooms.DataBind();
                gvAvailableRooms.Visible = true;
                lblNoRooms.Visible = false;
            }
            else
            {
                gvAvailableRooms.Visible = false;
                lblNoRooms.Visible = true;
            }
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

        protected void gvAvailableRooms_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "BookRoom")
            {
                int roomId = Convert.ToInt32(e.CommandArgument);
                Response.Redirect($"~/Pages/BookRoom.aspx?roomId={roomId}&date={txtDate.Text}&start={txtStartTime.Text}&end={txtEndTime.Text}");
            }
        }
    }
}