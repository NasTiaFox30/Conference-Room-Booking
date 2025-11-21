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

         <!-- Message block -->
        <asp:Label ID="lblMessage" runat="server" CssClass="alert" Visible="false"></asp:Label>

        <!-- Desktop Table -->
        <asp:GridView ID="gvAvailableRooms" runat="server" AutoGenerateColumns="false" 
            CssClass="rooms-table desktop-view" Visible="false" OnRowCommand="gvAvailableRooms_RowCommand">
            <Columns>
                <asp:BoundField DataField="RoomName" HeaderText="Nazwa sali" />
                <asp:BoundField DataField="Capacity" HeaderText="Pojemność" />
                <asp:BoundField DataField="Location" HeaderText="Lokalizacja" />
                <asp:TemplateField HeaderText="Wyposażenie">
                    <ItemTemplate>
                        <div class="equipment-badges">
                            <%# GetEquipmentBadges(Eval("HasProjector"), Eval("HasWhiteboard"), Eval("HasAudioSystem"), Eval("HasWiFi")) %>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Akcja">
                    <ItemTemplate>
                        <asp:Button ID="btnBook" runat="server" Text="Zarezerwuj" 
                            CommandName="BookRoom" 
                            CommandArgument='<%# Eval("RoomId") %>'
                            CssClass="book-button btn btn-success" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <!-- Mobile Cards -->
        <div class="mobile-rooms-cards mobile-view">
            <asp:Repeater ID="rptMobileRooms" runat="server" OnItemCommand="rptMobileRooms_ItemCommand">
                <ItemTemplate>
                    <div class="room-card">
                        <div class="card-header">
                            <h3 class="room-name"><%# Eval("RoomName") %></h3>
                            <span class="capacity-badge"><%# Eval("Capacity") %> os.</span>
                        </div>
                        <div class="card-body">
                            <div class="room-info">
                                <div class="info-item">
                                    <span class="info-label">Lokalizacja:</span>
                                    <span class="info-value"><%# Eval("Location") %></span>
                                </div>
                                <div class="info-item">
                                    <span class="info-label">Wyposażenie:</span>
                                    <span class="info-value equipment-value">
                                        <%# GetEquipmentText(Eval("HasProjector"), Eval("HasWhiteboard"), Eval("HasAudioSystem"), Eval("HasWiFi")) %>
                                    </span>
                                </div>
                            </div>
                        </div>
                        <div class="card-actions">
                            <asp:Button ID="btnBookMobile" runat="server" Text="Zarezerwuj" 
                                CommandName="BookRoom" 
                                CommandArgument='<%# Eval("RoomId") %>'
                                CssClass="btn btn-success btn-sm" />
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

    </div>
</asp:Content>