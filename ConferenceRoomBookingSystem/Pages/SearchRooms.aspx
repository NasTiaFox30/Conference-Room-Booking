<%@ Page Title="Wyszukiwanie sal" Language="C#" MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" CodeBehind="SearchRooms.aspx.cs" 
    Inherits="ConferenceRoomBookingSystem.Pages.SearchRooms" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Styles/SearchRooms.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="search-rooms-container">
        <div class="search-rooms-header">
            <h2 class="search-rooms-title">Wyszukiwanie sal konferencyjnych</h2>
        </div>
        
        <div class="search-form">
            <div class="search-filters-grid">
                <div class="search-filter-group form-group">
                    <label class="form-label">Data:</label>
                    <asp:TextBox ID="txtDate" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="search-filter-group form-group">
                    <label class="form-label">Godzina rozpoczęcia:</label>
                    <asp:TextBox ID="txtStartTime" runat="server" TextMode="Time" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="search-filter-group form-group">
                    <label class="form-label">Godzina zakończenia:</label>
                    <asp:TextBox ID="txtEndTime" runat="server" TextMode="Time" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="search-filter-group form-group">
                    <label class="form-label">Pojemność (minimum):</label>
                    <asp:TextBox ID="txtCapacity" runat="server" TextMode="Number" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            
            <div class="equipment-section">
                <span class="equipment-label form-label">Wyposażenie:</span>
                <div class="equipment-checkboxes">
                    <div class="equipment-checkbox">
                        <asp:CheckBox ID="chkProjector" runat="server" Text="Projektor" />
                    </div>
                    <div class="equipment-checkbox">
                        <asp:CheckBox ID="chkWhiteboard" runat="server" Text="Tablica" />
                    </div>
                    <div class="equipment-checkbox">
                        <asp:CheckBox ID="chkAudio" runat="server" Text="System audio" />
                    </div>
                    <div class="equipment-checkbox">
                        <asp:CheckBox ID="chkWifi" runat="server" Text="Wi‑Fi" />
                    </div>
                </div>
            </div>
            
            <asp:Button ID="btnSearch" runat="server" Text="Wyszukaj" 
                OnClick="btnSearch_Click" CssClass="search-button btn btn-primary" />
        </div>

        
        <asp:Label ID="lblNoRooms" runat="server" Text="Brak dostępnych sal spełniających podane kryteria." 
            Visible="false" CssClass="no-rooms-message"></asp:Label>
    </div>
</asp:Content>