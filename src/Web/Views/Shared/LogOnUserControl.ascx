<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
    if (Request.IsAuthenticated) {
%>
        <b><%= Html.Encode(Page.User.Identity.Name) %></b> | <%= Html.ActionLink("Sair", "logoff", "account") %>
<%}%>