<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="BigBallz.Core" %>
- Inscreva-se
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Inscreva-se</h2>

    <p>
        Para se inscrever no Bolão <strong>BigBallz</strong> é muito fácil e você pode faze-lo até às <strong>23:59 do dia <%=ViewData["StartDate"]%></strong>.
    </p>
    <p>
       O <strong>BigBallz</strong> não coleta dados de usuário nem senha dos participantes,
       para logar você deverá usar a sua senha atual do seu email do <a href="https://www.google.com/accounts/NewAccount">Google</a>, <a href="https://edit.yahoo.com/registration?.src=fpctx&.intl=br&.done=http://br.yahoo.com/">Yahoo</a> ou <a href="https://login.live.com/">Hotmail</a>
       ou ainda usar o seu login do <a href="http://www.facebook.com/">Facebook</a> ou do <a href="http://www.twitter.com/">Twitter</a>.
       Se não possuir conta em nenhum desses provedores você ainda pode usar a sua conta de qualquer provedor de <a href="http://openid.net/get-an-openid/">OpenID</a>.
    </p>
    <p>
        Provavelmente você já possui conta em alguns desses serviços, neste caso, escolha um dos provedores abaixo e faça o login. Caso contrário, você deverá criar uma conta em algum deles antes de prosseguir no <strong>BigBallz</strong>.
    </p>
    <%
    var host = Url.Encode(Url.Action("handleresponse", "auth", null, FormsAuthentication.RequireSSL ? "https" : "http"));
    %>
    <iframe src="http://bigballz.rpxnow.com/openid/embed?token_url=<%=host %>&language_preference=pt-BR&flags=show_provider_list" scrolling="no" frameBorder="no" allowtransparency="true" style="width:400px;height:240px"></iframe>
    <p>
        Feito o login você deverá pagar a taxa de inscrição de <strong><%=Html.Price().ToMoney() %></strong> conforme o <%=Html.ActionLink("regulamento", "Rules", "Home") %>.
        O pagamento é feito através do <a href="https://pagseguro.uol.com.br/">PagSeguro</a>. Clique neste botão
    <img src="https://p.simg.uol.com.br/out/pagseguro/i/botoes/doacao/btncontribuicao.jpg" /> e no campo para doação digite o valor de <%=Html.Price().ToMoney(false) %>, preencha os dados de identificação (não é necessário ter conta no pagseguro para fazer o pagamento), e escolha a forma de pagamento. Nossa sugestão é transferência online, pois o pagseguro cobra taxa de emissão em caso de Boleto Bancário.
    </p>

    <p>
        Feito o pagamento você deverá aguardar a confirmação por parte dos administradores que irão liberar o seu login para uso.
    </p>
    

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
</asp:Content>