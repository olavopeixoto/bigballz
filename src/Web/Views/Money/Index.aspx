<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<BigBallz.Models.MoneyDistribution>>" %>
<%@ Import Namespace="BigBallz.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Distribuição da arrecadação</h2>
    <div id="group-stage">
        <table class="match-table">
            <thead class="ui-widget-header">
                <tr>
                    <th>
                        Custodiante
                    </th>
                    <th>
                        Quantia
                    </th>
                    <th>
                        Qtd Jogadores
                    </th>
                </tr>
            </thead>
            <tbody>
                <%var i = 0;
                  foreach (var item in Model) {%>
                <tr class="<%= i%2==0 ? "ui-state-default": "odd"%>">
                    <td class="l">
                        <%= item.Holder%>
                    </td>
                    <td class="l">
                        <%= item.Amount.ToMoney()%>
                    </td>
                    <td class="c">
                        <%= item.TotalPlayers%>
                    </td>
                </tr>
                <%i++;
              } %>
            </tbody>
        </table>
    </div>
</asp:Content>