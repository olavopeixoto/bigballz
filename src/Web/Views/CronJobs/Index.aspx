<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<BigBallz.Services.ICronJobTask[]>" MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Import Namespace="BigBallz.Core" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent"> - Tarefas Agendadas</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="MainContent">
<h2>Tarefas Agendadas</h2>
<p>Horário Local: <%=DateTime.Now.FormatDateTime()%></p>
<p>Horário Brasil: <%=DateTime.Now.BrazilTimeZone().FormatDateTime()%></p>
<table class="match-table">
<thead class="ui-widget-header">
<tr>
    <th>Nome</th>
    <th>Horário</th>
</tr>
</thead>
<tbody>
<%var i = 0;
  foreach (var task in Model) {%>
    <tr class="<%= i%2==0 ? "ui-state-default": "odd"%>">
        <td class="c"><%=task.Name%></td>
        <td class="c"><%=task.AbsoluteExpiration.HasValue ? task.AbsoluteExpiration.Value.FormatDateTime() : task.SlidingExpiration.HasValue ? "em " + task.SlidingExpiration.Value.TotalSeconds + " segundos" : string.Empty%></td>
    </tr>
<%i++;}%>
</tbody>
</table>
</asp:Content>
<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="Scripts"></asp:Content>