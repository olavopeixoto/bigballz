<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IList<BigBallz.Models.User>>" %>
<%@ Import Namespace="BigBallz.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Distribuição da arrecadação</h2>
    <div id="group-stage">
        <p>
        A arrecadação do BigBallz é feita através de pagamento pelo PagSeguro ou de depósito direto em conta.<br/>
        Para depósito direto em conta não há nenhum tipo de desconto e o dinheiro é revertido integralmente para o prêmio.<br/>
        No caso do PagSeguro, o serviço cobra uma taxa de 3,99% + R$0,40 por transação. <a href="https://pagseguro.uol.com.br/para-seu-negocio/online/">Mais detalhes</a>.<br/>
        Confira na tabela abaixo como está distribuída a arrecadação.
        </p>
        <table class="match-table">
            <thead class="ui-widget-header">
                <tr>
                    <th>
                        Tipo
                    </th>
                    <th class="homeTeam">
                        Qtd Jogadores
                    </th>
                    <th class="homeTeam">
                        Bruto
                    </th>
                    <th class="homeTeam">
                        Líquido
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr class="odd">
                    <td class="l">
                        PagSeguro
                    </td>
                    <td class="c">
                        <%=Model.Count(x => x.PagSeguro) %>
                    </td>
                    <td class="c">
                        <%=(Html.Price() * Model.Count(x => x.PagSeguro)).ToMoney() %>
                    </td>
                    <td class="c">
                        <%=(Html.PriceNet() * Model.Count(x => x.PagSeguro)).ToMoney() %>
                    </td>
                </tr>
                <tr class="odd">
                    <td class="l">
                        Depósito
                    </td>
                    <td class="c">
                        <%=Model.Count(x => !x.PagSeguro) %>
                    </td>
                    <td class="c">
                        <%=(Html.Price() * Model.Count(x => !x.PagSeguro)).ToMoney() %>
                    </td>
                    <td class="c">
                        <%=(Html.Price() * Model.Count(x => !x.PagSeguro)).ToMoney() %>
                    </td>
                </tr>
            </tbody>
            <tfoot>
                <tr class="ui-state-default">
                    <td class="l">Total Arrecadado</td>
                    <td class="c homeTeam"><%=Model.Count() %></td>
                    <td class="c homeTeam"><%=(Html.Price() * Model.Count()).ToMoney() %></td>
                    <td class="c homeTeam"><%=(Html.PriceNet() * Model.Count(x => x.PagSeguro) + Html.Price() * Model.Count(x => !x.PagSeguro)).ToMoney() %></td>
                </tr>
            </tfoot>
        </table>
    </div>
</asp:Content>