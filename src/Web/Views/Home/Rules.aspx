<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="BigBallz.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <b>1. CRITÉRIO DE PAGAMENTO</b><br />
    <br />
    - A taxa de inscrição é de <b><%=Html.Price().ToMoney()%></b> e deverá ser paga através do PagSeguro. O Cadastro pode ser feito a qualquer momento, no entanto,
	é bom observar o prazo final das apostas Bonus que é até 23:59 do dia <%=ViewData["StartDate"] %>. Após essa data suas chances se reduzem significativamente.
    <br />
    <br />
    <b>2. CRITÉRIO DE APOSTAS</b>
    <br />
    <br />
    - Cada jogador poderá apostar ou fazer alteração no placar dos jogos até 1 (uma) hora antes do inicio de cada jogo.
    <br />
    Ex.: Jogo 1: 11/06/<%=DateTime.Today.Year %> às 11:00. A partir das 10:00 do dia 11/06/<%=DateTime.Today.Year %> será impossível
    realizar ou alterar apostas para o jogo 1.
    <br />
    <br />
    - Cada jogador somente poderá visualizar as apostas de jogos de outros jogadores
    após o encerramento do período de apostas.<br />
    Ex.: Jogo 1: 11/06/<%=DateTime.Today.Year %> às 11:00. A partir das 10:00 do dia 11/06/<%=DateTime.Today.Year %> será possível
    visualizar todas as apostas neste jogo.<br />
    <br />
    - Cada jogador poderá apostar o BONUS de primeiro colocado de cada grupo até as 23:59 dia
    11/06/<%=DateTime.Today.Year %>.<br />
    <br />
    - Cada jogador poderá apostar o BONUS de campeão, vice-campeão, terceiro e quarto colocado
    da Copa até as 23:59 dia 11/06/<%=DateTime.Today.Year %>.<br />
    <br />
    <br />
    <b>3. CRITÉRIO DE PONTUAÇÃO</b>
    <br />
    <br />
    3.1 Jogos<br />
    <br />
    <table border="1px" cellpadding="3px" cellspacing="3px">
        <tr align="center">
            <th>
                Tipo
            </th>
            <th>
                Fase de Grupos
            </th>
            <th>
                Fase Mata-Mata
            </th>
            <th>
                Exemplo
            </th>
        </tr>
        <tr>
            <td >
                Placar Exato
            </td>
            <td>
                7 pts
            </td>
            <td>
                10 pts
            </td>
            <td>
                Aposta: 1x0 Resultado: 1x0
            </td>
        </tr>
        <tr>
            <td>
                Gols por Equipe + Vencedor
            </td>
            <td>
                3 pts
            </td>
            <td>
                5 pts
            </td>
            <td>
                Aposta: 1x0 Resultado: 2x0
            </td>
        </tr>
        <tr>
            <td>
                Vencedor/Empate
            </td>
            <td>
                2 pts
            </td>
            <td>
                3 pts
            </td>
            <td>
                Aposta: 1x0 / 2x2 Resultado: 2x1 / 1x1
            </td>
        </tr>
        <tr>
            <td>
                Gols por Equipe
            </td>
            <td>
                1 pt
            </td>
            <td>
                2 pts
            </td>
            <td>
                Aposta: 1x0 Resultado: 1x2
            </td>
        </tr>
    </table>
    <br />
    <br />
    3.2 Bonus<br />
    <br />
    <table border="1px" cellpadding="3px" cellspacing="3px">
        <tr align="center">
            <th>
                Tipo
            </th>
            <th>
                Pontuação
            </th>
        </tr>
        <tr>
            <td>
                Campeão de Grupo
            </td>
            <td>
                5 pts
            </td>
        </tr>
        <tr>
            <td>
                Campeão da Copa
            </td>
            <td>
                15 pts
            </td>
        </tr>
        <tr>
            <td>
                Vice-Campeão da Copa
            </td>
            <td>
                10 pts
            </td>
        </tr>
        <tr>
            <td>
                3º Colocado da Copa
            </td>
            <td>
                8 pts
            </td>
        </tr>
        <tr>
            <td>
                4º Colocado da Copa
            </td>
            <td>
                8 pts
            </td>
        </tr>
    </table>
    <br />
    <br />
    <br />
    3.3 Total Máximo de Pontos<br />
    <br />
    <table border="1px" cellpadding="3px" cellspacing="3px">
        <tr align="center">
            <th>
                Tipo
            </th>
            <th>
                Pontuação
            </th>
            <th>
                Percentual
            </th>
        </tr>
        <tr>
            <td>
                Fase de Grupos
            </td>
            <td>
                336 pts
            </td>
            <td>
                58%
            </td>
        </tr>
        <tr>
            <td>
                Fase Mata-Mata
            </td>
            <td>
                160 pts
            </td>
            <td>
                28%
            </td>
        </tr>
        <tr>
            <td>
                Bonus
            </td>
            <td>
                81 pts
            </td>
            <td>
                14%
            </td>
        </tr>
        <tr>
            <td>
                TOTAL
            </td>
            <td>
                577 pts
            </td>
            <td>
                100%
            </td>
        </tr>
    </table>
    <br />
    <br />
    <b>4. CRITÉRIO DE CLASSIFICAÇÃO </b>
    <br />
    <br />
    - O jogador que somar o maior número de pontos durante toda a Copa ganhará o Bolão.
    <br />
    <br />
    Critérios de Desempate:
    <br />
    <br />
    1 - Maior número de placares exato;<br />
    2 - Maior número de pontos em BONUS;<br />
    3 - Sorteio.
    <br />
    <br />
    <b>5. CRITÉRIO DE PREMIAÇÃO</b>
    <br />
    <br />
    - O primeiro colocado do Bolão receberá 65% do total acumulado.
    <br />
    <br />
    - O segundo colocado do Bolão receberá 20% do total acumulado.
    <br />
    <br />
    - O terceiro colocado do Bolão receberá 10% do total acumulado.
    <br />
    <br />
    - O site receberá 5% do total acumulado.
    <br />
    <br />
    
  
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
</asp:Content>