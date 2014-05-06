<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<BigBallz.Models.Match>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Jogos</h2>
    <h3>
        Fase de grupos</h3>
    <div id="group-stage">
        <%foreach (var group in Model.Select(x => x.Team1.Group).Distinct())
          {%>
        <table class="match-table" summary="<%=group.Name%>">
            <caption>
                <%=group.Name%></caption>
            <thead class="ui-widget-header">
                <tr>
                    <th>
                    </th>
                    <th>
                        Jogo
                    </th>
                    <th>
                        Data - Horário
                    </th>
                    <th colspan="5">
                    </th>
                </tr>
            </thead>
            <tbody>
                <%var i = 0;
                  foreach (var match in Model.Where(x => x.StageId == 1 && x.Team1.GroupId == group.GroupId))
                  {%>
                <tr class="<%= i%2==0 ? "ui-state-default": "odd"%>">
                    <td>
                        <%= Html.ActionLink("Editar", "Edit", new { id = match.MatchId })%>
                    </td>
                    <td class="c mNum">
                        <%= match.MatchId%>
                    </td>
                    <td class="l dt">
                        <%= Html.Encode(match.StartTime.ToString("dd/MM HH:mm"))%>
                    </td>
                    <td class="c">
                        <%= Html.TeamFlag(match.Team1Id)%>
                    </td>
                    <td class="l homeTeam">
                        <%= Html.Encode(match.Team1.Name)%>
                    </td>
                    <td class="c mResult">
                        <%= Html.Encode(match.Score1)%>
                        X
                        <%= Html.Encode(match.Score2)%>
                    </td>
                    <td class="r awayTeam">
                        <%= Html.Encode(match.Team2.Name)%>
                    </td>
                    <td class="c">
                        <%= Html.TeamFlag(match.Team2Id)%>
                    </td>
                </tr>
                <%i++;
              } %>
            </tbody>
        </table>
        <%} %>
    </div>
    <h3>
        2ª Fase</h3>
    <div id="stage-2">
        <table class="match-table" summary="2ª Fase">
            <thead class="ui-widget-header">
                <tr>
                    <th>
                    </th>
                    <th>
                        Jogo
                    </th>
                    <th>
                        Data - Horário
                    </th>
                    <th colspan="5">
                    </th>
                    <th>
                        Fase
                    </th>
                </tr>
            </thead>
            <tbody>
                <%var j = 0;
                  foreach (var match in Model.Where(x => x.StageId != 1))
                  {%>
                <tr class="<%= j%2==0 ? "ui-state-default": "odd"%>">
                    <td>
                        <%= Html.ActionLink("Edit", "Edit", new { id = match.MatchId })%>
                    </td>
                    <td class="c mNum">
                        <%= match.MatchId%>
                    </td>
                    <td class="l dt">
                        <%= Html.Encode(match.StartTime.ToString("dd/MM HH:mm"))%>
                    </td>
                    <td class="c">
                        <%= Html.TeamFlag(match.Team1Id)%>
                    </td>
                    <td class="l homeTeam">
                        <%= Html.Encode(match.Team1.Name)%>
                    </td>
                    <td class="c mResult">
                        <%= Html.Encode(match.Score1)%>
                        X
                        <%= Html.Encode(match.Score2)%>
                    </td>
                    <td class="r awayTeam">
                        <%= Html.Encode(match.Team2.Name)%>
                    </td>
                    <td class="c">
                        <%= Html.TeamFlag(match.Team2Id)%>
                    </td>
                    <td class="c">
                        <%= match.Stage.Name%>
                    </td>
                </tr>
                <%j++;
              } %>
            </tbody>
        </table>
    </div>
    <p>
        <%= Html.ActionLink("Novo jogo", "Create") %>
    </p>
</asp:Content>
