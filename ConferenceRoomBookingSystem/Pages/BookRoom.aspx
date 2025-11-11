<%@ Page Title="Rezerwacja sali" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="BookRoom.aspx.cs" 
    Inherits="ConferenceRoomBookingSystem.Pages.BookRoom" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Styles/BookRoom.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="bookroom-container">
        <div class="bookroom-header">
            <h2 class="bookroom-title">Rezerwacja sali konferencyjnej</h2>
        </div>
    
        <asp:Label ID="lblMessage" runat="server" CssClass="alert" Visible="false"></asp:Label>
    </div>
</asp:Content>