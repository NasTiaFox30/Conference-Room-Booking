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