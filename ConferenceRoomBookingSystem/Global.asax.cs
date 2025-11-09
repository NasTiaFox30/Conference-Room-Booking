using System;
using System.Web;

namespace ConferenceRoomBookingSystem
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            string appDataPath = Server.MapPath("~/App_Data");
            AppDomain.CurrentDomain.SetData("DataDirectory", appDataPath);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // Error handling logic:
            Exception exc = Server.GetLastError();
            // Here will log the error or notify developers
        }
    }
}