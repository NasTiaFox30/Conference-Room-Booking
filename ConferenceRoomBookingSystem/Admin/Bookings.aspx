<%@ Page Title="Manage Bookings" Language="C#" MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" CodeBehind="Bookings.aspx.cs" 
    Inherits="ConferenceRoomBookingSystem.Admin.Bookings" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Styles/Admin.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="admin-container">
        <div class="admin-header">
            <h2 class="admin-title">Zarządzanie rezerwacjami</h2>
        </div>

        <!-- Message block -->
        <asp:Label ID="lblMessage" runat="server" CssClass="alert" Visible="false"></asp:Label>

        <!-- Filters -->
        <div class="filters-section">
            <div class="filter-group">
                <label class="form-label">Status:</label>
                <asp:DropDownList ID="ddlStatusFilter" runat="server" AutoPostBack="true" 
                    OnSelectedIndexChanged="ddlStatusFilter_SelectedIndexChanged" CssClass="form-control">
                    <asp:ListItem Text="Wszystkie" Value="All" Selected="True" />
                    <asp:ListItem Text="Potwierdzone" Value="Confirmed" />
                    <asp:ListItem Text="Anulowane" Value="Cancelled" />
                </asp:DropDownList>
            </div>
            <div class="filter-group">
                <label class="form-label">Data od:</label>
                <asp:TextBox ID="txtDateFrom" runat="server" TextMode="Date" CssClass="form-control" />
            </div>
            <div class="filter-group">
                <label class="form-label">Data do:</label>
                <asp:TextBox ID="txtDateTo" runat="server" TextMode="Date" CssClass="form-control" />
            </div>
            <div class="filter-group">
                <asp:Button ID="btnApplyFilters" runat="server" Text="Filtruj" 
                    OnClick="btnApplyFilters_Click" CssClass="btn btn-primary" />
                <asp:Button ID="btnClearFilters" runat="server" Text="Wyczyść" 
                    OnClick="btnClearFilters_Click" CssClass="btn btn-secondary" />
            </div>
        </div>

        <!-- Bookings Grid -->
        <asp:GridView ID="gvBookings" runat="server" AutoGenerateColumns="false" 
            CssClass="admin-table" OnRowCommand="gvBookings_RowCommand"
            DataKeyNames="BookingId">
            <Columns>
                <asp:BoundField DataField="RoomName" HeaderText="Sala" />
                <asp:BoundField DataField="UserName" HeaderText="Użytkownik" />
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
                <asp:TemplateField HeaderText="Akcje">
                    <ItemTemplate>
                        <div class="action-buttons">
                            <asp:Button ID="btnConfirm" runat="server" Text="Potwierdź" 
                                CommandName="ConfirmBooking" CommandArgument='<%# Eval("BookingId") %>'
                                CssClass="btn btn-success btn-sm" 
                                Visible='<%# Eval("Status").ToString() != "Confirmed" %>' />
                            <asp:Button ID="btnCancel" runat="server" Text="Anuluj" 
                                CommandName="CancelBooking" CommandArgument='<%# Eval("BookingId") %>'
                                CssClass="btn btn-danger btn-sm" 
                                Visible='<%# Eval("Status").ToString() != "Cancelled" %>' 
                                OnClientClick='<%# GetCancelConfirmation(Eval("RoomName"), Eval("StartTime")) %>' />
                            <asp:Button ID="btnDetails" runat="server" Text="Szczegóły" 
                                CommandName="ViewDetails" CommandArgument='<%# Eval("BookingId") %>'
                                CssClass="btn btn-info btn-sm" />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

    </div>
</asp:Content>