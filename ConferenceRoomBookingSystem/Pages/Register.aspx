<%@ Page Title="Rejestracja" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Register.aspx.cs"
    Inherits="ConferenceRoomBookingSystem.Pages.Register" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Styles/Register.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="auth-container">
        <div class="auth-header">
            <h2 class="auth-title">Rejestracja nowego użytkownika</h2>
        </div>
    
        <div class="auth-form">
            <div class="form-group">
                <label class="form-label">Login:</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername"
                    ErrorMessage="Pole wymagane" CssClass="validator-error" Display="Dynamic" />
            </div>
        </div>
    </div>
</asp:Content>