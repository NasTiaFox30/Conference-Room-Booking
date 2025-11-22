using System;
using System.Web.UI;
using ConferenceRoomBookingSystem.Data;

namespace ConferenceRoomBookingSystem.Admin
{
    public partial class AdminDashboard : Page
    {
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

                LoadStatistics();
            }
        }

    }
}