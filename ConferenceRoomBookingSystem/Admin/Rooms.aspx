<%@ Page Title="Manage Rooms" Language="C#" MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" CodeBehind="Rooms.aspx.cs" 
    Inherits="ConferenceRoomBookingSystem.Admin.Rooms" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Styles/Admin.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="admin-container">
        <div class="admin-header">
            <h2 class="admin-title">Zarządzanie salami konferencyjnymi</h2>
            <asp:Button ID="btnAddRoom" runat="server" Text="Dodaj nową salę" 
                OnClick="btnAddRoom_Click" CssClass="btn btn-success" />
        </div>

        <!-- Message block -->
        <asp:Label ID="lblMessage" runat="server" CssClass="alert" Visible="false"></asp:Label>

        <!-- Add/Edit Room Form -->
        <asp:Panel ID="pnlRoomForm" runat="server" Visible="false" CssClass="room-form-panel">
            <div class="form-container">
                <h3><asp:Label ID="lblFormTitle" runat="server" /></h3>
                
                <div class="form-grid">
                    <div class="form-group">
                        <label class="form-label">Nazwa sali *</label>
                        <asp:TextBox ID="txtRoomName" runat="server" CssClass="form-control" 
                            placeholder="Wprowadź nazwę sali" MaxLength="100" />
                        <asp:RequiredFieldValidator ID="rfvRoomName" runat="server" 
                            ControlToValidate="txtRoomName" ErrorMessage="Nazwa sali jest wymagana" 
                            CssClass="validation-error" Display="Dynamic" />
                    </div>

                    <div class="form-group">
                        <label class="form-label">Pojemność *</label>
                        <asp:TextBox ID="txtCapacity" runat="server" TextMode="Number" 
                            CssClass="form-control" Min="1" Max="500" />
                        <asp:RequiredFieldValidator ID="rfvCapacity" runat="server" 
                            ControlToValidate="txtCapacity" ErrorMessage="Pojemność jest wymagana" 
                            CssClass="validation-error" Display="Dynamic" />
                        <asp:RangeValidator ID="rvCapacity" runat="server" 
                            ControlToValidate="txtCapacity" Type="Integer" MinimumValue="1" MaximumValue="500"
                            ErrorMessage="Pojemność musi być między 1 a 500" CssClass="validation-error" Display="Dynamic" />
                    </div>

                    <div class="form-group">
                        <label class="form-label">Lokalizacja</label>
                        <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control" 
                            placeholder="Np. 1 piętro, pok. 101" MaxLength="200" />
                    </div>

                    <div class="form-group full-width">
                        <label class="form-label">Opis</label>
                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" 
                            Rows="3" CssClass="form-control" placeholder="Opis sali" MaxLength="500" />
                    </div>
                </div>

                <div class="equipment-section">
                    <label class="form-label">Wyposażenie:</label>
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
                            <asp:CheckBox ID="chkWifi" runat="server" Text="Wi-Fi" />
                        </div>
                    </div>
                </div>

                <div class="form-actions">
                    <asp:Button ID="btnSave" runat="server" Text="Zapisz" 
                        OnClick="btnSave_Click" CssClass="btn btn-primary" />
                    <asp:Button ID="btnCancel" runat="server" Text="Anuluj" 
                        OnClick="btnCancel_Click" CssClass="btn btn-secondary" CausesValidation="false" />
                </div>
            </div>
        </asp:Panel>

        <!-- Rooms Grid -->
        <asp:GridView ID="gvRooms" runat="server" AutoGenerateColumns="false" 
            CssClass="admin-table" OnRowCommand="gvRooms_RowCommand"
            OnRowDataBound="gvRooms_RowDataBound" DataKeyNames="RoomId">
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
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <span class='status-badge status-<%# Eval("IsActive") %>'>
                            <%# (bool)Eval("IsActive") ? "Aktywna" : "Nieaktywna" %>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Akcje">
                    <ItemTemplate>
                        <div class="action-buttons">
                            <asp:Button ID="btnEdit" runat="server" Text="Edytuj" 
                                CommandName="EditRoom" CommandArgument='<%# Eval("RoomId") %>'
                                CssClass="btn btn-warning btn-sm" />
                            <asp:Button ID="btnDelete" runat="server" Text="Usuń" 
                                CommandName="DeleteRoom" CommandArgument='<%# Eval("RoomId") %>'
                                CssClass="btn btn-danger btn-sm" 
                                OnClientClick='<%# GetDeleteConfirmation(Eval("RoomName")) %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

    </div>
</asp:Content>