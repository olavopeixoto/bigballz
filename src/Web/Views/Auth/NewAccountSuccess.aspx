<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Import Namespace="BigBallz.Core" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent">- Taxa de Inscri��o</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="MainContent">
<h2>Registro Efetuado Com Sucesso</h2>
<p>
Seu registro foi efetuado com sucesso, voc� j� tem uma conta associada ao <strong>BigBallz</strong>.
</p>
<p>
Use sempre a sua conta do <strong><%:ViewData["nomeProvedor"]%></strong> para entrar no sistema.
</p>
<p>
Para finalizar o processo e garantir o seu acesso � preciso pagar a taxa de inscri��o.
</p>
<p>
A taxa deve ser paga atrav�s do PagSeguro. Clique no bot�o abaixo e preencha o valor de <strong><%=Html.Price().ToMoney() %></strong> quando for solicitado.
Complete os seus dados e escolha a forma de pagamento. Recomendamos pagar atrav�s de transfer�ncia online pois por boleto banc�rio tem cobran�a de taxa pelo PagSeguro.
</p>
<p>
Terminado o processo voc� dever� aguardar a confirma��o do pagamento por um dos administradores do site para que seja liberado o seu login.
</p>
<p class="c">
<%Html.RenderPartial("pagseguro", Model); %>
</p>
</asp:Content>
<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="Scripts"></asp:Content>