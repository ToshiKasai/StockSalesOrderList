/// <reference path="jquery/jquery.d.ts" />

interface JQueryStatic {
    material: Material.Material;
    format: dateFormat.format;
}

interface JQuery {
    // tooltip: Function;
    modal: Function;
    // datepicker: Function;
}

declare namespace dateFormat {
    interface format {
        date(value, format): any;
    }
}

declare namespace Material {
    interface Material {
        init(options?): Function;
    }
}
