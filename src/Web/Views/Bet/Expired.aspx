<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<BigBallz.ViewModels.BetViewModel>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent">- Apostas Encerradas</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="MainContent">
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
<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="Scripts"></asp:Content>