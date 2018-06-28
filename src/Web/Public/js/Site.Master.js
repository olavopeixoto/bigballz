(function () {
    function hashCode(s) {
        return s.split("").reduce(function(a, b) {
                a = ((a << 5) - a) + b.charCodeAt(0);
                return a & a;
            },
            0);
    };

    var bigBallz = {
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
            },
            notify: function (title, message) {
                // Check for notification compatibility.
                if (!'Notification' in window) {
                    // If the browser version is unsupported, remain silent.
                    return;
                }
                // Log current permission level
                console.log(Notification.permission);
                // If the user has not been asked to grant or deny notifications
                // from this domain...
                if (Notification.permission === 'default') {
                    Notification.requestPermission(function () {
                        // ...callback this function once a permission level has been set.
                        notify();
                    });
                }
                // If the user has granted permission for this domain to send notifications...
                else if (Notification.permission === 'granted') {

                    var n = new Notification(title,
                        {
                            'body': message,
                            'tag': hashCode(title + '|#|' + message)
                        });

                    // Remove the notification from Notification Center when clicked.
                    n.onclick = function () {
                        this.close();
                    };
                    // Callback function when the notification is closed.
                    n.onclose = function () {
                        console.log('Notification closed');
                    };
                }
                // If the user does not want notifications to come from this domain...
                else if (Notification.permission === 'denied') {
                    // ...remain silent.
                    return;
                }
            }
        }
    };

    $(document).ready(function () {
        bigBallz.SiteMaster.setDefaults();
    });

    window.BigBallz = bigBallz;
})();