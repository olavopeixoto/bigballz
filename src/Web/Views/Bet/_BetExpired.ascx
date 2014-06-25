<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BigBallz.ViewModels.BetViewModel>" %>
<%@ Import Namespace="System.Threading" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="BigBallz.Core" %>
<%Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR"); %>
<%var i = 0; var lineIndex = 0;%>
<%foreach (var date in Model.BetList.GroupBy(x => new DateTime(x.Match.StartTime.Year, x.Match.StartTime.Month, x.Match.StartTime.Day)).Select(x => x.Key)){%>
    <table class="match-table" summary="<%=date.FormatDate()%>">
        <thead class="ui-widget-header">
        <tr>
            <th colspan="6" class="l"><%=date.ToLongDateString()%></th>
            <% var pontosRodada = Model.BetList.Where(x => x.Match.StartTime.Year == date.Year && x.Match.StartTime.DayOfYear == date.DayOfYear).Sum(x => x.PointsEarned); %>
            <th class="r"><%: pontosRodada%> ponto<%: pontosRodada == 1 ? "" : "s" %></th>
        </tr>
        </thead>
        <tbody>
    <%foreach (var matchBet in Model.BetList.Where(x => x.Match.StartTime.Year == date.Year && x.Match.StartTime.DayOfYear == date.DayOfYear)){%>
    <tr class="<%= lineIndex%2==0 ? "ui-state-default": "odd"%>">
        <td class="c dt"><%: matchBet.Match.StartTime.ToString("HH:mm")%></td>
        <td class="r homeTeam"><%: matchBet.Match.Team1.Name%></td>
        <td class="c"><%= Html.TeamFlag(matchBet.Match.Team1Id)%></td>
        <td class="c mResult"><%: matchBet.Bet.NullSafe(x => (int?)x.Score1)%> X <%: matchBet.Bet.NullSafe(x => (int?)x.Score2)%><%if (matchBet.Match.Score1.HasValue) {%><div class="mResultSub"><%=matchBet.Match.Score1%> X <%=matchBet.Match.Score2%></div><%}%></td>
        <td class="c"><%= Html.TeamFlag(matchBet.Match.Team2Id)%></td>
        <td class="l awayTeam"><%: matchBet.Match.Team2.Name%></td>
        <td class="l reminder"><div class="match-reminder bet-times-up"><%:matchBet.PointsEarned%> ponto<%:matchBet.PointsEarned == 1 ? "" : "s"%></div></td>
    </tr>
    <%i++; lineIndex++; } %>
    </tbody>
    </table>
<%} %>