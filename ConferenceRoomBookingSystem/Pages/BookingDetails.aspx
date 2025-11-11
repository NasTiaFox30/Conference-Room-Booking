<%@ Page Title="Szczegóły rezerwacji" Language="C#" MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" CodeBehind="BookingDetails.aspx.cs" 
    Inherits="ConferenceRoomBookingSystem.Pages.BookingDetails" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Styles/BookingDetails.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="booking-details-container">
        <div class="booking-details-header">
            <h2 class="booking-details-title">Szczegóły rezerwacji</h2>
        </div>
    
    </div>
</asp:Content>