/// <reference path="../typings/tsd.d.ts" />

module logView {
    "use strict";

    $(document).ready(function () {
        $.datepicker.setDefaults($.datepicker.regional['ja']);
        $("[data-toggle='datepicker']").datepicker(
            {
                changeYear: true,
                changeMonth: true,
                closeText: "閉じる",
                currentText: "今日",
                showButtonPanel: true,
                beforeShowDay: function (day: Date): any {
                    var result: any;
                    switch (day.getDay()) {
                        case 0: // 日曜日か？
                            result = [true, "date-sunday"];
                            break;
                        case 6: // 土曜日か？
                            result = [true, "date-saturday"];
                            break;
                        default:
                            result = [true, ""];
                            break;
                    }
                    return result;
                }
            }
        );
        $.material.init();
    });
}
