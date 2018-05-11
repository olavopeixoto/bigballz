<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<BigBallz.Models.User>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent"> - Novo Registro</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="MainContent">
<script type="text/javascript">
    (function () {
        if (typeof window.janrain !== 'object') window.janrain = {};
        if (typeof window.janrain.settings !== 'object') window.janrain.settings = {};

        janrain.settings.tokenUrl = '<%=Url.Action("handleresponse", "auth", null, FormsAuthentication.RequireSSL ? "https" : "http") %>';
        janrain.settings.showAttribution = false;
        janrain.settings.language = "pt-BR";

        function isReady() { janrain.ready = true; };
        if (document.addEventListener) {
            document.addEventListener("DOMContentLoaded", isReady, false);
        } else {
            window.attachEvent('onload', isReady);
        }

        var e = document.createElement('script');
        e.type = 'text/javascript';
        e.id = 'janrainAuthWidget';

        if (document.location.protocol === 'https:') {
            e.src = 'https://rpxnow.com/js/lib/bigballz/engage.js';
        } else {
            e.src = 'http://widget-cdn.rpxnow.com/js/lib/bigballz/engage.js';
        }

        var s = document.getElementsByTagName('script')[0];
        s.parentNode.insertBefore(e, s);
    })();
</script>

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

    <a class="janrainEngage" href="#">Associar a uma conta existente</a>

    <p><small>* ao registrar a sua conta você está de acordo com o nosso <%=Html.ActionLink("regulamento", "rules", "home") %></small></p>
</fieldset>
 <% } %>
</asp:Content>
<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="Scripts"></asp:Content>