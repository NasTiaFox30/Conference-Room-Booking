<%@ Page Title="Logowanie" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Login.aspx.cs"
    Inherits="ConferenceRoomBookingSystem.Pages.Login" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Styles/Login.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="auth-container">
    
        <div class="auth-header">
            <h2 class="auth-title">Logowanie do systemu</h2>
        </div>

        <div class="auth-form">
            <div class="form-group">
                <label class="form-label">Login:</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Wprowadź login" />
                <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername"
                    ErrorMessage="Wprowadź login" CssClass="validator-error" Display="Dynamic" />
            </div>

            <div class="form-group">
                <label class="form-label">Hasło:</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Wprowadź hasło" />
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                    ErrorMessage="Wprowadź hasło" CssClass="validator-error" Display="Dynamic" />
            </div>
        </div>
    </div>
</asp:Content>