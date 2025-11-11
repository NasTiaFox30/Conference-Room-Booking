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

        private void PrefillBookingTimes()
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

        private void ShowError(string message)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = "alert alert-danger";
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