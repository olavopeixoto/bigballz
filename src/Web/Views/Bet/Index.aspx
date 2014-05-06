<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<BigBallz.ViewModels.BetViewModel>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">- Apostas</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="tabs">
    	<ul>
            <li><a href="#Jogos" title="Jogos"><span>Jogos</span></a></li>
    		<li><a href="#Bonus" title="Bonus"><span>Bonus</span></a></li>
    	</ul>        
        <div id="Jogos">
            <%Html.RenderPartial("_BetIndex");%>
        </div>
        <div id="Bonus">
            <%if (Model.BonusEnabled) {%>
                <%Html.RenderPartial("_BonusIndex");%>
            <%} else {%>
                <%Html.RenderPartial("_DisplayBonusIndex");%>    
            <%}%>
        </div>
    </div>    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <%=Html.Script("Bet.js")%>
</asp:Content>