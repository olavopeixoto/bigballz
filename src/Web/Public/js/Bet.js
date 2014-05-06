BigBallz.Bet = {
    init: function () {
        var betInput = $("td.mResult input.bet-score-value");
        var valueBuffer;
        var dirty = false;
        betInput.focus(function () { if (!dirty) valueBuffer = $(this).val(); });
        betInput.blur(function () {
            if (!dirty && valueBuffer != $(this).val()) {
                dirty = true;
                window.onbeforeunload = function () { return "Existem apostas modificadas que não foram salvas, deseja sair sem salvar?"; };
                $("#apostar").removeClass("ui-helper-hidden");
                $("input[type=submit]").click(function () { window.onbeforeunload = null; });
            }
        });
    }
};

$(document).ready(function () {
    BigBallz.Bet.init();
});