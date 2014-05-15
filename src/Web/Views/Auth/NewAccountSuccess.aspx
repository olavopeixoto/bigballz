<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Import Namespace="BigBallz.Core" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent">- Taxa de Inscrição</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="MainContent">
<h2>Registro Efetuado Com Sucesso</h2>
<p>
Obrigado por escolher o BigBallz!
</p>
<p>
Seu registro foi efetuado com sucesso, e está associado a sua conta do <strong><%:ViewData["nomeProvedor"]%></strong>.
</p>
<p>
Use sempre esta mesma conta para entrar no sistema.
</p>
<p>
Para finalizar o processo e garantir o seu ingresso no jogo é preciso pagar a taxa de inscrição no valor de <strong><%=Html.Price().ToMoney() %></strong>.
</p>
<p>
A taxa deve ser paga através do PagSeguro.
</p>
<p>
Feito o pagamento você deverá aguardar a confirmação do pagamento por parte do pagseguro, o que pode levar alguns dias.
</p>
<p class="c">
<%Html.RenderPartial("pagseguro"); %>
</p>
</asp:Content>
<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="Scripts"></asp:Content>