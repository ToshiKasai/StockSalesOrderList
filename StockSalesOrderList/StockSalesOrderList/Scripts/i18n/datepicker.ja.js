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
        days: ['���j', '���j', '�Ηj', '���j', '�ؗj', '���j', '�y�j'],
        daysShort: ['��', '��', '��', '��', '��', '��', '�y'],
        daysMin: ['��', '��', '��', '��', '��', '��', '�y'],
        months: ['�r��', '�@��', '�퐶', '�K��', '�H��', '������', '����', '�t��', '����', '�_����', '����', '�t��'],
        monthsShort: ['1��', '2��', '3��', '4��', '5��', '6��', '7��', '8��', '9��', '10��', '11��', '12��'],
        weekStart: 1,
        startView: 0,
        yearFirst: true,
        yearSuffix: '/'
    };
});

