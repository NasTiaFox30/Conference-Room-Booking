<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" 
    Inherits="ConferenceRoomBookingSystem.Admin.AdminDashboard" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Styles/Admin.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="admin-container">
        <div class="admin-header">
            <h2 class="admin-title">Panel administracyjny</h2>
            <p class="admin-subtitle">Zarządzaj salami konferencyjnymi i rezerwacjami</p>
        </div>

        <!-- Message block -->
        <asp:Label ID="lblMessage" runat="server" CssClass="alert" Visible="false"></asp:Label>

    </div>
</asp:Content>