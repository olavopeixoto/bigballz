<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="BigBallz.Helpers" %>
<ul>
    <li><%=Html.ActionActiveLink("BigBallz", "index", "home", null, new {title="BigBallz"})%></li>
    <li><%=Html.ActionActiveLink("Regulamento", "rules", "home", null, new { title = "Regulamento" })%></li>
    
    <%if (Request.IsAuthenticated) {%>

    <li><%=Html.ActionActiveLink("Comentários", "index", "comments", null, new { title = "Mensagens BigBallz" })%></li>
    <li><%=Html.ActionActiveLink("Apostas", "index", "bet", null, new { title = "Apostas" })%></li>
                
        <%if (Html.IsAuthorized()){%>
        
        <li><%=Html.ActionActiveLink("Ranking", "index", "standings", null, new { title = "Ranking" })%></li>
        <%--<li><%=Html.ActionActiveLink("Pesquisa", "poll", "home", null, new { title = "Pesquisa" })%></li>--%>
        <%} %>

        <%if (Html.IsAdmin()) {%>
        <li><%=Html.ActionActiveLink("Bonus", "index", "bonus", null, new { title = "Bonus" })%></li>
        <li><%=Html.ActionActiveLink("Jogos", "index", "match", null, new { title = "Jogos" })%></li>
        <li><%=Html.ActionActiveLink("Usuários", "index", "user", null, new { title = "Usuários" })%></li>
        <li><%=Html.ActionActiveLink("Tarefas", "index", "cronjobs", null, new { title = "Tarefas Agendadas" })%></li>        
        <%}%>
    
    <%} else {%>

    <li><%=Html.ActionActiveLink("Inscrição", "join", "auth", null, new { title = "Inscrição" })%></li>
    
    <%}%>
</ul>