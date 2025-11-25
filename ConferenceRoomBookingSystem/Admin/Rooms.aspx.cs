using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using ConferenceRoomBookingSystem.Models;
using ConferenceRoomBookingSystem.Data;
using System.Collections.Generic;

namespace ConferenceRoomBookingSystem.Admin
{
    public partial class Rooms : Page
    {
        private ConferenceRoomRepository roomRepo = new ConferenceRoomRepository();

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

                LoadRooms();
            }
        }

        private void LoadRooms()
        {
            var rooms = roomRepo.GetAllRooms();
            gvRooms.DataSource = rooms;
            gvRooms.DataBind();

            gvRooms.Visible = rooms.Count > 0;
            if (rooms.Count == 0)
                ShowMessage("Brak sal konferencyjnych do wyświetlenia.", "info");
        }

        protected void btnAddRoom_Click(object sender, EventArgs e)
        {
            ClearForm();
            lblFormTitle.Text = "Dodaj nową salę";
            pnlRoomForm.Visible = true;
            ViewState["EditingRoomId"] = null;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                var room = new ConferenceRoom
                {
                    RoomName = txtRoomName.Text.Trim(),
                    Capacity = Convert.ToInt32(txtCapacity.Text),
                    Location = txtLocation.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    HasProjector = chkProjector.Checked,
                    HasWhiteboard = chkWhiteboard.Checked,
                    HasAudioSystem = chkAudio.Checked,
                    HasWiFi = chkWifi.Checked,
                    IsActive = true
                };

                bool success;
                if (ViewState["EditingRoomId"] != null)
                {
                    // Update existing room
                    room.RoomId = (int)ViewState["EditingRoomId"];
                    success = roomRepo.UpdateRoom(room);
                    ShowMessage("Sala została zaktualizowana pomyślnie.", "success");
                }
                else
                {
                    // Create new room
                    success = roomRepo.CreateRoom(room);
                    ShowMessage("Sala została dodana pomyślnie.", "success");
                }

                if (success)
                {
                    pnlRoomForm.Visible = false;
                    LoadRooms();
                }
                else
                {
                    ShowMessage("Wystąpił błąd podczas zapisywania sali.", "danger");
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Błąd: {ex.Message}", "danger");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlRoomForm.Visible = false;
            ClearForm();
        }

        protected void gvRooms_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int roomId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditRoom")
            {
                EditRoom(roomId);
            }
            else if (e.CommandName == "DeleteRoom")
            {
                DeleteRoom(roomId);
            }
        }

        private void EditRoom(int roomId)
        {
            var room = roomRepo.GetRoomById(roomId);
            if (room != null)
            {
                txtRoomName.Text = room.RoomName;
                txtCapacity.Text = room.Capacity.ToString();
                txtLocation.Text = room.Location;
                txtDescription.Text = room.Description;
                chkProjector.Checked = room.HasProjector;
                chkWhiteboard.Checked = room.HasWhiteboard;
                chkAudio.Checked = room.HasAudioSystem;
                chkWifi.Checked = room.HasWiFi;

                lblFormTitle.Text = "Edytuj salę";
                pnlRoomForm.Visible = true;
                ViewState["EditingRoomId"] = roomId;
            }
        }

        private void DeleteRoom(int roomId)
        {
            try
            {
                if (roomRepo.DeleteRoom(roomId))
                {
                    ShowMessage("Sala została usunięta pomyślnie.", "success");
                    LoadRooms();
                }
                else
                {
                    ShowMessage("Wystąpił błąd podczas usuwania sali.", "danger");
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Błąd: {ex.Message}", "danger");
            }
        }

        protected void gvRooms_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Additional row formatting if needed
        }

        public string GetEquipmentBadges(object hasProjector, object hasWhiteboard, object hasAudioSystem, object hasWiFi)
        {
            var equipment = new List<string>();

            if (hasProjector != null && (bool)hasProjector)
                equipment.Add("<span class='equipment-badge'>Projektor</span>");
            if (hasWhiteboard != null && (bool)hasWhiteboard)
                equipment.Add("<span class='equipment-badge'>Tablica</span>");
            if (hasAudioSystem != null && (bool)hasAudioSystem)
                equipment.Add("<span class='equipment-badge'>Audio</span>");
            if (hasWiFi != null && (bool)hasWiFi)
                equipment.Add("<span class='equipment-badge'>Wi-Fi</span>");

            return equipment.Count > 0 ? string.Join(" ", equipment) : "<span class='no-equipment'>Brak</span>";
        }

        public string GetDeleteConfirmation(object roomName)
        {
            return $"return confirm('Czy na pewno chcesz usunąć salę \\'{roomName}\\'?');";
        }

        private void ClearForm()
        {
            txtRoomName.Text = "";
            txtCapacity.Text = "";
            txtLocation.Text = "";
            txtDescription.Text = "";
            chkProjector.Checked = false;
            chkWhiteboard.Checked = false;
            chkAudio.Checked = false;
            chkWifi.Checked = false;
        }

        private void ShowMessage(string message, string type)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = $"alert alert-{type}";
            lblMessage.Visible = true;
        }
    }
}