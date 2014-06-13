<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<BigBallz.ViewBonusViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Editar Bonus</h2>

    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>
        
        <fieldset>
            <legend>Bonus</legend>      
        
            <%=Html.HiddenFor(x => x.Bonus.BonusId) %>

            <div class="editor-label">
               <label for="Date">Bonus:</label>
            </div>
            <div class="editor-field">
                <%= Html.TextBoxFor(model => model.Bonus.Name, new { disabled = "disabled"}) %>
                <%= Html.ValidationMessageFor(model => model.Bonus.Name, "Favor informar o nome do Bonus.")%>             
            </div>
            
            <div class="editor-label">
               <label for="Date">Seleção:</label>
            </div>
            <div class="editor-field">
                <%= Html.DropDownListFor(model => model.Bonus.Team, Model.Teams, "--- Selecione ---")%>
                <%= Html.ValidationMessageFor(model => model.Bonus.Team, "Favor selecionar o time.")%>
            </div>
            <p>
                <input type="submit" value="Salvar" />
            </p>
        </fieldset>

    <% } %>

    <div>
        <%: Html.ActionLink("Voltar a lista", "Index") %>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
</asp:Content>

