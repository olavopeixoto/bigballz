<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BigBallz.ViewModels.BetViewModel>" %>
<table class="match-table">
<tbody>
<%var index = 0; var lineIndex = 0; %>
<%foreach (var bonus in Model.BonusList){%>
    <tr class="<%= lineIndex%2==0 ? "ui-state-default": "odd"%>">
        <td class="c"><%=bonus.Bonus.Name%></td>
        <td class="c"><%=Html.DropDownList("foo", bonus.Teams, "--- Selecione ---", new {disabled="disabled"})%></td>
    </tr>        
<%index++; lineIndex++;} %>
</tbody>
</table>