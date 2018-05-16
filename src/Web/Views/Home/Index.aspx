<%@ Page Language="C#" MasterPageFile="Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="BigBallz.Core.Extension.Web" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">, o Bolão da Copa do Mundo</asp:Content>

<asp:Content ID="FacebookMetadata" ContentPlaceHolderID="FacebookMetadata" runat="server">
    <meta property="og:url"           content="<%=Url.SiteRoot() %>" />
    <meta property="og:type"          content="website" />
    <meta property="og:title"         content="BigBallz" />
    <meta property="og:description"   content="O Bolão da Copa do Mundo" />
    <meta property="og:image"         content="<%=Url.VersionedContent("~/public/images/grana.png") %>" />
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
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
                            <%if (!Request.IsAuthenticated) {%>
                            <li class="li-login">
                                <a class="btn btn-default btn-lg janrainEngage" href="#"><i class="fa fa-sign-in fa-fw"></i> <span class="network-name">Entrar</span></a>
                            </li>
                            <%}%>
                            <li><div class="fb-like" data-href="<%=Url.SiteRoot() %>" data-layout="button_count" data-action="like" data-size="small" data-show-faces="true" data-share="true"></div></li>
                            <li><a class="twitter-share-button"
                                   href="https://twitter.com/share"
                                   data-text="Estou participando do BigBallz, o bolão da Copa do Mundo. Venha participar também!"
                                   data-url="<%=Url.SiteRoot() %>"
                                   data-lang="pt-BR"
                                   data-hashtags="bigballz2018"
                                   data-via="bigballz2018">
                                Tweet
                            </a></li>
                            <li>
                                <a href="whatsapp://send?text=<%=("Estou participando do BigBallz, o bolão da Copa do Mundo. Venha participar também: " + Url.SiteRoot()).UrlEncode()%>" class="wa_btn wa_btn_s" target="_top">Compartilhar</a>
                            </li>
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
                    <div style="padding: 60px 0 0 0;">
                        <ul class="providers" id="janrainProviders_0" style="float: left !important; list-style-type: none !important; margin: 0px !important; padding: 0 0 0 0 !important;">
                            <li id="janrain-google" role="button" style="list-style: none !important; height: 30px !important; width: 185px !important; margin-top: 0px !important; margin-right: 5px !important; margin-bottom: 5px !important; position: relative !important; border: 1px solid rgb(204, 204, 204) !important; border-top-left-radius: 5px !important; border-top-right-radius: 5px !important; border-bottom-right-radius: 5px !important; border-bottom-left-radius: 5px !important; white-space: nowrap !important; overflow: hidden !important; background-color: rgb(227, 227, 227); background-image: -webkit-linear-gradient(bottom, rgb(238, 238, 238), rgb(255, 255, 255));"><span tabindex="1" href="javascript:void(0);" style="font-family: Helvetica, 'lucida grande', Verdana, sans-serif !important; font-size: 12px !important; line-height: 14px !important; margin-left: auto !important; margin-right: auto !important; text-decoration: none !important; display: block !important; padding-left: 5px !important; padding-right: 5px !important; text-align: left !important; width: auto !important;"><span class="janrain-provider-icon-24 janrain-provider-icon-google" style="margin-top: 3px !important; background-color: transparent !important;"></span><span class="janrain-provider-text-color-google" style="font-family: Helvetica, 'lucida grande', Verdana, sans-serif !important; margin-left: 7px !important; text-align: left !important; margin-top: 9px !important; vertical-align: top !important; display: inline-block !important;">Google</span></span></li>
                            <li id="janrain-facebook" role="button" style="list-style: none !important; height: 30px !important; width: 185px !important; margin-top: 0px !important; margin-right: 5px !important; margin-bottom: 5px !important; position: relative !important; border: 1px solid rgb(204, 204, 204) !important; border-top-left-radius: 5px !important; border-top-right-radius: 5px !important; border-bottom-right-radius: 5px !important; border-bottom-left-radius: 5px !important; white-space: nowrap !important; overflow: hidden !important; background-color: rgb(227, 227, 227); background-image: -webkit-linear-gradient(bottom, rgb(238, 238, 238), rgb(255, 255, 255));"><span tabindex="3" href="javascript:void(0);" style="font-family: Helvetica, 'lucida grande', Verdana, sans-serif !important; font-size: 12px !important; line-height: 14px !important; margin-left: auto !important; margin-right: auto !important; text-decoration: none !important; display: block !important; padding-left: 5px !important; padding-right: 5px !important; text-align: left !important; width: auto !important;"><span class="janrain-provider-icon-24 janrain-provider-icon-facebook" style="margin-top: 3px !important; background-color: transparent !important;"></span><span class="janrain-provider-text-color-facebook" style="font-family: Helvetica, 'lucida grande', Verdana, sans-serif !important; margin-left: 7px !important; text-align: left !important; margin-top: 9px !important; vertical-align: top !important; display: inline-block !important;">Facebook</span></span></li>
                            <li id="janrain-microsoftaccount" role="button" style="list-style: none !important; height: 30px !important; width: 185px !important; margin-top: 0px !important; margin-right: 5px !important; margin-bottom: 5px !important; position: relative !important; border: 1px solid rgb(204, 204, 204) !important; border-top-left-radius: 5px !important; border-top-right-radius: 5px !important; border-bottom-right-radius: 5px !important; border-bottom-left-radius: 5px !important; white-space: nowrap !important; overflow: hidden !important; background-color: rgb(227, 227, 227); background-image: -webkit-linear-gradient(bottom, rgb(238, 238, 238), rgb(255, 255, 255));"><span tabindex="5" href="javascript:void(0);" style="font-family: Helvetica, 'lucida grande', Verdana, sans-serif !important; font-size: 12px !important; line-height: 14px !important; margin-left: auto !important; margin-right: auto !important; text-decoration: none !important; display: block !important; padding-left: 5px !important; padding-right: 5px !important; text-align: left !important; width: auto !important;"><span class="janrain-provider-icon-24 janrain-provider-icon-microsoftaccount" style="margin-top: 3px !important; background-color: transparent !important;"></span><span class="janrain-provider-text-color-microsoftaccount" style="font-family: Helvetica, 'lucida grande', Verdana, sans-serif !important; margin-left: 7px !important; text-align: left !important; margin-top: 9px !important; vertical-align: top !important; display: inline-block !important;">Microsoft Account</span></span></li>
                        </ul>
                        <ul class="providers" id="janrainProviders_1" style="float: left !important; list-style-type: none !important; margin: 0px !important; padding: 0 0 0 0 !important;">
                            <li id="janrain-yahoo" role="button" style="list-style: none !important; height: 30px !important; width: 185px !important; margin-top: 0px !important; margin-right: 5px !important; margin-bottom: 5px !important; position: relative !important; border: 1px solid rgb(204, 204, 204) !important; border-top-left-radius: 5px !important; border-top-right-radius: 5px !important; border-bottom-right-radius: 5px !important; border-bottom-left-radius: 5px !important; white-space: nowrap !important; overflow: hidden !important; background-color: rgb(227, 227, 227); background-image: -webkit-linear-gradient(bottom, rgb(238, 238, 238), rgb(255, 255, 255));"><span tabindex="2" href="javascript:void(0);" style="font-family: Helvetica, 'lucida grande', Verdana, sans-serif !important; font-size: 12px !important; line-height: 14px !important; margin-left: auto !important; margin-right: auto !important; text-decoration: none !important; display: block !important; padding-left: 5px !important; padding-right: 5px !important; text-align: left !important; width: auto !important;"><span class="janrain-provider-icon-24 janrain-provider-icon-yahoo" style="margin-top: 3px !important; background-color: transparent !important;"></span><span class="janrain-provider-text-color-yahoo" style="font-family: Helvetica, 'lucida grande', Verdana, sans-serif !important; margin-left: 7px !important; text-align: left !important; margin-top: 9px !important; vertical-align: top !important; display: inline-block !important;">Yahoo!</span></span></li>
                            <li id="janrain-twitter" role="button" style="list-style: none !important; height: 30px !important; width: 185px !important; margin-top: 0px !important; margin-right: 5px !important; margin-bottom: 5px !important; position: relative !important; border: 1px solid rgb(204, 204, 204) !important; border-top-left-radius: 5px !important; border-top-right-radius: 5px !important; border-bottom-right-radius: 5px !important; border-bottom-left-radius: 5px !important; white-space: nowrap !important; overflow: hidden !important; background-color: rgb(227, 227, 227); background-image: -webkit-linear-gradient(bottom, rgb(238, 238, 238), rgb(255, 255, 255));"><span tabindex="4" href="javascript:void(0);" style="font-family: Helvetica, 'lucida grande', Verdana, sans-serif !important; font-size: 12px !important; line-height: 14px !important; margin-left: auto !important; margin-right: auto !important; text-decoration: none !important; display: block !important; padding-left: 5px !important; padding-right: 5px !important; text-align: left !important; width: auto !important;"><span class="janrain-provider-icon-24 janrain-provider-icon-twitter" style="margin-top: 3px !important; background-color: transparent !important;"></span><span class="janrain-provider-text-color-twitter" style="font-family: Helvetica, 'lucida grande', Verdana, sans-serif !important; margin-left: 7px !important; text-align: left !important; margin-top: 9px !important; vertical-align: top !important; display: inline-block !important;">Twitter</span></span></li>
                            <li id="janrain-instagram" role="button" style="list-style: none !important; height: 30px !important; width: 185px !important; margin-top: 0px !important; margin-right: 5px !important; margin-bottom: 5px !important; position: relative !important; border: 1px solid rgb(204, 204, 204) !important; border-top-left-radius: 5px !important; border-top-right-radius: 5px !important; border-bottom-right-radius: 5px !important; border-bottom-left-radius: 5px !important; white-space: nowrap !important; overflow: hidden !important; background-color: rgb(227, 227, 227); background-image: -webkit-linear-gradient(bottom, rgb(238, 238, 238), rgb(255, 255, 255));"><span tabindex="6" href="javascript:void(0);" style="font-family: Helvetica, 'lucida grande', Verdana, sans-serif !important; font-size: 12px !important; line-height: 14px !important; margin-left: auto !important; margin-right: auto !important; text-decoration: none !important; display: block !important; padding-left: 5px !important; padding-right: 5px !important; text-align: left !important; width: auto !important;"><span class="janrain-provider-icon-24 janrain-provider-icon-instagram" style="margin-top: 3px !important; background-color: transparent !important;"></span><span class="janrain-provider-text-color-instagram" style="font-family: Helvetica, 'lucida grande', Verdana, sans-serif !important; margin-left: 7px !important; text-align: left !important; margin-top: 9px !important; vertical-align: top !important; display: inline-block !important;">Instagram</span></span></li>
                        </ul>
                    </div>
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
                    <p class="lead">Aposte nos placares dos jogos de todas as fases da Copa até 1h antes de cada jogo<br/>
                    Receba e-mails com o resumo das apostas de todos os jogadores antes do início de cada partida</p>
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
                    <p class="lead">Acompanhe em tempo real a classificação do Bolão.</p>
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
                    <p class="lead">Com a aposta bonus o jogo pode virar no final.<br>Antes de começar o campeonato, chute quem será o campeão de cada grupo além do campeão, vice, terceiro e quarto colocado da copa. Confira o <%=Html.ActionLink("regulamento", "rules", "home")%></p>
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
                        <div class="fb-like fb-share-button" data-href="<%=Url.SiteRoot() %>" data-layout="button_count" data-action="like" data-size="small" data-show-faces="true" data-share="true"></div>
                        <a class="twitter-share-button"
                           href="https://twitter.com/share"
                           data-text="Estou participando do BigBallz, o bolão da Copa do Mundo. Venha participar também!"
                           data-url="<%=Url.SiteRoot() %>"
                           data-lang="pt-BR"
                           data-hashtags="bigballz2018"
                           data-via="bigballz2018">
                            Tweet
                        </a>
                        <script>!function (d, s, id) { var js, fjs = d.getElementsByTagName(s)[0]; if (!d.getElementById(id)) { js = d.createElement(s); js.id = id; js.src = "https://platform.twitter.com/widgets.js"; fjs.parentNode.insertBefore(js, fjs); } }(document, "script", "twitter-wjs");</script>
                        <div class="g-plus" data-action="share" data-annotation="bubble"></div>
                        <script type="text/javascript">
                            window.___gcfg = { lang: 'pt-BR' };

                            (function () {
                                var po = document.createElement('script'); po.type = 'text/javascript'; po.async = true;
                                po.src = 'https://apis.google.com/js/platform.js';
                                var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(po, s);
                            })();
                        </script>
                        <div class="wabtn">
                        <a href="whatsapp://send?text=<%=("Estou participando do BigBallz, o bolão da Copa do Mundo. Venha participar também: " + Url.SiteRoot()).UrlEncode()%>" class="wa_btn wa_btn_s" target="_top">Compartilhar</a>
                        </div>
                    </p>
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
                    <h2>Entenda as regras:</h2>
                </div>
                <div class="col-lg-6">
                    <ul class="list-inline banner-social-buttons">
                        <li><a href="<%=Url.Action("rules", "home") %>" class="btn btn-default btn-lg"><i class="fa fa-book fa-fw"></i> <span class="network-name">Regulamento</span></a></li>
                    </ul>
                </div>
            </div>

        </div>
        <!-- /.container -->

    </div>
    <!-- /.banner -->
</asp:Content>