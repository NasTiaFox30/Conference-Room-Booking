using System;
using System.Web;
using System.Web.UI;

namespace ConferenceRoomBookingSystem.Pages
{
    public partial class Logout : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        private void PerformLogout()
        {
            try
            {
                // Clear sesion
                Session.Clear();
                Session.Abandon();

                // Delete cookies 
                if (Response.Cookies["ASP.NET_SessionId"] != null)
                {
                    Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                    Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
        }

                // Delete authentication cookie
                if (Response.Cookies[".ASPXAUTH"] != null)
                {
                    Response.Cookies[".ASPXAUTH"].Value = string.Empty;
                    Response.Cookies[".ASPXAUTH"].Expires = DateTime.Now.AddMonths(-20);
                }

                // Clear Cache
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                Response.Cache.SetNoStore();
            catch (Exception)
            {

    }
}
    }
}