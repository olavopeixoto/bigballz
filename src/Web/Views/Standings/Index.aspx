<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IList<BigBallz.Models.UserPoints>>" MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Import Namespace="BigBallz.Core" %>
<%@ Import Namespace="BigBallz.Core.Extension.Web" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent"></asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="MainContent">
<br />
<table class="match-table">
<thead class="ui-widget-header">
<tr>
<th></th>
<th>Posição</th>
<th></th>
<th>Nome</th>
<th>Pontos</th>
<th>Placar Exato</th>
<th>Bonus</th>
<th>Pontos da Rodada</th>
<th></th>
</tr>
</thead>
<tbody>
<%var lineIndex = 0; foreach (var userPoint in Model) {%>
  <tr class="<%= userPoint.Position < 4 ? "ui-state-default": "odd"%>">
  <td class="c" style="width:50px;height:50px;"><%=Html.GetUserPhoto(userPoint.User)%></td>
  <% var classe = "";
       if (userPoint.Position < userPoint.LastPosition) classe = "up";
       else if (userPoint.Position > userPoint.LastPosition) classe = "down";%>
  <td class="c"><%=userPoint.Position%></td>
  <td class="c <%: classe %>" style="white-space: nowrap"><%if (userPoint.Position < userPoint.LastPosition) {%>
      <i class="fa fa-long-arrow-up"></i>
      <%} else if (userPoint.Position > userPoint.LastPosition) {%>
      <i class="fa fa-long-arrow-down"></i>
      <%} else {%>
      <i class="fa fa-minus"></i>
      <%} %>
      <% var gain = Math.Abs(userPoint.LastPosition - userPoint.Position); %>
      <small><%: gain > 0 ? gain.ToString() : "" %></small>
  </td>
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
<% var shareUrl = Url.Action("standings", "home", new {user = ViewData["UserId"], date = DateTime.UtcNow.BrazilTimeZone().ToString("yyyy-MM-ddTHH-mm")}, Request.IsSecureConnection ? "https" : "http"); %>
<div class="fb-share-button" data-href="<%=shareUrl %>" data-layout="button" data-size="small" data-mobile-iframe="true"><a target="_blank" href="https://www.facebook.com/sharer/sharer.php?u=<%=shareUrl.UrlEncode()%>&amp;src=sdkpreparse" class="fb-xfbml-parse-ignore">Share</a></div>
<script>!function (d, s, id) { var js, fjs = d.getElementsByTagName(s)[0]; if (!d.getElementById(id)) { js = d.createElement(s); js.id = id; js.src = "https://platform.twitter.com/widgets.js"; fjs.parentNode.insertBefore(js, fjs); } }(document, "script", "twitter-wjs");</script>
<a class="twitter-share-button"
   href="https://twitter.com/share"
   data-text="Se liga na classificação do BigBallz deste momento!"
   data-url="<%=shareUrl %>"
   data-lang="pt-BR"
   data-hashtags="bigballz2018"
   data-via="bigballz2018">
    Tweet
</a>
<div class="wabtn">
    <a href="whatsapp://send?text=<%=("Se liga na classificação do BigBallz deste momento! " + shareUrl).UrlEncode()%>" class="wa_btn wa_btn_s" target="_top">Compartilhar</a>
</div>
<div class="wabtn">
    <a class="label label-primary" target="_blank" href="<%=shareUrl%>"><i class="fa">&#xf0c1;</i> Permalink</a>
</div>
<div class="fb-comments" data-href="<%=Url.Action("index", "standings",null, Request.IsSecureConnection ? "https" : "http") %>" data-width="675" data-numposts="5" data-colorscheme="light"></div>
</asp:Content>
<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="Scripts"></asp:Content>
