<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<BigBallz.ViewModels.BetViewModel>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">- Apostas Encerradas</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: "Apostas de " + Model.UserName%></h2>
    <div id="tabs">
    	<ul>
            <li><a href="#Jogos" title="Jogos"><span>Jogos</span></a></li>
    		<li><a href="#Bonus" title="Bonus"><span>Bonus</span></a></li>
    	</ul>
        <div id="Jogos">
            <%Html.RenderPartial("_BetExpired");%>
        </div>
        <div id="Bonus">
            <%Html.RenderPartial("_DisplayBonusExpired");%>
        </div>
    </div>
    <%:Html.ActionLink("voltar", "index", "standings") %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server"></asp:Content>