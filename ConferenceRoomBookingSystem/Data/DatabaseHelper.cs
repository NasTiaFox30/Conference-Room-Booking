using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;

namespace ConferenceRoomBookingSystem.Data
{
    public class DatabaseHelper
    {
        private readonly string connectionString;
       
        public DatabaseHelper()
        {
            connectionString = ConfigurationManager.ConnectionStrings["BookingDB"].ConnectionString;

            if (connectionString.Contains("|DataDirectory|"))
            {
                string dataDirectory;
                if (HttpContext.Current != null)
                {
                    dataDirectory = HttpContext.Current.Server.MapPath("~/App_Data");
                }
                else
                {
                    // When HttpContext is not availabel:
                    dataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory") as string ??
                                  AppDomain.CurrentDomain.BaseDirectory + "App_Data";
                }
                connectionString = connectionString.Replace("|DataDirectory|", dataDirectory);
            }
        }

    }
}