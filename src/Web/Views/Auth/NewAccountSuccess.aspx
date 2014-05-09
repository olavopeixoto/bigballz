<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Import Namespace="BigBallz.Core" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent">- Taxa de Inscrição</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="MainContent">
<h2>Registro Efetuado Com Sucesso</h2>
<p>
Seu registro foi efetuado com sucesso, você já tem uma conta associada ao <strong>BigBallz</strong>.
</p>
<p>
Use sempre a sua conta do <strong><%:ViewData["nomeProvedor"]%></strong> para entrar no sistema.
</p>
<p>
Para finalizar o processo e garantir o seu acesso é preciso pagar a taxa de inscrição.
</p>
<p>
A taxa deve ser paga através do PagSeguro. Clique no botão abaixo e preencha o valor de <strong><%=Html.Price().ToMoney() %></strong> quando for solicitado.
Complete os seus dados e escolha a forma de pagamento. Recomendamos pagar através de transferência online pois por boleto bancário tem cobrança de taxa pelo PagSeguro.
</p>
<p>
Terminado o processo você deverá aguardar a confirmação do pagamento por um dos administradores do site para que seja liberado o seu login.
</p>
<p class="c">
<%Html.RenderPartial("pagseguro"); %>
</p>
</asp:Content>
<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="Scripts"></asp:Content>