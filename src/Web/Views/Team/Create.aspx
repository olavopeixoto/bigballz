<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<BigBallz.ViewModels.TeamViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Create
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Create</h2>
    <% using (Html.BeginForm())
       {%>
    <fieldset>  
        <legend>Fields</legend>
        <%--<%= Html.ValidationSummary("Please correct the errors and try again.") %>--%>
        <div class="editor-label">
            <label for="Name">Id:</label>
        </div>
        <div class="editor-field">
            <%= Html.TextBoxFor(model => model.Team.TeamId) %>
            <%= Html.ValidationMessageFor(model => model.Team.TeamId)%>
        </div>
        <div class="editor-label">
            <label for="Name">Nome:</label>
        </div>
        <div class="editor-field">
            <%= Html.TextBoxFor(model => model.Team.Name) %>
            <%= Html.ValidationMessageFor(model => model.Team.Name)%>
        </div>
        <div class="editor-label">
            <label for="Group">Id da Fifa:</label>
        </div>
        <div class="editor-field">
            <%= Html.TextBoxFor(model => model.Team.FifaId) %>
            <%= Html.ValidationMessageFor(model => model.Team.FifaId)%>
        </div>
        <div class="editor-label">
            <label for="Group">Grupo:</label>
        </div>
        <div class="editor-field">
            <%= Html.DropDownListFor(model => model.Team.GroupId, Model.Groups, "--- Selecione ---")%>
            <%= Html.ValidationMessageFor(model => model.Team.GroupId, "Favor selecionar o grupo.")%>
            
        </div>
        <p>
            <input type="submit" value="Create" />
        </p>
    </fieldset>
    <% } %>
    <div>
        <%=Html.ActionLink("Back to List", "Index") %>
    </div>
</asp:Content>
