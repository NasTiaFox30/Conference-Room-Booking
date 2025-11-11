<%@ Page Title="Moje rezerwacje" Language="C#" MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" CodeBehind="MyBookings.aspx.cs" 
    Inherits="ConferenceRoomBookingSystem.Pages.MyBookings" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Styles/MyBookings.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="my-bookings-container">
        <div class="my-bookings-header">
            <h2 class="my-bookings-title">Moje rezerwacje</h2>
        </div>
    
    </div>
</asp:Content>