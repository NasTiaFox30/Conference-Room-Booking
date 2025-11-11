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

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                var dataTable = new DataTable();
                var adapter = new SqlDataAdapter(command);

                connection.Open();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        public int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }
        public object ExecuteScalar(string query, SqlParameter[] parameters = null)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                connection.Open();
                return command.ExecuteScalar();
            }
        }
    }
}