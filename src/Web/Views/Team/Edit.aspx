<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<BigBallz.Models.Team>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit</h2>

    <% using (Html.BeginForm()) {%>

        <fieldset>
            <legend>Fields</legend>
            
        <div class="editor-label">
            <label for="Name">Time:</label>
        </div>
        <div class="editor-field">
            <%= Html.TextBoxFor(model => model.Name) %>
            <%= Html.ValidationMessageFor(model => model.Name)%>
        </div>
        <div class="editor-label">
            <label for="Group">Grupo:</label>
        </div>
        <div class="editor-field">
            <%= Html.DropDownListFor(model => model.GroupId, (SelectList)ViewData["Groups"], "--- Selecione ---")%>
            <%= Html.ValidationMessageFor(model => model.GroupId, "Favor selecionar o grupo.")%>
            
        </div>
            
            <p>
                <input type="submit" value="Save" />
            </p>
        </fieldset>

    <% } %>

    <div>
        <%=Html.ActionLink("Back to List", "Index") %>
    </div>

</asp:Content>

