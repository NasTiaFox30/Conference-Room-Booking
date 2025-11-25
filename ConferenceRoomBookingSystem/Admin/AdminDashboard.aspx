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

        <div class="admin-nav">
            <asp:Button ID="btnRooms" runat="server" Text="Zarządzanie salami" 
                OnClick="btnRooms_Click" CssClass="admin-nav-btn btn btn-primary" />
            <asp:Button ID="btnBookings" runat="server" Text="Zarządzanie rezerwacjami" 
                OnClick="btnBookings_Click" CssClass="admin-nav-btn btn btn-info" />
        </div>

        <div class="admin-stats">
            <div class="stat-card">
                <div class="stat-number"><asp:Label ID="lblTotalRooms" runat="server" Text="0" /></div>
                <div class="stat-label">Wszystkich sal</div>
            </div>
            <div class="stat-card">
                <div class="stat-number"><asp:Label ID="lblActiveBookings" runat="server" Text="0" /></div>
                <div class="stat-label">Aktywnych rezerwacji</div>
            </div>
            <div class="stat-card">
                <div class="stat-number"><asp:Label ID="lblTodayBookings" runat="server" Text="0" /></div>
                <div class="stat-label">Rezerwacji na dziś</div>
            </div>
        </div>
    </div>
</asp:Content>