<%@ Page Language="C#" MasterPageFile="Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IList<BigBallz.Models.UserPoints>>" %>

<asp:Content ID="FacebookMetadata" ContentPlaceHolderID="FacebookMetadata" runat="server">
    <meta property="og:url"           content="<%=Request.Url.AbsoluteUri %>" />
    <meta property="og:type"          content="website" />
    <meta property="og:title"         content="Classificação BigBallz 2018" />
    <meta property="og:description"   content="<%=((DateTime)ViewData["RequestDate"]).ToString("dddd, dd 'de' MMMM 'de' yyyy 'às' HH:mm") %>" />
    <meta property="og:image"         content="<%=Url.VersionedContent("~/public/images/ranking.png") %>" />
</asp:Content>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">, o Bolão da Copa do Mundo</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="content-section-a">

        <div class="container">
            
            <div class="table-responsive">
                <h1>Classificação <small><%=((DateTime)ViewData["RequestDate"]).ToString("dddd, dd 'de' MMMM 'de' yyyy 'às' HH:mm") %></small></h1>
                <div class="panel panel-default">
                <table class="table table-striped table-hover table-condensed table-center">
                    <thead>
                    <tr>
                        <th>#</th>
                        <th class="hidden-xs"></th>
                        <th>Jogador</th>
                        <th>Pts</th>
                        <th>Exato</th>
                        <th>Bonus</th>
                        <th>Rodada</th>
                    </tr>
                    </thead>
                    <tbody>
                    <%foreach (var userPoint in Model) {%>

                        <tr class="<%= userPoint.User.UserId == (int?)ViewData["User"] ? "info" : "" %>">
                            <% var classe = "";
                               if (userPoint.Position < userPoint.LastPosition) classe = "text-success";
                               else if (userPoint.Position > userPoint.LastPosition) classe = "text-danger";%>
                            <td><span class="badge"><%=userPoint.Position%></span><sup class="<%=classe %>">
                                <%if (userPoint.Position < userPoint.LastPosition) {%>
                                    <i class="glyphicon glyphicon-chevron-up" aria-hidden="true"></i>
                                <%} else if (userPoint.Position > userPoint.LastPosition) {%>
                                    <i class="glyphicon glyphicon-chevron-down" aria-hidden="true"></i>
                                <%}%>
                                <% var gain = Math.Abs(userPoint.LastPosition - userPoint.Position); %>
                                <small><%: gain > 0 ? gain.ToString() : "" %></small></sup></td>
                            <td class="hidden-xs" style="width:60px;"><%=Html.GetUserPhoto(userPoint.User)%></td>
                            <td><%=userPoint.User.UserName%></td>
                            <td><%=userPoint.TotalPoints%></td>
                            <td><%=userPoint.TotalExactScore%></td>  
                            <td><%=userPoint.TotalBonusPoints%></td>
                            <td><%=userPoint.TotalDayPoints%> <%if (userPoint.Position < 4) {%><i class="fa fa-trophy pull-right <%=userPoint.Position == 1 ? "gold" : userPoint.Position == 2 ? "silver" : "bronze" %>"></i><%} %></td>
                        </tr>
                    <%} %>
                    </tbody>
                </table>
            </div>
            </div>
        </div>
        <!-- /.container -->

    </div>
    <!-- /.content-section-a -->
</asp:Content>