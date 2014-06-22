<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<BigBallz.ViewModels.BetViewModel>" %>
<%@ Import Namespace="BigBallz.Core.Extension.Web.Mvc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">- Apostas</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="tabs">
    	<ul>
            <li><a href="#Jogos" title="Jogos"><span>Jogos (<%:Model.BetList.Sum(b => b.PointsEarned) %>)</span></a></li>
    		<li><a href="#Bonus" title="Bonus"><span>Bonus (<%:Model.BonusList.Sum(b => b.PointsEarned) %>)</span></a></li>
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
    <script src="//ajax.aspnetcdn.com/ajax/knockout/knockout-3.1.0.js" type="text/javascript"></script>
    <%=Html.RegisterScriptAndDumpRegisteredScripts("Bet.js")%>
    
    <%if(Model.ShowHelp) {%>
    <%=Html.Script("chardinjs.min.js")%>
    <script type="text/javascript">
        $('body').chardinJs('start');
    </script>
    <%}%>
</asp:Content>