<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">, o Bolão da Copa do Mundo</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
	<%=Html.Script("Home.js") %>
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
   <%Html.RenderPartial("_Index"); %>
</asp:Content>