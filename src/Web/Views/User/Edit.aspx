<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<BigBallz.Models.User>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Administração de Usuário</h2>

    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>
        
        <fieldset>
            <legend>Fields</legend>            
          
             <%=Html.HiddenFor(x => x.UserId) %>

            <div class="editor-label">
                <%: Html.Label("Nome") %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.UserName, new { disabled = "disabled" })%>
                <%: Html.ValidationMessageFor(model => model.UserName) %>
            </div>
            
            <div class="editor-label">
                 <%: Html.Label("Email") %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.EmailAddress, new { disabled = "disabled" })%>
                <%: Html.ValidationMessageFor(model => model.EmailAddress) %>
            </div>      
                    
            <div class="editor-label">
                 <%: Html.Label("Autorizado") %>
            </div>
            <div class="editor-field">
                <%: Html.CheckBoxFor(model => model.Authorized) %>
                <%: Html.ValidationMessageFor(model => model.Authorized) %>
            </div>
            
            <div class="editor-label">
                 <%: Html.Label("Administrador") %>
            </div>
            <div class="editor-field">
                <%: Html.CheckBoxFor(model => model.IsAdmin) %>
                <%: Html.ValidationMessageFor(model => model.IsAdmin) %>
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