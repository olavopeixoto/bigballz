<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BigBallz.ViewModels.BetViewModel>" %>
<%@ Import Namespace="BigBallz.Core" %>
<table class="match-table">
<thead class="ui-widget-header">
    <tr>
        <th>Bonus</th>
        <th>Aposta</th>
        <th>Resultado</th>
        <th>Pontos</th>
        <th>Mais Apostado</th>
    </tr>
</thead>
<tbody>
<%var index = 0; var lineIndex = 0; %>
<%foreach (var bonus in Model.BonusList){%>
    <tr class="<%= lineIndex%2==0 ? "ui-state-default": "odd"%>">
        <td class="l"><%:bonus.Bonus.Name%></td>
        <td class="l"><%:bonus.BonusBet.NullSafe(y => y.Team.NullSafe(x => x.Name))%></td>
        <td class="l"><%:bonus.Bonus.Team.NullSafe(x => x.Name)%></td>
        <td class="c"><%if (string.IsNullOrEmpty(bonus.Bonus.TeamId)) {%>
            -
        <%} else {%>
            <%:bonus.PointsEarned%> ponto<%:bonus.PointsEarned == 1 ? "" : "s"%>
        <%}%></td>
       <td class="l"><%:bonus.BonusBetStatistic.Team.NullSafe(x => x.Name)%> (<%:bonus.BonusBetStatistic.TeamPerc.ToPercent()%>)</td>
    </tr>
<%index++; lineIndex++;} %>
    <%var totalBonus = Model.BonusList.Sum(x => x.PointsEarned);%>
    <tr class="ui-widget-header">
        <td colspan="3" class="l">Total</td>
        <td colspan="2"><%:totalBonus%> ponto<%:totalBonus == 1 ? "" : "s"%></td>
    </tr>
</tbody>
</table>