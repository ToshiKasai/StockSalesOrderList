(function (factory) {
    if (typeof define === 'function' && define.amd) {
        // AMD. Register as anonymous module.
        define('datepicker.ja', ['jquery'], factory);
    } else if (typeof exports === 'object') {
        // Node / CommonJS
        factory(require('jquery'));
    } else {
        // Browser globals.
        factory(jQuery);
    }
})(function ($) {

    'use strict';

    $.fn.datepicker.languages['ja'] = {
        format: 'yyyy/mm/dd',
        days: ['“ú—j', 'Œ—j', '‰Î—j', '…—j', '–Ø—j', '‹à—j', '“y—j'],
        daysShort: ['“ú', 'Œ', '‰Î', '…', '–Ø', '‹à', '“y'],
        daysMin: ['“ú', 'Œ', '‰Î', '…', '–Ø', '‹à', '“y'],
        months: ['–rŒ', '”@Œ', '–í¶', '‰KŒ', 'HŒ', '…–³Œ', '•¶Œ', '—tŒ', '’·Œ', '_–³Œ', '‘šŒ', 't‘–'],
        monthsShort: ['1Œ', '2Œ', '3Œ', '4Œ', '5Œ', '6Œ', '7Œ', '8Œ', '9Œ', '10Œ', '11Œ', '12Œ'],
        weekStart: 1,
        startView: 0,
        yearFirst: true,
        yearSuffix: '/'
    };
});

