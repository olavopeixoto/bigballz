<%@ Page Language="C#" MasterPageFile="Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">, o Bolão da Copa do Mundo</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <%
        var returnUrl = Url.Encode(Url.Action("handleresponse", "auth", null, FormsAuthentication.RequireSSL ? "https" : "http"));
    %>
    <div class="intro-header">

        <div class="container">

            <div class="row">
                <div class="col-lg-12">
                    <div class="intro-message">
                        <h1>BigBallz</h1>
                        <h3>O Bolão da Copa do Mundo</h3>
                        <%=Html.CountDown((DateTime) ViewData["StartDate"]) %>
                        <hr class="intro-divider">
                        <ul class="list-inline intro-social-buttons">
                            <li>
                                <%if (!Request.IsAuthenticated) {%>
                                <a href="https://bigballz.rpxnow.com/openid/v2/signin?token_url=<%=returnUrl%>" onclick="return false;" class="btn btn-default btn-lg rpxnow"><i class="fa fa-sign-in fa-fw"></i> <span class="network-name">Entrar</span></a>
                                <%} else {%>
                                <a href="<%=Url.Action("index", "standings")%>" class="btn btn-default btn-lg"><i class="fa fa-sort-numeric-asc fa-fw"></i> <span class="network-name">Ranking</span></a>
                                <%}%>
                            </li>
                            <li><div class="fb-like" data-href="<%=Url.SiteRoot() %>" data-layout="button_count" data-action="like" data-show-faces="true" data-share="true"></div></li>
                        </ul>
                    </div>
                </div>
            </div>

        </div>
        <!-- /.container -->

    </div>
    <!-- /.intro-header -->

    <div class="content-section-a">

        <div class="container">

            <div class="row">
                <div class="col-lg-5 col-sm-6">
                    <hr class="section-heading-spacer">
                    <div class="clearfix"></div>
                    <h2 class="section-heading">Login social</h2>
                    <p class="lead">Não é necessário se cadastrar, basta usar uma conta de algum destes provedores</p>
                </div>
                <div class="col-lg-5 col-lg-offset-2 col-sm-6">
                    <iframe src="https://bigballz.rpxnow.com/openid/embed?token_url=<%=returnUrl %>&language_preference=pt-BR&flags=hide_sign_in_with,show_provider_list" scrolling="no" frameBorder="no" allowtransparency="true" style="width:400px;height:240px"></iframe>
                </div>
            </div>

        </div>
        <!-- /.container -->

    </div>
    <!-- /.content-section-a -->

    <div class="content-section-b">

        <div class="container">

            <div class="row">
                <div class="col-lg-5 col-lg-offset-1 col-sm-push-6  col-sm-6">
                    <hr class="section-heading-spacer">
                    <div class="clearfix"></div>
                    <h2 class="section-heading">Mais interatividade</h2>
                    <p class="lead">Aposte nos placares dos jogos de todas as fases da Copa até 1h antes de cada jogo</p>
                    <p class="lead">Receba e-mails com o resumo das apostas de todos os jogadores antes do início de cada partida</p>
                </div>
                <div class="col-lg-5 col-sm-pull-6  col-sm-6">
                    <img class="img-responsive" src="<%=Url.Content("~/public/images/grana.png") %>" alt="">
                </div>
            </div>

        </div>
        <!-- /.container -->

    </div>
    <!-- /.content-section-b -->

    <div class="content-section-a">

        <div class="container">

            <div class="row">
                <div class="col-lg-5 col-sm-6">
                    <hr class="section-heading-spacer">
                    <div class="clearfix"></div>
                    <h2 class="section-heading">24h</h2>
                    <p class="lead">Acompanhe em tempo real a classificação do Bolão</p>
                </div>
                <div class="col-lg-5 col-lg-offset-2 col-sm-6">
                    <img class="img-responsive" src="<%=Url.Content("~/public/images/clock.png") %>" alt="">
                </div>
            </div>

        </div>
        <!-- /.container -->

    </div>
    <!-- /.content-section-a -->

    <div class="content-section-b">

        <div class="container">

            <div class="row">
                <div class="col-lg-5 col-lg-offset-1 col-sm-push-6  col-sm-6">
                    <hr class="section-heading-spacer">
                    <div class="clearfix"></div>
                    <h2 class="section-heading">Mais chances</h2>
                    <p class="lead">Os três primeiros colocados são premiados</p>
                </div>
                <div class="col-lg-5 col-sm-pull-6  col-sm-6">
                    <img class="img-responsive" src="<%=Url.Content("~/public/images/ranking.png") %>" alt="">
                </div>
            </div>

        </div>
        <!-- /.container -->

    </div>
    <!-- /.content-section-b -->

    <div class="content-section-a">

        <div class="container">

            <div class="row">
                <div class="col-lg-5 col-sm-6">
                    <hr class="section-heading-spacer">
                    <div class="clearfix"></div>
                    <h2 class="section-heading">Bonus</h2>
                    <p class="lead">Com a aposta bonus o jogo pode virar no final.<br>Antes de começar o campeonato, chute quem será o campeão de cada grupo além do campeão, vice e terceiro colocado da copa. Confira o <%=Html.ActionLink("regulamento", "rules", "home")%></p>
                </div>
                <div class="col-lg-5 col-lg-offset-2 col-sm-6">
                    <img class="img-responsive" src="<%=Url.Content("~/public/images/bonus.png") %>" alt="">
                </div>
            </div>

        </div>
        <!-- /.container -->

    </div>
    <!-- /.content-section-a -->
    
    <div class="content-section-b">

        <div class="container">

            <div class="row">
                <div class="col-lg-5 col-lg-offset-1 col-sm-push-6  col-sm-6">
                    <hr class="section-heading-spacer">
                    <div class="clearfix"></div>
                    <h2 class="section-heading">Espalhe a notícia</h2>
                    <p class="lead">
                        Convide os seus amigos, quanto mais gente participar maior a bolada do prêmio!
                        <div class="fb-like twitter-share-button" data-href="<%=Url.SiteRoot() %>" data-layout="button_count" data-action="recommend" data-show-faces="true" data-share="true"></div>
                        <a href="https://twitter.com/share" class="twitter-share-button" data-url="<%=Url.SiteRoot() %>" data-lang="pt-BR">Tweet</a>
                        <script>!function (d, s, id) { var js, fjs = d.getElementsByTagName(s)[0]; if (!d.getElementById(id)) { js = d.createElement(s); js.id = id; js.src = "https://platform.twitter.com/widgets.js"; fjs.parentNode.insertBefore(js, fjs); } }(document, "script", "twitter-wjs");</script>                    </p>
                </div>
                <div class="col-lg-5 col-sm-pull-6  col-sm-6">
                    <img class="img-responsive" src="<%=Url.Content("~/public/images/bolao_paises.png") %>" alt="">
                </div>
            </div>

        </div>
        <!-- /.container -->

    </div>
    <!-- /.content-section-b -->

    <div class="banner">

        <div class="container">

            <div class="row">
                <div class="col-lg-6">
                    <h2>Entre agora no BigBallz:</h2>
                </div>
                <div class="col-lg-6">
                    <ul class="list-inline banner-social-buttons">
                        <li><a href="<%=Url.Action("rules", "home") %>" class="btn btn-default btn-lg"><i class="fa fa-book fa-fw"></i> <span class="network-name">Regulamento</span></a></li>
                        <%if (!Request.IsAuthenticated) {%>
                        <li><a href="https://bigballz.rpxnow.com/openid/v2/signin?token_url=<%=returnUrl %>" onclick="return false;" class="btn btn-default btn-lg rpxnow"><i class="fa fa-sign-in fa-fw"></i> <span class="network-name">Entrar</span></a></li>
                        <%} else {%>
                        <li><a href="<%=Url.Action("index", "standings")%>" class="btn btn-default btn-lg"><i class="fa fa-sort-numeric-asc fa-fw"></i> <span class="network-name">Ranking</span></a></li>
                        <%}%>
                    </ul>
                </div>
            </div>

        </div>
        <!-- /.container -->

    </div>
    <!-- /.banner -->
</asp:Content>