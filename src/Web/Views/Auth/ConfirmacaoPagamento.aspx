<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<BigBallz.Models.PagSeguroTransactionStatus>" MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Import Namespace="BigBallz.Core" %>
<%@ Import Namespace="BigBallz.Models" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent"></asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="MainContent">
<h2><%=Model.ToString().FromCamelCase()%></h2>
    
<% if (Model == PagSeguroTransactionStatus.AguardandoPagamento || Model == PagSeguroTransactionStatus.EmAnalise)
   { %>
<p>
Estamos aguardando a confirmação do pagamento por parte da instituição financeira. Assim que for autorizado, você receberá um e-mail confirmando a transação e liberando o seu acesso ao BigBallz.
</p>
<% } else { %>
    Seu pagamento não obteve sucesso. A resposta do PagSeguro foi: <b><i><%=Model.Description() %></i></b><br/>
    Por favor, verifique os seus dados e tente novamente, possivelmente usando outra forma de pagamento.
    <%Html.RenderPartial("PagSeguro"); %>
<% } %>
</asp:Content>
<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="Scripts"></asp:Content>