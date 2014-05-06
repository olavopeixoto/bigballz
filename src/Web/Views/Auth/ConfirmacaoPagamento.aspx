<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent"></asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="MainContent">
<h2>Pagamento Concluído</h2>
<p>
Seu pagamento foi concluído com sucesso! Uma mensagem com os detalhes desta transação foi enviada para o seu e-mail. Você também poderá acessar sua conta PagSeguro no endereço <a href="https://pagseguro.uol.com.br/">https://pagseguro.uol.com.br/</a> para mais informações.
</p>
</asp:Content>
<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="Scripts"></asp:Content>