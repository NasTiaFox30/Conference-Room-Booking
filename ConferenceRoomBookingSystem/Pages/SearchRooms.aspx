<%@ Page Title="Wyszukiwanie sal" Language="C#" MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" CodeBehind="SearchRooms.aspx.cs" 
    Inherits="ConferenceRoomBookingSystem.Pages.SearchRooms" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Styles/SearchRooms.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="search-rooms-container">
    </div>
</asp:Content>