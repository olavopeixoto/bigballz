<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<BigBallz.Models.User>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent"> - Novo Registro</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="MainContent">
<h2>Novo Registro</h2>
<% using (Html.BeginForm()) {%>
<fieldset>
    <legend>Confirme os seus dados:</legend>
    <%if (!string.IsNullOrEmpty(Model.PhotoUrl)) {%>
    <img src="<%=Model.PhotoUrl%>" alt="<%=Model.UserName%>" />
    <%}%>

    <div class="editor-label">
        <label for="Date">Apelido:</label>
    </div>
    <div class="editor-field">
        <%= Html.EditorFor(model => model.UserName)%>
        <%= Html.ValidationMessageFor(model => model.UserName)%>
    </div>

    <div class="editor-label">
        <label for="Date">E-Mail:</label>
    </div>
    <div class="editor-field">
        <%=Html.EditorFor(u => u.EmailAddress) %>
        <%= Html.ValidationMessageFor(model => model.EmailAddress)%>
    </div>
    
    <input type="submit" value="Registrar" />
<%
    var returnUrl = Url.Encode(Url.Action("handleresponse", "auth", null, FormsAuthentication.RequireSSL ? "https" : "http"));
%> 
    <a class="rpxnow" onclick="return false;" href="https://bigballz.rpxnow.com/openid/v2/signin?token_url=<%=returnUrl%>">Associar a uma conta existente</a>
</fieldset>
 <% } %>
</asp:Content>
<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="Scripts"></asp:Content>