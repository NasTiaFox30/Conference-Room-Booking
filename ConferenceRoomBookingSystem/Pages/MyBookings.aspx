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
        
        <div class="booking-filter">
            <asp:DropDownList ID="ddlStatusFilter" runat="server" AutoPostBack="true" 
                OnSelectedIndexChanged="ddlStatusFilter_SelectedIndexChanged" CssClass="form-control">
                <asp:ListItem Text="Nadchodzące" Value="Upcoming" Selected="True" />
                <asp:ListItem Text="Minione" Value="Past" />
                <asp:ListItem Text="Wszystkie" Value="All" />
            </asp:DropDownList>
        </div>

        <!-- Desktop Table -->
        <asp:GridView ID="gvMyBookings" runat="server" AutoGenerateColumns="false" 
            CssClass="bookings-table desktop-view" OnRowCommand="gvMyBookings_RowCommand"
            EmptyDataText="Nie masz rezerwacji.">
            <Columns>
                <asp:BoundField DataField="RoomName" HeaderText="Sala" />
                <asp:BoundField DataField="Title" HeaderText="Tytuł wydarzenia" />
                <asp:BoundField DataField="StartTime" HeaderText="Początek" DataFormatString="{0:dd.MM.yyyy HH:mm}" />
                <asp:BoundField DataField="EndTime" HeaderText="Koniec" DataFormatString="{0:dd.MM.yyyy HH:mm}" />
                <asp:BoundField DataField="Status" HeaderText="Status" />
                <asp:TemplateField HeaderText="Akcje">
                    <ItemTemplate>
                        <div class="action-buttons">
                            <asp:Button ID="btnCancel" runat="server" Text="Anuluj" 
                                CommandName="CancelBooking" 
                                CommandArgument='<%# Eval("BookingId") %>'
                                CssClass="btn btn-warning btn-sm" 
                                Visible='<%# CanCancelBooking(Eval("Status"), Eval("StartTime")) %>' />
                            <asp:Button ID="btnDetails" runat="server" Text="Szczegóły" 
                                CommandName="ViewDetails" 
                                CommandArgument='<%# Eval("BookingId") %>'
                                CssClass="btn btn-info btn-sm" />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <!-- Mobile Cards -->
        <div class="mobile-bookings-cards mobile-view">
            <asp:Repeater ID="rptMobileBookings" runat="server" OnItemCommand="rptMobileBookings_ItemCommand">
                <ItemTemplate>
                    <div class="booking-card">
                        <div class="card-header">
                            <h3 class="room-name"><%# Eval("RoomName") %></h3>
                            <span class="status-badge status-<%# Eval("Status") %>"><%# Eval("Status") %></span>
                        </div>
                        <div class="card-body">
                            <div class="booking-info">
                                <div class="info-item">
                                    <span class="info-label">Tytuł:</span>
                                    <span class="info-value"><%# Eval("Title") %></span>
                                </div>
                                <div class="info-item">
                                    <span class="info-label">Początek:</span>
                                    <span class="info-value"><%# FormatDate(Eval("StartTime")) %></span>
                                </div>
                                <div class="info-item">
                                    <span class="info-label">Koniec:</span>
                                    <span class="info-value"><%# FormatDate(Eval("EndTime")) %></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <asp:Label ID="lblNoBookingsMobile" runat="server" Text="Nie masz rezerwacji." 
                CssClass="empty-bookings mobile-view" Visible="false"></asp:Label>
        </div>
    </div>
</asp:Content>