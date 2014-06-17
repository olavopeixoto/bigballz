<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<BigBallz.ViewModels.MatchViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Cadastro de Jogo</h2>
    <% using (Html.BeginForm())
       {%>
    <fieldset>
        <legend>Dados</legend>
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
            <%= Html.DropDownListFor(model => model.Match.Stage, Model.Stages, "--- Selecione ---")%>
            <%= Html.ValidationMessageFor(model => model.Match.Stage, "Favor selecionar a fase.")%>
        </div>
        <div class="editor-label">
            <label for="Team1">
                Jogo:</label>
        </div>
        <div class="editor-field">
            <%= Html.DropDownListFor(model => model.Match.Team1, Model.Teams, "--- Selecione ---")%>
            X
            <%= Html.ValidationMessageFor(model => model.Match.Team1, "Favor selecionar o time A.")%>
            <%= Html.DropDownListFor(model => model.Match.Team2, Model.Teams, "--- Selecione ---")%>
            <%= Html.ValidationMessageFor(model => model.Match.Team2, "Favor selecionar o time B diferente do time A.")%>
        </div>
        <%--    <div class="editor-label">
            <label for="Team1">
                Time B:</label>
        </div>
        <div class="editor-field">
        </div>--%>
        <p>
            <input type="submit" value="Salvar" />
        </p>
    </fieldset>
    <% } %>
    <div>
        <%=Html.ActionLink("Voltar a lista", "Index") %>
    </div>
</asp:Content>
