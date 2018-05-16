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
                        <th></th>
                        <th>Jogador</th>
                        <th class="text-center">#</t>
                        <th>Pts</th>
                        <th>Exato</th>
                        <th>Bonus</th>
                        <th>Rodada</th>
                    </tr>
                    </thead>
                    <tbody>
                    <%foreach (var userPoint in Model) {%>

                        <tr class="<%= userPoint.User.UserId == (int?)ViewData["User"] ? "info" : "" %>">
                            <td class="profile-pic"><img class="profile-pic img-responsive img-circle" src="<%=Html.GetUserPhotoUrl(userPoint.User)%>"/></td>
                            <td><%=userPoint.User.UserName%></td>
                            <% var classe = "";
                               if (userPoint.Position < userPoint.LastPosition) classe = "text-success";
                               else if (userPoint.Position > userPoint.LastPosition) classe = "text-danger";%>
                            <td class="text-center"><span class="badge"><%if (userPoint.Position < 4) {%><i class="fa fa-trophy <%=userPoint.Position == 1 ? "gold" : userPoint.Position == 2 ? "silver" : "bronze" %>"></i> <%} %><%=userPoint.Position%></span><sup class="<%=classe %>">
                                <%if (userPoint.Position < userPoint.LastPosition) {%>
                                    <i class="glyphicon glyphicon-arrow-up" aria-hidden="true"></i>
                                <%} else if (userPoint.Position > userPoint.LastPosition) {%>
                                    <i class="glyphicon glyphicon-arrow-down" aria-hidden="true"></i>
                                <%}%>
                                <% var gain = Math.Abs(userPoint.LastPosition - userPoint.Position); %>
                                <small><%: gain > 0 ? gain.ToString() : "" %></small></sup></td>
                            <td><%=userPoint.TotalPoints%></td>
                            <td><%=userPoint.TotalExactScore%></td>  
                            <td><%=userPoint.TotalBonusPoints%></td>
                            <td><%=userPoint.TotalDayPoints%></td>
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