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

    }
}