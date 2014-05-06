<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="BigBallz.Helpers" %>
<p>
    <%=Html.Image("bolao_paises.png", new { alt = "BigBallz", style = "float:left;width:165px;height:165px;padding-right:10px" })%>
    Bem-vindo ao <strong>BigBallz</strong>, o Bolão da Copa do Mundo!
</p>
<p>
    Já virou tradição, o BigBallz existe desde a copa de 2002.
    Fez cada vez mais sucesso em cada uma das edições e esse ano não vai ser diferente.
</p>
<p>
    Tudo começou com um grupo de amigos da faculdade que reuniram os seus próprios amigos e amigos dos amigos para fazer um bolão.
    Eles cursavam engenharia de computação e dessa forma não podia mesmo ser diferente, o bolão virou um site.
    Todos as apostas e acompanhamentos dos resultados eram feitos online através desta aplicação.
    A cada edição o site foi evoluindo ainda mais, até chegar ao que é hoje.
</p>
<%if (!Request.IsAuthenticated) {%>
<p style="text-align:center">
    <%=Html.ActionLink("Inscreva-se", "Join", "Auth", null, new { @class = "btn-join" })%>
</p>
<%} else if (!Html.IsAuthorized()) {%>
    <p>Você já está inscrito no bolão, no entando o seu pagamento ainda não foi registrado. Se você já efetuou o pagamento aguarde a sua autorização, se não o fez, poderá faze-lo online pelo PagSeguro.</p>
    <p>
<%Html.RenderPartial("PagSeguro", Model); %>
</p>
<%}%>