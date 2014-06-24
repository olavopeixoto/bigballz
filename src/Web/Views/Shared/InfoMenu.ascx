<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl"%>
<%@ Import Namespace="System.Threading" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="BigBallz.Core" %>
<%@ Import Namespace="BigBallz.Helpers" %>

<%var standings = ViewData["Standings"] as IList<BigBallz.Models.UserPoints>;%>

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
        <span style="display:block">(<%:currentUserStats.NullSafe(x => x.TotalPoints)%> pts - <%:currentUserStats.NullSafe(x => x.Position)%>� lugar)</span>
        <%} if (Page.User.Identity.IsAuthenticated && !Html.IsAuthorized())
          {%>
            <span style="display:block"><%=Html.ActionLink("Pagamento n�o registrado. Clique aqui.", "payment", "auth")%></span>
        <%}%>
        <%--<span style="display:block"><span title="3 placares exatos (333) | 1 placar parcial (333) | 5 resultados corretos (333)">90 pontos (85%)</span></span>--%>
        <% if (Context.User.IsInRole("admin")) { 
                if (Request.Cookies["x-profiler"] == null) { %>
                    <span style="display:block"><span><%= Html.ActionLink("Enable Profiler", "EnableProfiler", "auth") %></span></span>
            <% } else { %>
                     <span style="display:block"><span><%= Html.ActionLink("Disable Profiler", "DisableProfiler", "auth") %></span></span>
            <% }
         } %>
    </span>
  </div>
</div>

<%var pendingBets = ViewData["PendingBets"] as IList<BigBallz.Models.Match>;
if (pendingBets!=null && pendingBets.Any())
{
    var pendingBetsCount = pendingBets.Count();
    var plural = pendingBetsCount > 1 ? "s" : string.Empty;
    %>
    <p><%=Html.ActionLink(string.Format("Voc� tem {0} aposta{1} pendente{1} para as pr�ximas 24h", pendingBetsCount, plural), "index", "bet")%></p>
<%}%>

<%var prizes = ViewData["prizes"] as IList<decimal>;%>
<%if (prizes!=null) {%>
<%Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");%>
<hr />
<h2>Pr�mio Acumulado</h2>
<ol>
<li><%=prizes[0].ToString("C")%></li>
<li><%=prizes[1].ToString("C")%></li>
<li><%=prizes[2].ToString("C")%></li>
</ol>
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
        <td class="c"><%=userPoints.Position%>�</td>
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

<%var dayStandings = ViewData["DayStandings"] as IList<BigBallz.Models.UserPoints>;%>
<%if (dayStandings != null && dayStandings.Any(s => s.TotalDayPoints > 0))
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
        <td class="c"><%=userPoints.Position%>�</td>
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
<h2>�ltimas Partidas</h2>

<%var i = 0; var lineIndex = 0;%>
    <table class="match-table">
       <tbody>
    <%foreach (var match in lastMatches)
      {%>
    <tr class="<%= lineIndex%2==0 ? "ui-state-default": "odd"%>">
        <td class="c"><%= Html.TeamFlag(match.Team1Id, match.Team1.Name)%>&nbsp;<%= Html.Encode(match.Score1)%>&nbsp;X&nbsp;<%= Html.Encode(match.Score2)%>&nbsp;<%= Html.TeamFlag(match.Team2Id, match.Team2.Name)%>&nbsp;<%: Html.ActionLink("ver apostas", "matchbets", "bet", new { id = match.MatchId }, null)%>
         </td>
    </tr>
    <%i++; lineIndex++; } %>
    </tbody>
    </table>
<%} %>


<% var matches = ViewData["NextMatches"] as IList<BigBallz.Models.Match>; %>
<%if (matches!=null && matches.Any()) {%>
<hr />
<h2>Pr�ximas Partidas</h2>
<%var i = 0; var lineIndex = 0;%>
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
    <%i++; lineIndex++; } %>
    </tbody>
    </table>
<%} %>

