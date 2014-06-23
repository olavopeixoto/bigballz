<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<BigBallz.Models.Team>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
	<%=Html.ScriptInclude("Team.js") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

 <h2>Times</h2>
    <div id="group-stage">
        <%foreach (var group in Model.Select(x => x.Group).Distinct().OrderBy(g => g.Name))
          {%>
        <table class="match-table" summary="<%=group.Name%>">
            <caption><%=group.Name%></caption>
            <thead class="ui-widget-header">
                <tr>
                    <th>
                    </th>
                    <th>
                    </th>
                    <th>
                        Id
                    </th>
                    <th>
                        Nome
                    </th>
                </tr>
            </thead>
            <tbody>
                <%var i = 0;
                  foreach (var team in Model.Where(x => x.GroupId == group.GroupId))
                  {%>
                <tr class="<%= i%2==0 ? "ui-state-default": "odd"%>">
                    <td class="homeTeam">
                        <%= Html.ActionLink("Editar", "Edit", new { id = team.TeamId })%>
                    </td>
                    <td class="l homeTeam">
                        <%= Html.TeamFlag(team.TeamId)%>
                    </td>
                    <td class="l homeTeam">
                        <%: team.TeamId%>
                    </td>
                    <td class="l">
                        <%: team.Name%>
                    </td>
                </tr>
                <%i++;
              } %>
            </tbody>
        </table>
        <%} %>
    </div>
   
    <p>
        <%= Html.ActionLink("Novo Time", "Create") %>
    </p>

</asp:Content>