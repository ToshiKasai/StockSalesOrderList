/// <reference path="../typings/tsd.d.ts" />
var logView;
(function (logView) {
    "use strict";
    $(document).ready(function () {
        $.datepicker.setDefaults($.datepicker.regional['ja']);
        $("[data-toggle='datepicker']").datepicker({
            changeYear: true,
            changeMonth: true,
            closeText: "閉じる",
            currentText: "今日",
            showButtonPanel: true,
            beforeShowDay: function (day) {
                var result;
                switch (day.getDay()) {
                    case 0:
                        result = [true, "date-sunday"];
                        break;
                    case 6:
                        result = [true, "date-saturday"];
                        break;
                    default:
                        result = [true, ""];
                        break;
                }
                return result;
            }
        });
        $.material.init();
    });
})(logView || (logView = {}));
//# sourceMappingURL=logView.js.map