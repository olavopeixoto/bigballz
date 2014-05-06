$(document).ready(function() {
    BigBallz.SiteMaster.setDefaults();
});

var BigBallz = {
    Team: {
        BigBallz: {}
    },
    Home: {
        BigBallz: {}
    },
    Bet: {
        BigBallz: {}
    },
    SiteMaster: {
        setDefaults: function () {
            $("#tabs").tabs();
            $("button, input:submit").button();
            $(".datepicker").datepicker();
            $("input.numbersonly").keydown(function (event) {
                // Allow only backspace, delete and tab
                if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9) {
                    // let it happen, don't do anything
                }
                else {
                    // Ensure that it is a number and stop the keypress
                    if (event.keyCode < 48 || (event.keyCode > 57 && event.keyCode < 96) || event.keyCode > 105) {
                        event.preventDefault();
                    }
                }
            });
        }
    }
};