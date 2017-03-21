/// <reference path="../typings/tsd.d.ts" />
var managedConfig;
(function (managedConfig) {
    "use strict";
    managedConfig.AppName = "myApp";
    managedConfig.ServiceAppName = "myService";
    managedConfig.ManagedServiceName = "resources";
    managedConfig.ManagedDi = [
        "ui.router",
        "ngAnimate",
        "ngMessages",
        "ui.utils",
        "ui.select",
        "ngSanitize",
        "angular-loading-bar",
        managedConfig.ServiceAppName // 管理機能用サービス
    ];
    managedConfig.MenuController = "MenuController";
    managedConfig.UserListController = "UserListController";
    managedConfig.UserEditController = "UserEditController";
    managedConfig.UserNewController = "UserNewController";
    managedConfig.UserRoleController = "UserRoleController";
    managedConfig.UserMakerController = "UserMakerController";
    managedConfig.MakerListController = "MakerListController";
    managedConfig.GroupListController = "GroupListController";
    managedConfig.GroupEditController = "GroupEditController";
    managedConfig.GroupProductController = "GroupProductController";
    managedConfig.ProductListController = "ProductListController";
    managedConfig.ProductEditController = "ProductEditController";
    managedConfig.StateMenu = "Menu";
    managedConfig.StateUserList = "UserList";
    managedConfig.StateUserEdit = "UserEdit";
    managedConfig.StateUserNew = "UserNew";
    managedConfig.StateUserRole = "UserRole";
    managedConfig.StateUserMaker = "UserMaker";
    managedConfig.StateMakerList = "MakerList";
    managedConfig.StateGroupList = "GroupList";
    managedConfig.StateGroupEdit = "GroupEdit";
    managedConfig.StateGroupProduct = "GroupProduct";
    managedConfig.StateProductList = "ProductList";
    managedConfig.StateProductEdit = "ProductEdit";
})(managedConfig || (managedConfig = {}));
//# sourceMappingURL=AppConfig.js.map