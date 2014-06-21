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

        var now = ko.observable(new Date());
        setInterval(function () { now(new Date()); }, 1000);

        var model = function (e) {
            var dateObj = moment($(e).attr("data-expirationdate"));

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
    }
};

$(document).ready(function () {
    BigBallz.Bet.init();
});