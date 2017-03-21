/// <reference path="../typings/tsd.d.ts" />

module managedConfig {
    "use strict";

    export const AppName: string = "myApp";
    export const ServiceAppName: string = "myService";
    export const ManagedServiceName: string = "resources";

    export const ManagedDi: string[] = [
        "ui.router",                    // ルーティング
        "ngAnimate",                    // アニメーション
        "ngMessages",                   // メッセージ
        "ui.utils",                     // バリデーション用
        "ui.select",                    // ui.select
        "ngSanitize",                   // select用サニタイズ
        "angular-loading-bar",          // ローディング
        managedConfig.ServiceAppName    // 管理機能用サービス
    ];

    export const MenuController: string = "MenuController";
    export const UserListController: string = "UserListController";
    export const UserEditController: string = "UserEditController";
    export const UserNewController: string = "UserNewController";
    export const UserRoleController: string = "UserRoleController";
    export const UserMakerController: string = "UserMakerController";
    export const MakerListController: string = "MakerListController";
    export const GroupListController: string = "GroupListController";
    export const GroupEditController: string = "GroupEditController";
    export const GroupProductController: string = "GroupProductController";
    export const ProductListController: string = "ProductListController";
    export const ProductEditController: string = "ProductEditController";

    export const StateMenu: string = "Menu";
    export const StateUserList: string = "UserList";
    export const StateUserEdit: string = "UserEdit";
    export const StateUserNew: string = "UserNew";
    export const StateUserRole: string = "UserRole";
    export const StateUserMaker: string = "UserMaker";
    export const StateMakerList: string = "MakerList";
    export const StateGroupList: string = "GroupList";
    export const StateGroupEdit: string = "GroupEdit";
    export const StateGroupProduct: string = "GroupProduct";
    export const StateProductList: string = "ProductList";
    export const StateProductEdit: string = "ProductEdit";
}
