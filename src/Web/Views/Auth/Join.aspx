<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="BigBallz.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
- Inscrição
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Inscreva-se</h2>

    <p>
        Para se inscrever no Bolão <strong>BigBallz</strong> você deverá usar uma conta do <a href="https://www.google.com/accounts/NewAccount">Google</a>, <a href="https://edit.yahoo.com/registration?.src=fpctx&.intl=br&.done=http://br.yahoo.com/">Yahoo</a>, <a href="https://login.live.com/">Hotmail</a>
       <a href="http://www.facebook.com/">Facebook</a> ou do <a href="http://www.twitter.com/">Twitter</a>.
       Se não possuir conta em nenhum desses provedores você ainda pode usar a sua conta de qualquer provedor de <a href="http://openid.net/get-an-openid/">OpenID</a>.
    </p>
    <%
    var host = Url.Encode(Url.Action("handleresponse", "auth", null, FormsAuthentication.RequireSSL ? "https" : "http"));
    %>
    <iframe src="https://bigballz.rpxnow.com/openid/embed?token_url=<%=host %>&language_preference=pt-BR&flags=show_provider_list" scrolling="no" frameBorder="no" allowtransparency="true" style="width:400px;height:240px"></iframe>
    <p>
        Feito o login você deverá pagar a taxa de inscrição de <strong><%=Html.Price().ToMoney() %></strong> conforme o <%=Html.ActionLink("regulamento", "Rules", "Home") %>, para ter acesso aos recursos do jogo.
        O pagamento é feito através do <a href="https://pagseguro.uol.com.br/">PagSeguro</a>.
    </p>
    <p>
        A autorização para o jogo será dada assim que for confirmado o pagamento pelo <a href="https://pagseguro.uol.com.br/">PagSeguro</a>, o que poderá levar alguns dias.
    </p>
    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
</asp:Content>