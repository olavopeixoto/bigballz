<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage" MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Import Namespace="BigBallz.Core" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent">- Taxa de Inscrição</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="MainContent">
<h2>Pagamento</h2>
<p>
Você está registrado no BigBallz mas ainda não registramos o pagamento da inscrição no valor de <%=ConfigurationHelper.Price.ToMoney()%>.
</p>
<p>
Para faze-lo basta clicar no botão do Pagseguro abaixo:
</p>
<p class="c">
<%Html.RenderPartial("PagSeguro"); %>
</p>
</asp:Content>
<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="Scripts"></asp:Content>