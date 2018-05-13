﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<BigBallz.Models.User>>" %>
<%@ Import Namespace="BigBallz.Models" %>
<%@ Import Namespace="BigBallz.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <table class="match-table">
        <thead class="ui-widget-header">
            <tr>
                <th>
                    Nome
                </th>
                <th>
                    Email
                </th>
                <th>
                    Autorizado
                </th>
                <th>
                    Admin
                </th>
                <th>
                    PagSeguro
                </th>
                <th>
                    Autorizado Por
                </th>
            </tr>
        </thead>
        <tbody>
            <%
            var lineIndex = 0;
            foreach (var item in Model.OrderByDescending(x => x.Authorized).ThenBy(x => x.UserName))
            { %>
            <tr class="<%= lineIndex%2==0 ? "ui-state-default": "odd"%>">
                <td class="l linebreak">
                    <%: Html.ActionLink(item.UserName, "Edit", new { id=item.UserId }) %>
                </td>
                <td class="l linebreak">
                    <%: item.EmailAddress %>
                </td>
                <td class="c">
                    <%: Html.CheckBox("autorizado", item.Authorized, new { disabled = "disabled" }) %>
                </td>
                <td class="c">
                    <%: Html.CheckBox("administrador", item.UserRoles.Any(x => x.Role.Name.ToLower() == BBRoles.Admin), new { disabled = "disabled" }) %>
                </td>
                <td class="c">
                    <%: Html.CheckBox("pagseguro", item.PagSeguro, new { disabled = "disabled" }) %>
                </td>
                <td class="c linebreak">
                    <%: item.AuthorizedBy %>
                </td>
            </tr>
            <%
                lineIndex++;
            } %>
        </tbody>
    </table>

    <div class="editor-label">
    <%: Html.Label("Total de Jogadores Autorizados: " + ViewData["TotalUsuarios"] + " / " + Model.Count() + " (" + (Convert.ToDecimal(ViewData["TotalUsuarios"])/Convert.ToDecimal(Model.Count())).ToPercent() + ")")%> - <%=Html.ActionLink("Enviar notificação para os não autorizados", "sendnotification", "user") %>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server"></asp:Content>