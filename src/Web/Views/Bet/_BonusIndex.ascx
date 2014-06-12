<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BigBallz.ViewModels.BetViewModel>" %>
<% using (Html.BeginForm("savebonus", "bet")){%>
<table class="match-table">
<tbody>
<%var index = 0; var lineIndex = 0; %>
<%foreach (var bonus in Model.BonusList){%>
    <tr class="<%= lineIndex%2==0 ? "ui-state-default": "odd"%>">
        <td class="c"><%=Html.Hidden("bonusBet[" + index + "].BonusBetId", bonus.BonusBetId)%><%=Html.Hidden("bonusBet[" + index + "].Bonus", bonus.Bonus.BonusId)%><%=bonus.Bonus.Name%></td>
        <td class="c"><%=Html.DropDownList("bonusBet[" + index + "].Team", bonus.Teams, "--- Selecione ---")%></td>   
        <%--<td class="l reminder"><%=Html.BonusReminder(bonus.CupStartDate, bonus.PointsEarned)%></td>--%>
         
    </tr>        
<%index++; lineIndex++;} %>
</tbody>
</table>
<div id="Save" style="text-align:center;">
    <input type="submit" value="Apostar" />
</div>
<%} %>