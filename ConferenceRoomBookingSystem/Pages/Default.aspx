<%@ Page Language="C#" 
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" MasterPageFile="~/Site.Master"
    Inherits="ConferenceRoomBookingSystem.Pages.Default" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Styles/Default.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="default-container">
        <div class="default-header">
            <h1 class="default-title">Witamy w systemie rezerwacji sal</h1>
            <p class="default-subtitle">
                Łatwo i wygodnie rezerwuj sale konferencyjne na Twoje spotkania i wydarzenia
            </p>
        </div>
        
        <div class="default-button-container">
            <asp:Button ID="btnSearchRooms" runat="server" Text="Wyszukaj sale" 
                PostBackUrl="~/Pages/SearchRooms.aspx" CssClass="default-btn btn btn-primary" />
            <asp:Button ID="btnMyBookings" runat="server" Text="Moje rezerwacje" 
                PostBackUrl="~/Pages/MyBookings.aspx" CssClass="default-btn btn btn-secondary" />
        </div>

        <div class="default-features-grid">
            <div class="default-feature-card">
                <h3 class="default-feature-title">Proste wyszukiwanie</h3>
                <p class="default-feature-description">Znajduj wolne sale według daty, godziny i potrzebnego wyposażenia</p>
            </div>
            <div class="default-feature-card">
                <h3 class="default-feature-title">Rezerwacja online</h3>
                <p class="default-feature-description">Natychmiastowa rezerwacja bez zbędnych formalności</p>
            </div>
            <div class="default-feature-card">
                <h3 class="default-feature-title">Powiadomienia</h3>
                <p class="default-feature-description">Przypomnienia o nadchodzących spotkaniach i potwierdzenia rezerwacji</p>
            </div>
        </div>
    </div>
</asp:Content>