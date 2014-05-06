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
</table><%--
<script src="http://static.ak.connect.facebook.com/js/api_lib/v0.4/FeatureLoader.js.php" type="text/javascript"></script>
<fb:comments></fb:comments>
<script type="text/javascript">
    FB.init("945736eb634f9b9b98e4b8bd98cf6a15", "<%=Url.SiteRoot()%>/xd_receiver.htm");
</script>
<script type="text/javascript" src="http://cdn.widgetserver.com/syndication/subscriber/InsertWidget.js"></script>
<script type="text/javascript">
if (WIDGETBOX) WIDGETBOX.renderWidget('55bd568e-768d-4a4a-a1f5-cd25f05fcb83');
</script>--%>
</asp:Content>
<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="Scripts"></asp:Content>