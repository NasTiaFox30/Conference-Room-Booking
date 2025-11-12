<%@ Page Title="Logowanie" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Login.aspx.cs"
    Inherits="ConferenceRoomBookingSystem.Pages.Login" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Styles/Login.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="auth-container">
    
    </div>
</asp:Content>