using System;
using System.Web.UI;
using ConferenceRoomBookingSystem.Data;
using ConferenceRoomBookingSystem.Models;

namespace ConferenceRoomBookingSystem.Pages
{
    public partial class Register : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] != null)
                Response.Redirect("~/Pages/Default.aspx");
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var repo = new UsersRepository();
                if (repo.GetUserByUsername(txtUsername.Text.Trim()) != null)
                {
                    ShowMessage("Użytkownik o takiej nazwie już istnieje.", "danger");
                    return;
                }
                
            }
        }

        private void ShowMessage(string text, string type)
        {
            lblMessage.Text = text;
            lblMessage.CssClass = $"alert alert-{type}";
            lblMessage.Visible = true;
        }
    }
}