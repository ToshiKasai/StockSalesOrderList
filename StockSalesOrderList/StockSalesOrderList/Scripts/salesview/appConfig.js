var salesViewConfig;
(function (salesViewConfig) {
    "use strict";
    salesViewConfig.AppName = "myApp";
    salesViewConfig.ServiceAppName = "myService";
    salesViewConfig.ServiceName = "resources";
    salesViewConfig.ManagedDi = [
        "ui.router",
        "ngAnimate",
        "ngMessages",
        "ui.utils",
        "ui.select",
        "ngSanitize",
        "angular-loading-bar",
        "chart.js",
        salesViewConfig.ServiceAppName
    ];
    salesViewConfig.MakerController = "MakerController";
    salesViewConfig.GroupController = "GroupController";
    salesViewConfig.SalesController = "SalesController";
    salesViewConfig.ProductController = "ProductController";
    salesViewConfig.StateMaker = "Maker";
    salesViewConfig.StateGroup = "Group";
    salesViewConfig.StateSales = "Sales";
    salesViewConfig.StateProduct = "Product";
})(salesViewConfig || (salesViewConfig = {}));
//# sourceMappingURL=appConfig.js.map