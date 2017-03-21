/// <reference path="../typings/tsd.d.ts" />

module salesViewConfig {
    "use strict";

    export const AppName: string = "myApp";
    export const ServiceAppName: string = "myService";
    export const ServiceName: string = "resources";

    export const ManagedDi: string[] = [
        "ui.router",                    // ルーティング
        "ngAnimate",                    // アニメーション
        "ngMessages",                   // メッセージ
        "ui.utils",                     // バリデーション用
        "ui.select",                    // ui.select
        "ngSanitize",                   // select用サニタイズ
        "angular-loading-bar",          // ローディング
        "chart.js",                     // チャート
        salesViewConfig.ServiceAppName  // 管理機能用サービス
    ];

    export const MakerController: string = "MakerController";
    export const GroupController: string = "GroupController";
    export const SalesController: string = "SalesController";
    export const ProductController: string = "ProductController";

    export const StateMaker: string = "Maker";
    export const StateGroup: string = "Group";
    export const StateSales: string = "Sales";
    export const StateProduct: string = "Product";
}
