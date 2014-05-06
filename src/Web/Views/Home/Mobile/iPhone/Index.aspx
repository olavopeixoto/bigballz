<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Mobile/iPhone/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
	<%=Html.Script("Home.js") %>
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
   <%Html.RenderPartial("_Index"); %>
</asp:Content>