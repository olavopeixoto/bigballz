<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Import Namespace="BigBallz.Core" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent">- Taxa de Inscri��o</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="MainContent">
<h2>Registro Efetuado Com Sucesso</h2>
<p>
Obrigado por escolher o BigBallz!
</p>
<p>
Seu registro foi efetuado com sucesso, e est� associado a sua conta do <strong><%:ViewData["nomeProvedor"]%></strong>.
</p>
<p>
Use sempre esta mesma conta para entrar no sistema.
</p>
<p>
Para finalizar o processo e garantir o seu ingresso no jogo � preciso pagar a taxa de inscri��o no valor de <strong><%=Html.Price().ToMoney() %></strong>.
</p>
<p>
A taxa deve ser paga atrav�s do PagSeguro.
</p>
<p>
Feito o pagamento voc� dever� aguardar a confirma��o do pagamento por parte do pagseguro, o que pode levar alguns dias.
</p>
<p class="c">
<%Html.RenderPartial("pagseguro"); %>
</p>
</asp:Content>
<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="Scripts"></asp:Content>