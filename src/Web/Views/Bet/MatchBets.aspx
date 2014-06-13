<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<BigBallz.ViewModels.MatchBetsViewModel>" MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Import Namespace="BigBallz.Core" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent"></asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="MainContent">
<p>
<table class="match-table">
<thead class="ui-widget-header">
    <tr>
        <th class="l" colspan="2">Estat�sticas das Apostas</th>
    </tr>
</thead>
<tbody>
<tr>
    <th>% Vitoria <%:Model.Match.Team1Id.Name%></th>
    <td><%: Model.Statistics.Team1Perc.ToPercent()%></td>
</tr>
<tr>
    <th>% Vitoria <%:Model.Match.Team2Id.Name%></th>
    <td><%: Model.Statistics.Team2Perc.ToPercent()%></td>
</tr>
<tr>
    <th>% Empate</th>
    <td><%: Model.Statistics.TiePerc.ToPercent()%></td>
</tr>
<tr>
    <th>M�dia de Gols <%:Model.Match.Team1Id.Name%></th>
    <td><%: Model.Statistics.AverageScore1.ToString("N2")%></td>
</tr>
<tr>
    <th>M�dia de Gols <%:Model.Match.Team2Id.Name%></th>
    <td><%: Model.Statistics.AverageScore2.ToString("N2")%></td>
</tr>
<tr>
    <th>Placar Mais Apostado</th>
    <td><%:Model.Match.Team1Id%>&nbsp;<%: Model.Statistics.Score1MostBet%>&nbsp;X&nbsp;<%: Model.Statistics.Score2MostBet%>&nbsp;<%:Model.Match.Team2Id%></td>
</tr>
</tbody>
</table>
<table class="match-table">
<thead class="ui-widget-header">
    <tr>
        <th colspan="2">Jogador</th>
        <th colspan="5">Aposta</th>
        <th>Pontos</th>
    </tr>
</thead>
<tbody>
<%var lineIndex = 0;%>
<%foreach (var betPoints in Model.UsersMatchPoints) {%>
    <tr class="<%= lineIndex%2==0 ? "ui-state-default": "odd"%>">
        <td class="c" style="width:50px;height:50px;"><%=Html.GetUserPhoto(betPoints.Bet.User1)%></td>
        <td class="l"><%: betPoints.Bet.User1.UserName%></td>
        <td class="r homeTeam"><%: betPoints.Bet.Match1.Team1Id.Name%></td>
        <td class="c"><%= Html.TeamFlag(betPoints.Bet.Match1.Team1Id)%></td>
        <td class="c mResult"><%: betPoints.Bet.NullSafe(x => x.Score1)%> X <%: betPoints.Bet.NullSafe(x => x.Score2)%><%if (betPoints.Bet.Match1.Score1.HasValue) {%><div class="mResultSub"><%=betPoints.Bet.Match1.Score1%> X <%=betPoints.Bet.Match1.Score2%></div><%}%></td>
        <td class="c"><%= Html.TeamFlag(betPoints.Bet.Match1.Team2Id)%></td>
        <td class="l awayTeam"><%: betPoints.Bet.Match1.Team2Id.Name%></td>
        <td class="c reminder"><%: betPoints.Points%> ponto<%: betPoints.Points == 1 ? "" : "s"%></td>
    </tr>    
<%lineIndex++; } %>
</tbody>
</table>
</p>
<%:Html.ActionLink("voltar", "index", "bet")%>
</asp:Content>
<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="Scripts"></asp:Content>