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

         <!-- Message block -->
        <asp:Label ID="lblMessage" runat="server" CssClass="alert" Visible="false"></asp:Label>
       
        <div class="bookroom-content">
            <!-- Szczegóły sali -->
            <div>
                <asp:FormView ID="fvRoomDetails" runat="server">
                    <ItemTemplate>
                        <div class="room-details-card">
                            <h3 class="room-details-title"><%# Eval("RoomName") %></h3>
                            <div class="room-details-info">
                                <div class="room-details-item">
                                    <span class="room-details-label">Pojemność:</span>
                                    <span class="room-details-value"><%# Eval("Capacity") %> osób</span>
                                </div>
                                <div class="room-details-item">
                                    <span class="room-details-label">Lokalizacja:</span>
                                    <span class="room-details-value"><%# Eval("Location") %></span>
                                </div>
                                <div class="room-details-item">
                                    <span class="room-details-label">Opis:</span>
                                    <span class="room-details-value"><%# Eval("Description") %></span>
                                </div>
                            </div>
                            <div class="room-equipment">
                                <span class="room-equipment-label">Wyposażenie:</span>
                                <div class="equipment-badges">
                                    <%# GetEquipmentText(Eval("HasProjector"), Eval("HasWhiteboard"), Eval("HasAudioSystem"), Eval("HasWiFi")) %>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:FormView>
            </div>

            <!-- Formularz rezerwacji -->
            <div>
                <div class="bookroom-form">
                    <div class="form-group">
                        <label class="form-label">Nazwa wydarzenia:</label>
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control"
                            placeholder="Wprowadź nazwę wydarzenia" required="true"></asp:TextBox>
                    </div>
                   
                    <div class="form-group">
                        <label class="form-label">Opis (opcjonalnie):</label>
                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine"
                            Rows="3" CssClass="form-control" placeholder="Opisz szczegóły wydarzenia"></asp:TextBox>
                    </div>
                   
                    <div class="form-group">
                        <label class="form-label">Data:</label>
                        <asp:TextBox ID="txtBookingDate" runat="server" TextMode="Date"
                            CssClass="form-control" required="true"></asp:TextBox>
                    </div>
                   
                    <div class="time-grid">
                        <div class="form-group">
                            <label class="form-label">Godzina rozpoczęcia:</label>
                            <asp:TextBox ID="txtBookingStart" runat="server" TextMode="Time"
                                CssClass="form-control" required="true"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label class="form-label">Godzina zakończenia:</label>
                            <asp:TextBox ID="txtBookingEnd" runat="server" TextMode="Time"
                                CssClass="form-control" required="true"></asp:TextBox>
                        </div>
                    </div>
                   
                    <div class="form-group">
                        <label class="form-label">Uczestnicy (e-mail przez przecinek):</label>
                        <asp:TextBox ID="txtAttendees" runat="server" TextMode="MultiLine"
                            Rows="2" CssClass="form-control"
                            placeholder="user1@firma.pl, user2@firma.pl"></asp:TextBox>
                    </div>
                   
                    <div class="bookroom-buttons">
                        <asp:Button ID="btnConfirm" runat="server" Text="Potwierdź rezerwację"
                            OnClick="btnConfirm_Click" CssClass="bookroom-btn btn btn-primary" />
                        <asp:Button ID="btnCancel" runat="server" Text="Anuluj"
                            OnClick="btnCancel_Click" CssClass="bookroom-btn btn btn-secondary" CausesValidation="false" />
                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>