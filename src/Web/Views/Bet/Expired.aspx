<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<BigBallz.ViewModels.BetViewModel>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">- Apostas Encerradas</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% var totalPoints = Model.BetList.Sum(x => x.PointsEarned) + Model.BonusList.Sum(x => x.PointsEarned); %>
    <% var standings = (IList<BigBallz.Models.UserPoints>)ViewData["Standings"];%>
    <h2><%: "Apostas de " +  Model.UserName%> - <%: standings.First(x=>x.User.UserName==Model.UserName).Position%>º - <%: totalPoints%> ponto<%:totalPoints==1 ? "" : "s" %></h2>
    <div id="tabs">
    	<ul>
            <li><a href="#Jogos" title="Jogos"><span>Jogos (<%: Model.BetList.Sum(x => x.PointsEarned) %>)</span></a></li>
    		<li><a href="#Bonus" title="Bonus"><span>Bonus (<%: Model.BonusList.Sum(x => x.PointsEarned) %>)</span></a></li>
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