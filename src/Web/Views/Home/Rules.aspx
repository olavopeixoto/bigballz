<%@ Page Title="" Language="C#" MasterPageFile="Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="BigBallz.Core" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
   
</asp:Content>

<asp:Content ID="FacebookMetadata" ContentPlaceHolderID="FacebookMetadata" runat="server">
    <meta property="og:url"           content="<%=Url.SiteRoot() %>" />
    <meta property="og:type"          content="website" />
    <meta property="og:title"         content="BigBallz" />
    <meta property="og:description"   content="O Bolão da Copa do Mundo" />
    <meta property="og:image"         content="<%=Url.VersionedContent("~/public/images/grana.png") %>" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="content-section-a">

        <div class="container">
        <h1>Regulamento</h1>
                        
    <h3>1. CRITÉRIO DE PAGAMENTO</h3>
    <p>
    A taxa de inscrição é de <strong><%=ConfigurationHelper.Price.ToMoney()%></strong> e deverá ser paga através do PagSeguro. O Cadastro pode ser feito a qualquer momento, no entanto,
    é bom observar o prazo final das apostas Bonus que é até 1 (uma) hora antes do primeiro jogo. Após essa data suas chances se reduzem significativamente.
    </p>

    <h3>2. CRITÉRIO DE APOSTAS</h3>
    <p>
    Cada jogador poderá apostar ou fazer alteração no placar dos jogos até 1 (uma) hora antes do inicio de cada jogo.<br/>
    <em>Ex.: Jogo 1: 14/06/2018 às 12:00. A partir das 11:00 do dia 14/06/2018 será impossível
    realizar ou alterar apostas para o jogo 1.</em>
    </p>
    <p>
    Cada jogador somente poderá visualizar as apostas de jogos de outros jogadores
    após o encerramento do período de apostas.<br/>
    <em>
    Ex.: Jogo 1: 14/06/2018 às 12:00. A partir das 11:00 do dia 14/06/2018 será possível
    visualizar todas as apostas neste jogo.
    </em>
    </p>
    <p>
    Cada jogador poderá apostar o BONUS de primeiro colocado de cada grupo até 1 (uma) hora antes da primeira partida da copa.
    </p>
    <p>
    Cada jogador poderá apostar o BONUS de campeão, vice-campeão, terceiro e quarto colocado
    da Copa até 1 (uma) hora antes da primeira partida da copa.
    </p>
    <p>
      Para o resultado do jogo, será considerado o resultado oficial da fifa. Portanto, para a fase mata mata, o que fica valendo é o resultado final após a prorrogação, quando houver. Não será considerado os gols das disputas de pênaltis.
    </p>

    <h3>3. CRITÉRIO DE PONTUAÇÃO</h3>
    <h4>3.1 Jogos</h4>

    <div class="table-responsive">
    <table class="table table-striped">
        <thead><tr>
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
        </thead>
        <tbody>
        <tr>
            <td>
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
    </tbody></table>
    </div>

    <h4>3.2 Bonus</h4>
    <div class="table-responsive">
    <table class="table table-striped">
        <thead><tr>
            <th>
                Tipo
            </th>
            <th>
                Pontuação
            </th>
        </tr>
        </thead>
        <tbody>
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
                10 pts
            </td>
        </tr>
        <tr>
            <td>
                Vice-Campeão da Copa
            </td>
            <td>
                7 pts
            </td>
        </tr>
        <tr>
            <td>
                3º Colocado da Copa
            </td>
            <td>
                5 pts
            </td>
        </tr>
        <tr>
            <td>
                4º Colocado da Copa
            </td>
            <td>
                5 pts
            </td>
        </tr>
    </tbody></table>
    </div>
    <h4>3.3 Total Máximo de Pontos</h4>
    <div class="table-responsive">
    <table class="table table-striped">
        <thead><tr>
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
        </thead>
        <tbody>
        <tr>
            <td>
                Fase de Grupos
            </td>
            <td>
                336 pts
            </td>
            <td>
                60%
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
                67 pts
            </td>
            <td>
                12%
            </td>
        </tr>
        <tr>
            <td>
                TOTAL
            </td>
            <td>
                563 pts
            </td>
            <td>
                100%
            </td>
        </tr>
    </tbody></table>
    </div>

    <h3>4. CRITÉRIO DE CLASSIFICAÇÃO</h3>
    <p>
    O jogador que somar o maior número de pontos durante toda a Copa ganhará o Bolão.
    </p>
    <h4>
    Critérios de Desempate:
    </h4>
    <ol>
    <li>Maior número de placares exato</li>
    <li>Maior número de pontos em BONUS</li>
    <li>Sorteio</li>
    </ol>
    <h3>5. CRITÉRIO DE PREMIAÇÃO</h3>
    <ul>
    <li>O primeiro colocado do Bolão receberá <%=ConfigurationHelper.PrizeFirstPercentage.ToPercent()%> do total acumulado</li>
    <li>O segundo colocado do Bolão receberá <%=ConfigurationHelper.PrizeSecondPercentage.ToPercent()%> do total acumulado</li>
    <li>O terceiro colocado do Bolão receberá <%=ConfigurationHelper.PrizeThirdPercentage.ToPercent()%> do total acumulado</li>
    <li>O site receberá <%=(1.0M - ConfigurationHelper.PrizeFirstPercentage - ConfigurationHelper.PrizeSecondPercentage - ConfigurationHelper.PrizeThirdPercentage).ToPercent()%> do total acumulado</li>
        <li>O total acumulado é igual ao total arrecadado bruto descontando as <a href="https://pagseguro.uol.com.br/para-seu-negocio/online/">taxas</a> cobradas pelo meio de pagamento (O PagSeguro cobra <%=ConfigurationHelper.PagSeguroPercentageFee.ToPercent() %> + <%=ConfigurationHelper.PagSeguroFixedValueFee.ToMoney() %> por transação)</li>
    </ul>
    </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
</asp:Content>