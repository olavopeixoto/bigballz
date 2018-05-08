<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<BigBallz.ViewModels.MatchViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Editar</h2>
    <% using (Html.BeginForm())
       {%>
    <fieldset>
        <legend>Dados</legend>
        <%=Html.HiddenFor(x => x.Match.MatchId) %>
        <div class="editor-label">
            <label for="Date">
                Data:</label>
        </div>
        <div class="editor-field">
            <%= Html.TextBoxFor(model => model.Match.StartTime) %>
            <%= Html.ValidationMessageFor(model => model.Match.StartTime)%>
        </div>
        <div class="editor-label">
            <label for="Stage">
                Fase:</label>
        </div>
        <div class="editor-field">
            <%= Html.DropDownListFor(model => model.Match.StageId, Model.Stages, "--- Selecione ---")%>
            <%= Html.ValidationMessageFor(model => model.Match.StageId, "Favor selecionar a fase.")%>
        </div>
        <div class="editor-label">
            <label for="Team">
                Jogo:</label>
        </div>
        <div class="editor-field">
            <%= Html.DropDownListFor(model => model.Match.Team1Id, Model.Teams, "--- Selecione ---")%>
            <%= Html.ValidationMessageFor(model => model.Match.Team1Id, "Favor selecionar o time A.")%>
            <%= Html.TextBoxFor(model => model.Match.Score1, new { @class = "numbersonly bet-score-value score1", maxlength = "2", size = "2" }) %>
            <%= Html.ValidationMessageFor(model => model.Match.Score1, "Favor informar um número inteiro maior ou igual a zero.")%>
            X
            <%= Html.TextBoxFor(model => model.Match.Score2, new { @class = "numbersonly bet-score-value score2", maxlength = "2", size = "2" })%>
            <%= Html.ValidationMessageFor(model => model.Match.Score2, "Favor informar um número inteiro maior ou igual a zero.")%>
            <%= Html.DropDownListFor(model => model.Match.Team2Id, Model.Teams, "--- Selecione ---")%>
            <%= Html.ValidationMessageFor(model => model.Match.Team2Id, "Favor selecionar o time B diferente do time A.")%>
        </div>
        <p>
            <input type="submit" value="Salvar" />
        </p>
    </fieldset>
    <% } %>
    <div>
        <%=Html.ActionLink("Voltar a lista", "Index") %>
    </div>
</asp:Content>