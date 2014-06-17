<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<BigBallz.Models.Bonus>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <br />
    <table class="match-table" style="text-align:center;">
        <thead class="ui-widget-header">
        <tr>       
            <th class="table.th.admin"></th>
            <th class="table.th.admin">
                Bonus
            </th>
            <th class="table.th.admin">
                Seleção
            </th>
        </tr>
        </thead>
        <tbody>
        <% 
            var lineIndex = 0; foreach (var item in Model)
           { %>
         <tr class="<%= lineIndex%2==0 ? "ui-state-default": "odd"%>">
            <td class="table.td.admin">
                <%: Html.ActionLink("Editar", "Edit", new { id=item.BonusId }) %>
            </td>
            <td class="table.td.admin">
                <%: item.Name %>
            </td>
            <td class="table.td.admin">
                <%if (item.Team != null)
{%> <%:item.Team.Name%> <%
}%>
            </td>
        </tr>
        <%
               lineIndex++;
           } %>
            </tbody>
    </table>
    <p>
        <%: Html.ActionLink("Enviar Email Apostas", "SendBonusEmail") %>
    </p>
      
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
</asp:Content>
