<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="BigBallz.Helpers" %>
<ul>
    <li><%=Html.ActionActiveLink("BigBallz", "index", "home", null, new {title="BigBallz"})%></li>
    <li><%=Html.ActionActiveLink("Regulamento", "rules", "home", null, new { title = "Regulamento" })%></li>
    
    <%if (Request.IsAuthenticated) {%>

        <li><%=Html.ActionActiveLink("Coment�rios", "index", "comments", null, new { title = "Mensagens BigBallz" })%></li>
        <li><%=Html.ActionActiveLink("Apostas", "index", "bet", null, new { title = "Apostas" })%></li>
                
        <li><%=Html.ActionActiveLink("Ranking", "index", "standings", null, new { title = "Ranking" })%></li>

        <%if (Html.IsAdmin()) {%>
            <li><%=Html.ActionActiveLink("Bonus", "index", "bonus", null, new { title = "Bonus" })%></li>
            <li><%=Html.ActionActiveLink("Jogos", "index", "match", null, new { title = "Jogos" })%></li>
            <li><%=Html.ActionActiveLink("Usu�rios", "index", "user", null, new { title = "Usu�rios" })%></li>
            <li><%=Html.ActionActiveLink("Tarefas", "index", "cronjobs", null, new { title = "Tarefas Agendadas" })%></li>        
        <%}%>
    
    <%} else {%>

        <li><%=Html.ActionActiveLink("Inscri��o", "join", "auth", null, new { title = "Inscri��o" })%></li>
    
    <%}%>
</ul>