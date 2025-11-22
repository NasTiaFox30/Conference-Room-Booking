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

            }
        }

    }
}
