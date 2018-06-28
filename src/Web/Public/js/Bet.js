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
                $("form").submit(function () { window.onbeforeunload = null; $("#apostar input").attr("disabled", "disabled").val("Aguarde...").attr("type", "button"); });
            }
        });

        var now = ko.observable(moment.utc());
        setInterval(function () { now(moment.utc()); }, 1000);

        var model = function (e) {
            var dateObj = moment.utc($(e).attr("data-expirationdate"));

            this.expired = ko.computed(function() {
                return dateObj.isBefore(now());
            });
            this.expirationDate = ko.computed(function () {
                return dateObj.from(now(), true);
            });
        };

        $(".reminder").each(function (index, e) {
            ko.applyBindings(new model(e), e);
        });

        var oldBets = $("table.collapse");
        oldBets.hide();

        var moreBtn = $("button.btn-showhide");
        moreBtn.click(function() {
            moreBtn.hide();
            oldBets.show('blind', 500);
        });
        if (oldBets.length > 0) {
            moreBtn.show();
        }
    }
};

$(document).ready(function () {
    BigBallz.Bet.init();
});