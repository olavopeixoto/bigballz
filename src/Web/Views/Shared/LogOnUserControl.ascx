<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
    if (Request.IsAuthenticated) {
%>
        <b><%= Html.Encode(Page.User.Identity.Name) %></b> | <%= Html.ActionLink("Sair", "LogOff", "Account") %>
<%
    }
    else {
        var host = Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
%> 
        Já está registrado? <a class="rpxnow" onclick="return false;" href="https://bigballz.rpxnow.com/openid/v2/signin?token_url=http<%=FormsAuthentication.RequireSSL ? "s" : ""%>%3A%2F%2F<%=host %>%2Frpx">Entre aqui</a>
<%
    }
%>