<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BigBallz.ViewModels.BetViewModel>" %>
<%@ Import Namespace="BigBallz.Core" %>
<%@ Import Namespace="System.Threading" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Newtonsoft.Json" %>
<%Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");%>
<% using (Html.BeginForm("savebet", "bet")) {%>
<%var i = 0; var lineIndex = 0;%>
<%foreach (var date in Model.BetList.GroupBy(x => new DateTime(x.Match.StartTime.Year, x.Match.StartTime.Month, x.Match.StartTime.Day)).Select(x => x.Key)){%>
    <table class="match-table" summary="<%=date.FormatDate()%>"
        <%=lineIndex==0 ? " data-intro=\"Cadastre aqui os seus palpites e fique ligado no prazo informado. Você tem até uma hora antes do início de cada partida para cadastrar ou alterar o seu palpite.\" data-position=\"top\"" : "" %>>
        <thead class="ui-widget-header">
        <tr>
            <%
                var matchBets = Model.BetList.Where(x => x.Match.StartTime.Year == date.Year && x.Match.StartTime.DayOfYear == date.DayOfYear).ToList();
                var enabled = matchBets.Any(b => b.Enabled);
                var totalPoints = matchBets.Sum(x => x.PointsEarned); %>
            <th colspan="<%=!enabled ? "6" : "7"  %>" class="l"><%=date.ToLongDateString()%></th>
            <% if (!enabled) { %>
            <th class="r"><%:totalPoints%> ponto<%: totalPoints == 1 ? "" : "s" %></th>
            <% } %>
        </tr>
        </thead>
        <tbody>
    <%foreach (var matchBet in matchBets){%>
    <% 
          var totalBets = matchBet.Match.Bets.Count();
          var scoreMostBet = matchBet.Match.Bets.GroupBy(b => new { b.Score1, b.Score2 }).Select(b => new { b.Key, Total = b.Count() }).OrderByDescending(x => x.Total).FirstOrDefault().NullSafe(x => x.Key);
    %>
    <tr class="<%= lineIndex%2==0 ? "ui-state-default": "odd"%>">
        <td class="c dt"><%=Html.Hidden("bets[{0}].BetId".FormatWith(i), matchBet.Bet.NullSafe(x => (int?)x.BetId)).Conditional(matchBet.Enabled)%><%=Html.Hidden("bets[{0}].Match".FormatWith(i), matchBet.Match.MatchId).Conditional(matchBet.Enabled)%><%= Html.Encode(matchBet.Match.StartTime.ToString("HH:mm"))%></td>
        <td class="r homeTeam"><%: matchBet.Match.Team1.Name%>
            <% if (!matchBet.Enabled) {%>
            <span class="betpc"><%:((double)matchBet.Match.Bets.Count(x => x.Score1 > x.Score2) / totalBets).ToPercent()%></span>       
            <%}%>
        </td>
        <td class="c"><%= Html.TeamFlag(matchBet.Match.Team1Id)%></td>
        <td class="c mResult"><%=Html.TextBox("bets[{0}].Score1".FormatWith(i), matchBet.Bet.NullSafe(x => x.Score1), new { @class = "numbersonly bet-score-value score1", maxlength = "2", size = "2" }).Conditional(matchBet.Enabled, Html.TextBox("foo", matchBet.Bet.NullSafe(x => x.Score1), new { disabled = "disabled" }))%> X <%= Html.TextBox("bets[{0}].Score2".FormatWith(i), matchBet.Bet.NullSafe(x => (int?)x.Score2), new { @class = "numbersonly bet-score-value score2", maxlength = "2", size = "2" }).Conditional(matchBet.Enabled, Html.TextBox("foo", matchBet.Bet.NullSafe(x => x.Score2), new {disabled="disabled"}))%>
            <%if (matchBet.Match.Score1.HasValue) {%><div class="mResultSub"><%=matchBet.Match.Score1%> X <%=matchBet.Match.Score2%></div><%}
            else if (!matchBet.Enabled && !matchBet.Match.Score1.HasValue && scoreMostBet != null){%>
            <div class="mResultSubBet"><%=scoreMostBet.Score1%> X <%=scoreMostBet.Score2%></div>
            <%}%>
        </td>
        <td class="c"><%= Html.TeamFlag(matchBet.Match.Team2Id)%></td>
        <td class="l awayTeam"><%: matchBet.Match.Team2.Name%>
            <% if (!matchBet.Enabled) {%>
            <span class="betpc"><%:((double)matchBet.Match.Bets.Count(x => x.Score2 > x.Score1) / totalBets).ToPercent()%></span>       
            <%}%>
        </td>
        <td class="l reminder" data-expirationdate=<%=JsonConvert.SerializeObject(matchBet.Match.StartTime.AddHours(2)) %>><%= Html.MatchReminder(matchBet.Match.MatchId, matchBet.Match.StartTime, matchBet.PointsEarned)%></td>        
    </tr>
    <%if (matchBet.Enabled) i++; lineIndex++; } %>
    </tbody>
    </table>
<%} %>
<div class="always-on-bottom">
<div id="apostar" class="ui-widget ui-helper-hidden">
	<div class="ui-state-highlight ui-corner-all" style="padding: 0 .7em;"> 
		<p><span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span> 
		<input type="submit" value="Apostar" /></p>
	</div>
</div>
</div>
<input type="submit" value="Apostar" />
<%} %>