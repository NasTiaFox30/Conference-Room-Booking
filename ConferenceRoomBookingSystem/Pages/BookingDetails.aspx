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
    
        <asp:FormView ID="fvBookingDetails" runat="server">
            <ItemTemplate>
                <div class="booking-details-card">
                    <h3><%# Eval("Title") %></h3>
                    
                    <div class="booking-details-grid">
                        <div>
                            <strong>Sala:</strong>
                            <%# Eval("RoomName") %>
                        </div>
                        <div>
                            <strong>Status:</strong>
                            <span class='status-<%# Eval("Status") %>'><%# Eval("Status") %></span>
                        </div>
                        <div>
                            <strong>Data:</strong>
                            <%# Convert.ToDateTime(Eval("StartTime")).ToString("dd.MM.yyyy") %>
                        </div>
                        <div>
                            <strong>Czas:</strong>
                            <%# Convert.ToDateTime(Eval("StartTime")).ToString("HH:mm") %> - <%# Convert.ToDateTime(Eval("EndTime")).ToString("HH:mm") %>
                        </div>
                    </div>
                    
                    <div class="booking-section">
                        <strong>Opis:</strong>
                        <%# Eval("Description") %>
                    </div>
                    
                    <div class="booking-section">
                        <strong>Uczestnicy:</strong>
                        <%# Eval("Attendees") %>
                    </div>
                    
                    <div class="booking-created-section">
                        <strong>Zarezerwowane przez:</strong>
                        <%# Eval("UserName") %> (<%# Convert.ToDateTime(Eval("CreatedDate")).ToString("dd.MM.yyyy HH:mm") %>)
                    </div>
                </div>
            </ItemTemplate>
        </asp:FormView>
    </div>
</asp:Content>