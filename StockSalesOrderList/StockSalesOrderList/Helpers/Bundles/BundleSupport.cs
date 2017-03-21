using StockSalesOrderList.Helpers.Bundles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace StockSalesOrderList
{
    public class BundleSupport
    {
        private static List<BundleList> param = new List<BundleList>();
        public static void Initialize()
        {
            if (param.Count > 0)
                return;

            param.AddRange(new CommonBundleSet().Register());
            param.AddRange(new ManagedBundleSet().Register());
            param.AddRange(new SalesViewBundleSet().Register());
        }

        public static void BundleConfig()
        {
            BundleConfig(BundleTable.Bundles);
        }

        public static void BundleConfig(BundleCollection bundles)
        {
            Initialize();
            BundleTable.EnableOptimizations = true;

            foreach (BundleList item in param)
            {
                if (item.IsBaseList == false)
                {
                    if (item.Trans == null)
                        bundles.Add(new Bundle(item.VirtualPath).Include(item.BundleFile.ToArray()));
                    else
                        bundles.Add(new Bundle(item.VirtualPath, item.Trans).Include(item.BundleFile.ToArray()));
               }
            }
        }
    }

    /// <summary>
    /// 基本用バンドル
    /// </summary>
    public class CommonBundleSet
    {
        public BundleList[] Register()
        {
            return new BundleList[]{
                new BundleList("~/Scripts/base",
                new[] {
                    "~/Scripts/jquery-3.1.1.js",                // jQuery
                    "~/Scripts/angular.js",                     // angularjs
                    "~/Scripts/bootstrap.js",                   // bootstrap
                    "~/Scripts/material.js",                    // bootstrap-material
                    "~/Scripts/ripples.js",                     // bootstrap-ripples
                    "~/Scripts/jquery.validate.js",             // validation用
                    "~/Scripts/jquery.validate.unobtrusive.js", // validation用
                    "~/Scripts/common/materialInit.js"          // material初期化用
                },
                new JsCustomMinify()),
                 new BundleList("~/Content/base",
                new[] {
                    "~/Content/bootstrap.css",                  // bootstrap
                    "~/Content/bootstrap-material-design.css",  // bootstrap-material
                    "~/Content/ripples.css",                    // bootstrap-ripples
                    "~/Content/Site.css"                        // サイト独自
                },
                new CssCustomMinify()),
                 new BundleList("~/Scripts/modernizr",
                 new[] {
                     "~/Scripts/modernizr-2.8.3.js"
                 },
                 new JsCustomMinify())
            };
        }
    }

    /// <summary>
    /// 管理画面用バンドル
    /// </summary>
    public class ManagedBundleSet
    {
        public BundleList[] Register()
        {
            return new BundleList[]{
                new BundleList("~/Scripts/manage",
                new[] {
                    "~/Scripts/jquery-3.1.1.js",                // jQuery
                    "~/Scripts/angular.js",                     // angularjs
                    "~/Scripts/bootstrap.js",                   // bootstrap
                    "~/Scripts/ripples.js",                     // bootstrap-ripples
                    "~/Scripts/material.js",                    // bootstrap-material
                    "~/Scripts/jquery.validate.js",             // validation用
                    "~/Scripts/jquery.validate.unobtrusive.js", // validation用

                    "~/Scripts/managed/AppConfig.js",   // 定数ほか定義

                    "~/Scripts/lodash.js",              // lodash
                    "~/Scripts/restangular.js",         // RestAngular
                    "~/Scripts/angular-toastr.tpls.js", // トーストメッセージ
                    "~/Scripts/angular-ui-router.js",   // ルーティング
                    "~/Scripts/angular-animate.js",     // アニメーション
                    "~/Scripts/angular-messages.js",    // メッセージ(バリデーション表示)
                    "~/Scripts/angular-ui/ui-utils.js", // カスタムバリデーション
                    "~/Scripts/select.js",              // select機能
                    "~/Scripts/angular-sanitize.js",    // サニタイズ(select用)
                    "~/Scripts/loading-bar.js",         // ローディングバー
                    "~/Scripts/ocLazyLoad.js",          // lazyLoad

                    "~/Scripts/managed/ManagedResource.js", // サービス

                    "~/Scripts/managed/initialize.js",  // 初期
                    "~/Scripts/managed/menu.js",        // メニュースクリプト
                    "~/Scripts/managed/userList.js",    // ユーザー一覧
                    "~/Scripts/managed/userEdit.js",    // ユーザー編集
                    "~/Scripts/managed/userNew.js",     // ユーザー追加
                    "~/Scripts/managed/userRole.js",    // ユーザーロール編集
                    "~/Scripts/managed/userMaker.js",   // ユーザーメーカー編集
                    "~/Scripts/managed/makerList.js",   // メーカー一覧
                    "~/Scripts/managed/groupList.js",   // グループ一覧
                    "~/Scripts/managed/groupEdit.js",   // グループ編集
                    "~/Scripts/managed/groupProduct.js",// グループ商品編集
                    "~/Scripts/managed/productList.js", // 商品一覧
                    "~/Scripts/managed/productEdit.js", // 商品編集

                    "~/Scripts/managed/managed.js"      // メインスクリプト(bootstrap)
                },
                new JsCustomMinify()),
                new BundleList("~/Content/manage",
                new[] {
                    "~/Content/bootstrap.css",                  // bootstrap
                    "~/Content/bootstrap-material-design.css",  // bootstrap-material
                    "~/Content/ripples.css",                    // bootstrap-ripples

                    "~/Content/select.css",             // select用
                    "~/Content/angular-toastr.css",     // トーストメッセージ
                    "~/Content/loading-bar.css",        // ローディングバー

                    "~/Content/Site.css"                        // サイト独自
                },
                new CssCustomMinify())
            };
        }
    }

    /// <summary>
    /// 在庫販売画面用バンドル
    /// </summary>
    public class SalesViewBundleSet
    {
        public BundleList[] Register()
        {
            return new BundleList[]{
                new BundleList("~/Scripts/salesviews",
                new[] {
                    "~/Scripts/jquery-3.1.1.js",                // jQuery
                    "~/Scripts/angular.js",                     // angularjs
                    "~/Scripts/bootstrap.js",                   // bootstrap
                    "~/Scripts/ripples.js",                     // bootstrap-ripples
                    "~/Scripts/material.js",                    // bootstrap-material
                    "~/Scripts/jquery.validate.js",             // validation用
                    "~/Scripts/jquery.validate.unobtrusive.js", // validation用

                    "~/Scripts/common/holidayList.js",

                    // "~/Scripts/datepicker.js",                  // 日付選択
                    // "~/Scripts/i18n/datepicker.ja.js",          // 日付選択
                    // "~/Scripts/angular-datepicker.js",              // 日付選択
                    "~/Scripts/jquery-ui-1.12.1.js",            // jQueryUI
                    "~/Scripts/jquery-ui-i18n.js",
                    "~/Scripts/jquery.dateFormat-1.0.js",

                    "~/Scripts/salesview/appConfig.js",         // 定数ほか定義

                    "~/Scripts/lodash.js",              // lodash
                    "~/Scripts/restangular.js",         // RestAngular
                    "~/Scripts/angular-toastr.tpls.js", // トーストメッセージ
                    "~/Scripts/angular-ui-router.js",   // ルーティング
                    "~/Scripts/angular-animate.js",     // アニメーション
                    "~/Scripts/angular-messages.js",    // メッセージ(バリデーション表示)
                    "~/Scripts/angular-ui/ui-utils.js", // カスタムバリデーション
                    "~/Scripts/select.js",              // select機能
                    "~/Scripts/angular-sanitize.js",    // サニタイズ(select用)
                    "~/Scripts/loading-bar.js",         // ローディングバー
                    "~/Scripts/ocLazyLoad.js",          // lazyLoad

                    "~/Scripts/Chart.js",
                    "~/Scripts/angular-chart.js",

                    "~/Scripts/common/baseService.js",  // リソースサービスのベース
                    "~/Scripts/salesview/resource.js",  // リソースサービス
                    "~/Scripts/salesview/bootstrap.js", // 

                    "~/Scripts/salesview/makerList.js", // 
                    "~/Scripts/salesview/groupList.js", // 
                    "~/Scripts/salesview/salesList.js", // 
                    "~/Scripts/salesview/productDetail.js", // 

                    "~/Scripts/salesview/router.js"     // 
                },
                new JsCustomMinify()),
                new BundleList("~/Content/salesviews",
                new[] {
                    "~/Content/bootstrap.css",                  // bootstrap
                    "~/Content/bootstrap-material-design.css",  // bootstrap-material
                    "~/Content/ripples.css",                    // bootstrap-ripples

                    "~/Content/themes/base/all.css", // jQueryUI
                    // "~/Content/themes/base/datepicker.css", // 日付選択

                    // "~/Content/datepicker.css",         // 日付選択
                    // "~/Content/angular-datepicker.css",     // 日付選択
                    "~/Content/select.css",             // select用
                    "~/Content/angular-toastr.css",     // トーストメッセージ
                    "~/Content/loading-bar.css",        // ローディングバー

                    "~/Content/Site.css"                        // サイト独自
                },
                new CssCustomMinify())
            };
        }
    }
}
