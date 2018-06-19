<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl"%>
<%@ Import Namespace="System.Threading" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="BigBallz.Core" %>
<%@ Import Namespace="BigBallz.Helpers" %>
<%@ Import Namespace="BigBallz.Models" %>

<%var standings = ViewData["Standings"] as IList<UserPoints>;%>

<div id="profile" class="section">
  <div class="user_icon">
    <%var photoCookie = Request.Cookies["photoUrl"];%>
    <%if (photoCookie != null && !string.IsNullOrEmpty(photoCookie.Value)){%>
    <img class="profile-pic" alt="<%=Context.User.Identity.Name%>" src="<%=photoCookie.Value%>" />
    <%}%>
    <span class="stats">
        <span id="me_name"><%=Page.User.Identity.Name%></span>
        <%if (standings != null && Html.IsAuthorized())
          {%>
        <%var currentUserStats = standings.FirstOrDefault(x => x.User.UserName == Page.User.Identity.Name);%>
        <span class="admin">(<%:currentUserStats.NullSafe(x => x.TotalPoints)%> pts - <%:currentUserStats.NullSafe(x => x.Position)%>º lugar)</span>
        <%} if (Page.User.Identity.IsAuthenticated && !Html.IsAuthorized())
          {%>
            <%Html.RenderPartial("PagSeguro"); %>
        <%}%>
        <% if (Context.User.IsInRole(BBRoles.Admin)) { 
                if (Request.Cookies["x-profiler"] == null) { %>
                    <span class="admin"><%= Html.ActionLink("Enable Profiler", "EnableProfiler", "auth") %></span>
            <% } else { %>
                     <span class="admin"><%= Html.ActionLink("Disable Profiler", "DisableProfiler", "auth") %></span>
            <% }
         } %>
    </span>
  </div>
</div>

<%var pendingBets = ViewData["PendingBets"] as IList<BigBallz.Models.Match>;
if (pendingBets!=null && pendingBets.Any())
{
    var pendingBetsCount = pendingBets.Count;
    var plural = pendingBetsCount > 1 ? "s" : string.Empty;
    var minDate = pendingBets.Min(x => x.StartTime);
    %>
    <p><%=Html.ActionLink(string.Format("Você tem {0} aposta{1} pendente{1} para as próximas 24h", pendingBetsCount, plural), "index", "bet", null, null, "match-table-" + minDate.ToString("yyyy-MM-dd"), null, new { @class = "danger"})%></p>
<%}%>

<%var prizes = ViewData["prizes"] as Prizes;%>
<%if (prizes!=null) {%>
<%Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");%>
<hr />
<h2>Prêmio Acumulado</h2>
<ol>
<li><%=prizes.First.ToString("C")%></li>
<li><%=prizes.Second.ToString("C")%></li>
<li><%=prizes.Third.ToString("C")%></li>
</ol>
<%=Html.ActionLink("ver detalhes", "index", "money") %>
<%}%>


<%if (standings!=null && standings.Any(s => s.TotalPoints > 0)) {%>
<hr />
<h2>Top 5 Geral</h2>
    <table class="match-table">
       <tbody>
   <%var lineIndex = 0; var topfiveStandings = standings.Take(5).Union(standings.Where(s => s.User.UserName == Context.User.Identity.Name)).Distinct().ToList();
foreach (var userPoints in topfiveStandings){%>
           <% var classe = "";
       if (userPoints.Position < userPoints.LastPosition) classe = "up";
       else if (userPoints.Position > userPoints.LastPosition) classe = "down";%>
    <tr class="<%= lineIndex%2==0 ? "ui-state-default": "odd"%>">
        <td class="c"><%=userPoints.Position%>º</td>
        <td class="c <%: classe %>" style="white-space: nowrap"><%if (userPoints.Position < userPoints.LastPosition) {%>
          <i class="fa fa-long-arrow-up"></i>
          <%} else if (userPoints.Position > userPoints.LastPosition) {%>
          <i class="fa fa-long-arrow-down"></i>
          <%} else {%>
          <i class="fa fa-minus"></i>
          <%} %>
          <% var gain = Math.Abs(userPoints.LastPosition - userPoints.Position); %>
          <small><%: gain > 0 ? gain.ToString() : "" %></small>
      </td>
        <td class="l namesmall">
            <%= userPoints.User.UserName == Context.User.Identity.Name ? "<strong>" : string.Empty%>
            <%: Html.ActionLink(userPoints.User.UserName, "expired", "bet", new {id=userPoints.User.UserId}, null)%>
            <%= userPoints.User.UserName == Context.User.Identity.Name ? "</strong>" : string.Empty%>
        </td>
        <td class="c"><%: userPoints.TotalPoints%></td>
    </tr>
    <%lineIndex++; } %>
    </tbody>
    </table>
<%} %>

<%var dayStandings = ViewData["DayStandings"] as IList<UserPoints>;%>
<%if (standings != null && dayStandings != null && dayStandings.Any(s => s.TotalDayPoints > 0))
  {%>
<hr />
<h2>Top 5 da Rodada</h2>
<table class="match-table">
       <tbody>
   <%var lineIndex = 0; var topfiveDayStandings = dayStandings.Take(5).Union(dayStandings.Where(s => s.User.UserName == Context.User.Identity.Name)).Distinct().ToList(); %>
<%foreach (var userPoints in topfiveDayStandings) {%>
           <% var classe = "";
              var userPosition = standings.First(x => x.User.UserId == userPoints.User.UserId);
              if (userPosition.Position < userPosition.LastPosition) classe = "up";
              else if (userPosition.Position > userPosition.LastPosition) classe = "down";%>
    <tr class="<%= lineIndex%2==0 ? "ui-state-default": "odd"%>">
        <td class="c"><%=userPoints.Position%>º</td>
        <td class="c <%: classe %>" style="white-space: nowrap"><%if (userPosition.Position < userPosition.LastPosition) {%>
          <i class="fa fa-long-arrow-up"></i>
          <%} else if (userPosition.Position > userPosition.LastPosition) {%>
          <i class="fa fa-long-arrow-down"></i>
          <%} else {%>
          <i class="fa fa-minus"></i>
          <%} %>
          <% var gain = Math.Abs(userPosition.LastPosition - userPosition.Position); %>
          <small><%: gain > 0 ? gain.ToString(CultureInfo.InvariantCulture) : "" %></small>
      </td>
        <td class="l namesmall">
            <%= userPoints.User.UserName == Context.User.Identity.Name ? "<strong>" : string.Empty%>
            <%: Html.ActionLink(userPoints.User.UserName, "expired", "bet", new {id=userPoints.User.UserId}, null)%>
            <%= userPoints.User.UserName == Context.User.Identity.Name ? "</strong>" : string.Empty%>
        </td>
        <td class="c"><%: userPoints.TotalDayPoints%></td>
    </tr>
    <%lineIndex++; } %>
    </tbody>
</table>
<%} %>

<% var lastMatches = ViewData["LastMatches"] as IList<BigBallz.Models.Match>; %>
<%if (lastMatches != null && lastMatches.Any())
  {%>
<hr />
<h2>Últimas Partidas</h2>

<%var lineIndex = 0;%>
    <table class="match-table">
       <tbody>
    <%foreach (var match in lastMatches)
      {%>
    <tr class="<%= lineIndex%2==0 ? "ui-state-default": "odd"%>">
        <td class="c"><%= Html.TeamFlag(match.Team1Id, match.Team1.Name)%>&nbsp;<%= Html.Encode(match.Score1)%>&nbsp;X&nbsp;<%= Html.Encode(match.Score2)%>&nbsp;<%= Html.TeamFlag(match.Team2Id, match.Team2.Name)%>&nbsp;<%: Html.ActionLink("ver apostas", "matchbets", "bet", new { id = match.MatchId }, null)%>
         </td>
    </tr>
    <%lineIndex++; } %>
    </tbody>
    </table>
<%} %>


<% var matches = ViewData["NextMatches"] as IList<BigBallz.Models.Match>; %>
<%if (matches!=null && matches.Any()) {%>
<hr />
<h2>Próximas Partidas</h2>
<%var lineIndex = 0;%>
    <table class="match-table">
       <tbody>
    <%foreach (var match in matches){%>
    <tr class="<%= lineIndex%2==0 ? "ui-state-default": "odd"%>">
        <td class="c dt" style="width:70%;"><%= Html.Encode(match.StartTime.ToString("dd/MM"))%>&nbsp;&nbsp;<%= Html.Encode(match.StartTime.ToString("HH:mm"))%>
            <% if (match.StartTime.AddHours(-1) <= DateTime.Now.BrazilTimeZone())
               {%>
                 <%: Html.ActionLink("ver apostas", "matchbets", "bet", new { id = match.MatchId }, new {style="display:block;"})%>  
               <%} %>
        </td>
        <td class="c"><%= Html.TeamFlag(match.Team1Id, match.Team1.Name)%>&nbsp;<%= Html.Encode(match.Team1Id)%></td>
        <td class="c mResult"> X </td>
        <td class="c"><%= Html.TeamFlag(match.Team2Id, match.Team2.Name)%>&nbsp;<%= Html.Encode(match.Team2Id)%></td>        
    </tr>
    <%lineIndex++; } %>
    </tbody>
    </table>
<%} %>

