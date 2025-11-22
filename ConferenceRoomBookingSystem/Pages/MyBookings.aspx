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

        <!-- Message block -->
        <asp:Label ID="lblMessage" runat="server" CssClass="alert" Visible="false"></asp:Label>

        <!-- Desktop Table -->
        <asp:GridView ID="gvMyBookings" runat="server" AutoGenerateColumns="false" 
            CssClass="bookings-table desktop-view" OnRowCommand="gvMyBookings_RowCommand"
            OnRowDataBound="gvMyBookings_RowDataBound">
            <Columns>
                <asp:BoundField DataField="RoomName" HeaderText="Sala" />
                <asp:BoundField DataField="Title" HeaderText="Tytuł wydarzenia" />
                <asp:BoundField DataField="StartTime" HeaderText="Początek" DataFormatString="{0:dd.MM.yyyy HH:mm}" />
                <asp:BoundField DataField="EndTime" HeaderText="Koniec" DataFormatString="{0:dd.MM.yyyy HH:mm}" />
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <span class='status-badge status-<%# Eval("Status") %>'>
                            <%# Eval("Status") %>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Czas do rozpoczęcia">
                    <ItemTemplate>
                        <span class='time-badge <%# GetTimeBadgeClass(Eval("StartTime"), Eval("EndTime"), Eval("Status")) %>'>
                            <%# GetTimeBadgeText(Eval("StartTime"), Eval("EndTime"), Eval("Status")) %>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Akcje">
                    <ItemTemplate>
                        <div class="action-buttons">
                            <asp:Button ID="btnCancel" runat="server" Text="Anuluj" 
                                CommandName="CancelBooking" 
                                CommandArgument='<%# Eval("BookingId") %>'
                                CssClass="btn btn-warning btn-sm" 
                                Visible='<%# CanCancelBooking(Eval("Status"), Eval("StartTime")) %>' 
                                OnClientClick='<%# GetCancelConfirmation(Eval("RoomName"), Eval("StartTime")) %>' />
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
                    <div class="booking-card <%# GetUrgentCardClass(Eval("StartTime"), Eval("Status")) %>">
                        <div class="card-header">
                            <h3 class="room-name"><%# Eval("RoomName") %></h3>
                            <div class="header-badges">
                            <span class="status-badge status-<%# Eval("Status") %>"><%# Eval("Status") %></span>
                            </div>
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
                        <div class="card-actions">
                            <asp:Button ID="btnCancelMobile" runat="server" Text="Anuluj" 
                                CommandName="CancelBooking" 
                                CommandArgument='<%# Eval("BookingId") %>'
                                CssClass="btn btn-warning btn-sm" 
                                Visible='<%# CanCancelBooking(Eval("Status"), Eval("StartTime")) %>' 
                                OnClientClick='<%# GetCancelConfirmation(Eval("RoomName"), Eval("StartTime")) %>' />
                            <asp:Button ID="btnDetailsMobile" runat="server" Text="Szczegóły" 
                                CommandName="ViewDetails" 
                                CommandArgument='<%# Eval("BookingId") %>'
                                CssClass="btn btn-info btn-sm" />
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <!-- No bookings message -->
        <asp:Label ID="lblNoBookings" runat="server" Text="Nie masz rezerwacji." 
            CssClass="empty-bookings" Visible="false"></asp:Label>
    </div>
</asp:Content>