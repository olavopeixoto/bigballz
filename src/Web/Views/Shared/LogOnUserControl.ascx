<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
    if (Request.IsAuthenticated) {
%>
        <b><%= Html.Encode(Page.User.Identity.Name) %></b> | <a href="#" onclick="logoff()">Sair</a>
<%
    }
    else if (TempData["UserDetails"]==null)
    {
        var returnUrl = Url.Encode(Url.Action("handleresponse", "auth", null, FormsAuthentication.RequireSSL ? "https" : "http"));
%> 
        Já está registrado? <a class="rpxnow" onclick="return false;" href="https://bigballz.rpxnow.com/openid/v2/signin?token_url=<%=returnUrl%>">Entre aqui</a>
<%
    }
%>