<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<BigBallz.Models.User>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <table class="match-table">
        <thead class="ui-widget-header">
            <tr>
                <th>
                </th>
                <th>
                    Nome
                </th>
                <th>
                    Email
                </th>
                <th>
                    Autorizado
                </th>
            </tr>
        </thead>
        <tbody>
            <%
                var lineIndex = 0;
                foreach (var item in Model)
                { %>
            <tr class="<%= lineIndex%2==0 ? "ui-state-default": "odd"%>">
                <td class="c">
                    <%: Html.ActionLink("Editar", "Edit", new { id=item.UserId }) %>
                </td>
                <td class="c">
                    <%: item.UserName %>
                </td>
                <td class="c">
                    <%: item.EmailAddress %>
                </td>
                <td class="c">
                    <%: Html.CheckBox("autorizado", item.Authorized, new { disabled = "disabled" }) %>
                </td>
            </tr>
            <%
                    lineIndex++;
                } %>
        </tbody>
    </table>
    <%-- <p>
        <%: Html.ActionLink("Create New", "Create") %>
    </p>--%>
     <div class="editor-label">
                <%: Html.Label("Total de Usuários: " + ViewData["TotalUsuarios"])%>
     </div>

    <%--<%:Html.ActionLink("Enviar Notificação de Registro", "sendnotification", "user") %>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
</asp:Content>
