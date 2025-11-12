<%@ Page Title="Rejestracja" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Register.aspx.cs"
    Inherits="ConferenceRoomBookingSystem.Pages.Register" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Styles/Register.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="auth-container">
        <div class="auth-header">
            <h2 class="auth-title">Rejestracja nowego użytkownika</h2>
        </div>
    
        <div class="auth-form">
            <div class="form-group">
                <label class="form-label">Login:</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername"
                    ErrorMessage="Pole wymagane" CssClass="validator-error" Display="Dynamic" />
            </div>

            <div class="form-group">
                <label class="form-label">Email:</label>
                <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                    ErrorMessage="Pole wymagane" CssClass="validator-error" Display="Dynamic" />
            </div>

            <div class="name-grid">
                <div class="form-group">
                    <label class="form-label">Imię:</label>
                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName"
                        ErrorMessage="Wymagane" CssClass="validator-error" Display="Dynamic" />
            </div>

                <div class="form-group">
                    <label class="form-label">Nazwisko:</label>
                    <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName"
                        ErrorMessage="Wymagane" CssClass="validator-error" Display="Dynamic" />
                </div>
            </div>

            <div class="form-group">
                <label class="form-label">Dział (opcjonalnie):</label>
                <asp:TextBox ID="txtDepartment" runat="server" CssClass="form-control" />
            </div>

            <div class="form-group">
                <label class="form-label">Hasło:</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                    ErrorMessage="Wymagane" CssClass="validator-error" Display="Dynamic" />
            </div>

            <div class="auth-button-container">
                <asp:Button ID="btnRegister" runat="server" Text="Zarejestruj się" OnClick="btnRegister_Click" CssClass="auth-button auth-button-success" />
            </div>

            <asp:Label ID="lblMessage" runat="server" CssClass="alert" Visible="false" />

        </div>
    </div>
</asp:Content>