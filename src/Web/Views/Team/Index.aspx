<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
	<%=Html.Script("Team.js") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div>
        <div id="search">
        </div>
        <table id="grid" class="scroll" cellpadding="0" cellspacing="0">
        </table>
        <div id="pager" class="scroll" style="text-align: center;">
        </div>
    </div>

</asp:Content>