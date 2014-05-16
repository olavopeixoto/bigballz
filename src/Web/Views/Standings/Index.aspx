<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IList<BigBallz.Models.UserPoints>>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent"></asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="MainContent">
<br />
<table class="match-table">
<thead class="ui-widget-header">
<tr>
<th></th>
<th>Posição</th>
<th>Nome</th>
<th>Pontos</th>
<th>Placar Exato</th>
<th>Bonus</th>
<th>Pontos no Dia</th>
<th></th>
</tr>
</thead>
<tbody>
<%var lineIndex = 0; foreach (var userPoint in Model) {%>
  <%--<tr class="<%= lineIndex%2==0 ? "ui-state-default": "odd"%>">--%>
  <tr class="<%= lineIndex == 0 || lineIndex == 1 || lineIndex == 2 ? "ui-state-default": "odd"%>">
  <td class="c" style="width:50px;height:50px;"><%=Html.GetUserPhoto(userPoint.User)%></td>
  <td class="c"><%=userPoint.Position%></td>
  <td class="l"><%=userPoint.User.UserName%></td>
  <td class="c"><%=userPoint.TotalPoints%></td>  
  <td class="c"><%=userPoint.TotalExactScore%></td>  
  <td class="c"><%=userPoint.TotalBonusPoints%></td>
  <td class="c"><%=userPoint.TotalDayPoints%></td>
  <td class="c"><%:Html.ActionLink("ver apostas", "expired", "bet", new {id=userPoint.User.UserId}, null)%></td>
  </tr>
<%lineIndex++; } %>
</tbody>
</table>
<div class="fb-comments" data-href="<%=Url.Action("index", "standings",null,FormsAuthentication.RequireSSL ? "https" : "http") %>" data-width="675" data-numposts="5" data-colorscheme="light"></div>
</asp:Content>
<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="Scripts"></asp:Content>