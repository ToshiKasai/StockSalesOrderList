/// <reference path="../typings/tsd.d.ts" />
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var salesView;
(function (salesView) {
    "use strict";
    var app = angular.module(salesViewConfig.ServiceAppName, [
        "restangular",
        "toastr"
    ]);
    app.config([
        "RestangularProvider",
        function (RestangularProvider) {
            RestangularProvider.setBaseUrl("api");
        }
    ]);
    var Resources = (function (_super) {
        __extends(Resources, _super);
        function Resources(restangular, toastrService, $location, $http) {
            _super.call(this);
            this.restangular = restangular;
            this.toastrService = toastrService;
            this.$location = $location;
            this.$http = $http;
            this.initialize();
        }
        Resources.prototype.initialize = function () {
            var _this = this;
            this.session = new baseService.Session();
            this.restangular.all("Accounts").getList().then(function (acounts) {
                _this.session.id = acounts[0].id;
                _this.session.name = acounts[0].name;
            });
            this.restangular.all("Accounts/Roles").getList().then(function (roles) {
                _this.roles = roles;
            });
            // 年度選択肢の範囲
            this.yearList = [];
            var nowDate = new Date();
            var baseYear;
            if (nowDate.getMonth() < 10) {
                baseYear = nowDate.getFullYear();
            }
            else {
                baseYear = nowDate.getFullYear() + 1;
            }
            for (var i = 2015; i < baseYear + 2; i++) {
                this.yearList.push(i);
            }
            // メーカー関連
            this.makerList = [];
            this.userMakers = [];
            // グループ関連
            this.groupList = [];
            // 販売情報関連
            this.salesList = [];
            this.productPage = { start: 0, count: 0, pages: 0, page: 0, limit: 100, conditions: null };
            this.salesDetail = null;
            // 現在の情報関連
            this.currentData = null;
            // 販売動向情報
            this.trendList = [];
            this.trendDetail = null;
        };
        Object.defineProperty(Resources.prototype, "rest", {
            get: function () {
                return this.restangular;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Resources.prototype, "toastr", {
            get: function () {
                return this.toastrService;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Resources.prototype, "location", {
            get: function () {
                return this.$location;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Resources.prototype, "http", {
            get: function () {
                return this.$http;
            },
            enumerable: true,
            configurable: true
        });
        Resources.prototype.isNumber = function (numVal) {
            var pattern = /^[-]?([1-9]\d*|0)(\.\d+)?$/;
            return pattern.test(numVal);
            // return /^[+,-]?\d(\.\d+)?$/.test(numVal);
            // return /^[+,-]?([1-9]\d*|0)$/.test(numVal);
        };
        Resources.prototype.getMakerList = function (callback, target) {
            var _this = this;
            this.restangular.all("makers").getList().then(function (makers) {
                if (makers === null || makers === undefined) {
                    _this.makerList = [];
                }
                else {
                    _this.makerList = makers;
                }
                if (callback !== undefined && callback !== null) {
                    if (target !== undefined && target !== null) {
                        callback(target);
                    }
                    else {
                        callback();
                    }
                }
            });
            return this.makerList;
        };
        Resources.prototype.getUserMakerList = function (id, callback, target) {
            var _this = this;
            this.restangular.one("users", id).getList("makers").then(function (mymakers) {
                _this.userMakers = mymakers;
                if (callback !== undefined && callback !== null) {
                    if (target !== undefined && target !== null) {
                        callback(target);
                    }
                    else {
                        callback();
                    }
                }
            });
            return this.userMakers;
        };
        Resources.prototype.getGroupList = function (callback, target) {
            var _this = this;
            this.restangular.all("groups").getList({ MakerId: this.session.makerId }).then(function (groups) {
                _this.groupList = groups;
                if (callback !== undefined && callback !== null) {
                    if (target !== undefined && target !== null) {
                        callback(target);
                    }
                    else {
                        callback();
                    }
                }
            });
            return this.groupList;
        };
        Resources.prototype.getProductPage = function (callback, target) {
            var _this = this;
            var result;
            if (this.session.groupId !== null) {
                result = this.restangular.one("products/pages")
                    .get({ limit: this.productPage.limit, GroupId: this.session.groupId });
            }
            else {
                result = this.restangular.one("products/pages")
                    .get({ limit: this.productPage.limit, MakerId: this.session.makerId });
            }
            result.then(function (data) {
                if (data === undefined || data === null) {
                    _this.productPage.count = 0;
                    _this.productPage.pages = 0;
                }
                else {
                    _this.productPage.count = data.count;
                    _this.productPage.pages = data.pages;
                }
                if (callback !== undefined && callback !== null) {
                    if (target !== undefined && target !== null) {
                        callback(target);
                    }
                    else {
                        callback();
                    }
                }
            });
            return this.productPage;
        };
        // 販売在庫情報を一覧取得
        Resources.prototype.getSalesList = function (callback, target) {
            var _this = this;
            var result;
            if (this.session.groupId !== null) {
                result = this.restangular.all("salesviews")
                    .getList({
                    year: this.session.year, GroupId: this.session.groupId,
                    page: this.productPage.page, limit: this.productPage.limit
                });
            }
            else {
                result = this.restangular.all("salesviews")
                    .getList({
                    year: this.session.year, MakerId: this.session.makerId,
                    page: this.productPage.page, limit: this.productPage.limit
                });
            }
            result.then(function (salesviews) {
                _this.salesList = salesviews;
                _this.salesList.forEach(function (sales, index, array) {
                    sales.invoiceShow = false;
                    _this.recalculationSalesViewData(sales);
                });
                if (callback !== undefined && callback !== null) {
                    if (target !== undefined && target !== null) {
                        callback(target);
                    }
                    else {
                        callback();
                    }
                }
            });
            return this.salesList;
        };
        // キャッシュ情報より販売在庫情報を取得（サーバーアクセスはしない）
        Resources.prototype.getSalesDataByIdAsCache = function (id) {
            var result = null;
            this.salesList.forEach(function (sales, index, array) {
                if (sales.product.id === id) {
                    result = sales;
                }
            });
            return result;
        };
        // サーバーへの登録とキャッシュの更新
        Resources.prototype.setSalesData = function (data, callback, target) {
            return this.postSalesData(data, callback, target);
        };
        // サーバーへの登録とキャッシュの更新
        Resources.prototype.setSalesDataById = function (id, data) {
            var _this = this;
            var send;
            send = $.extend(true, {}, data);
            if (this.salesDetail === null || this.salesDetail === undefined) {
                this.getSalesListById(id);
            }
            if (this.salesDetail.product.id !== id) {
                this.getSalesListById(id);
            }
            var cnt = this.salesDetail.salesList.length;
            for (var i = 0; i < cnt; i++) {
                this.salesDetail.salesList[i].sales_plan = send.salesList[i].sales_plan;
            }
            this.salesDetail.put().then(function (value) {
                var tmp = _this.getSalesDataByIdAsCache(id);
                for (var i = 0; i < cnt; i++) {
                    tmp.salesList[i].sales_plan = send.salesList[i].sales_plan;
                }
                _this.recalculationSalesViewData(tmp);
            });
            return data;
        };
        Resources.prototype.postSalesData = function (data, callback, target) {
            var _this = this;
            var result;
            result = this.restangular.all("salesviews").post(data);
            result.then(function (sales) {
                if (sales !== null && sales !== undefined) {
                    var tmp = _this.getSalesDataByIdAsCache(sales.product.id);
                    var cnt = tmp.salesList.length;
                    for (var i = 0; i < cnt; i++) {
                        tmp.salesList[i].order_plan = sales.salesList[i].order_plan;
                        tmp.salesList[i].invoice_plan = sales.salesList[i].invoice_plan;
                        tmp.salesList[i].invoice_zan = sales.salesList[i].invoice_zan;
                        tmp.salesList[i].invoice_adjust = sales.salesList[i].invoice_adjust;
                        tmp.salesList[i].sales_plan = sales.salesList[i].sales_plan;
                        tmp.salesList[i].sales_trend = sales.salesList[i].sales_trend;
                    }
                    _this.recalculationSalesViewData(tmp);
                }
                if (callback !== undefined && callback !== null) {
                    if (target !== undefined && target !== null) {
                        callback(target);
                    }
                    else {
                        callback();
                    }
                }
            });
            return data;
        };
        // 販売在庫情報をサーバーより取得とキャッシュの更新
        Resources.prototype.getSalesListById = function (id, callback, target) {
            var _this = this;
            var result;
            result = this.restangular.one("salesviews", id).get({ year: this.session.year });
            result.then(function (salesview) {
                _this.salesDetail = salesview;
                _this.recalculationSalesViewData(_this.salesDetail);
                var cnt = _this.salesDetail.salesList.length;
                var tmp = _this.getSalesDataByIdAsCache(_this.salesDetail.product.id);
                for (var i = 0; i < cnt; i++) {
                    tmp.salesList[i].order_plan = _this.salesDetail.salesList[i].order_plan;
                    tmp.salesList[i].invoice_plan = _this.salesDetail.salesList[i].invoice_plan;
                    tmp.salesList[i].invoice_zan = _this.salesDetail.salesList[i].invoice_zan;
                    tmp.salesList[i].invoice_adjust = _this.salesDetail.salesList[i].invoice_adjust;
                    tmp.salesList[i].sales_plan = _this.salesDetail.salesList[i].sales_plan;
                    tmp.salesList[i].sales_trend = _this.salesDetail.salesList[i].sales_trend;
                }
                _this.recalculationSalesViewData(tmp);
                if (callback !== undefined && callback !== null) {
                    if (target !== undefined && target !== null) {
                        callback(target);
                    }
                    else {
                        callback();
                    }
                }
            });
            return this.getSalesDataByIdAsCache(id);
        };
        // 在庫予定や比率の算出（単一）
        Resources.prototype.recalculationSalesViewData = function (work) {
            var now = new Date();
            var test;
            if (work.zaikoPlan === undefined || work.zaikoPlan === null) {
                work.zaikoPlan = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
            }
            if (work.percentPreSales === undefined || work.percentPreSales === null) {
                work.percentPreSales = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
            }
            if (work.percentPlan === undefined || work.percentPlan === null) {
                work.percentPlan = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
            }
            for (var i = 0; i < 13; i++) {
                if (i === 0 || i === 1) {
                    work.zaikoPlan[i] = null;
                }
                else {
                    test = new Date(work.salesList[i - 1].detail_date);
                    if (test <= now) {
                        work.zaikoPlan[i] = work.salesList[i - 1].zaiko_actual;
                    }
                    else {
                        work.zaikoPlan[i] = work.zaikoPlan[i - 1];
                    }
                    // 販売数
                    work.zaikoPlan[i] -= (work.salesList[i - 1].sales_plan + work.salesList[i - 1].sales_trend);
                    // 入荷数
                    work.zaikoPlan[i] += (work.salesList[i - 1].invoice_plan
                        + work.salesList[i - 2].invoice_zan - work.salesList[i - 2].invoice_adjust);
                }
                if (work.salesList[i].pre_sales_actual !== 0) {
                    work.percentPreSales[i] = work.salesList[i].sales_actual / work.salesList[i].pre_sales_actual;
                }
                else {
                    work.percentPreSales[i] = 0;
                }
                if ((work.salesList[i].sales_plan + work.salesList[i].sales_trend) !== 0) {
                    work.percentPlan[i] = work.salesList[i].sales_actual / (work.salesList[i].sales_plan + work.salesList[i].sales_trend);
                }
                else {
                    work.percentPlan[i] = 0;
                }
            }
        };
        Resources.prototype.getCurrentData = function (id, callback, target) {
            var _this = this;
            var result;
            result = this.restangular.one("salesviews", id).one("Current").get();
            result.then(function (currents) {
                _this.currentData = currents;
                if (callback !== undefined && callback !== null) {
                    if (target !== undefined && target !== null) {
                        callback(target);
                    }
                    else {
                        callback();
                    }
                }
            });
            return this.currentData;
        };
        // 販売動向情報：一覧取得
        Resources.prototype.getTrendList = function (id, callback, target) {
            var _this = this;
            var result;
            result = this.restangular.one("salesviews", id).all("Trends").getList({ year: this.session.year });
            result.then(function (trends) {
                _this.trendList = trends;
                $.each(_this.trendList, function (index, value) {
                    if (value !== null) {
                        value.detail_date = new Date(value.detail_date);
                    }
                });
                if (callback !== undefined && callback !== null) {
                    if (target !== undefined && target !== null) {
                        callback(target);
                    }
                    else {
                        callback();
                    }
                }
            });
            return this.trendList;
        };
        // キャッシュ情報より販売動向情報を取得（サーバーアクセスはしない）
        Resources.prototype.getTrendDataByIdAsCache = function (id) {
            var result = null;
            this.trendList.forEach(function (trend, index, array) {
                if (trend.id === id) {
                    result = trend;
                }
            });
            return result;
        };
        // 販売動向情報をサーバーより取得とキャッシュの更新
        Resources.prototype.getTrendDataById = function (id, callback, target) {
            var _this = this;
            var result;
            result = this.restangular.one("salesviews", 0).one("Trends", id).get();
            result.then(function (trend) {
                _this.trendDetail = trend;
                _this.trendDetail.detail_date = new Date(_this.trendDetail.detail_date);
                if (callback !== undefined && callback !== null) {
                    if (target !== undefined && target !== null) {
                        callback(target);
                    }
                    else {
                        callback();
                    }
                }
            });
            return this.getTrendDataByIdAsCache(id);
        };
        // サーバーへの登録とキャッシュの更新(PUT/POST兼用)
        Resources.prototype.setTrendDataById = function (id, data, callback, target) {
            var send;
            send = $.extend(true, {}, data);
            if (id === null || id === "0") {
                this.postTrendData(send, callback, target);
            }
            else {
                this.trendDetail = null;
                this.getTrendDataById(id);
                this.putTrendData(send, null, callback, target);
            }
            return data;
        };
        Resources.prototype.postTrendData = function (data, callback, target) {
            var _this = this;
            var result;
            result = this.restangular.one("salesviews", 0).all("Trends").post(data);
            result.then(function (trends) {
                if (trends !== null && trends !== undefined) {
                    trends.detail_date = new Date(trends.detail_date);
                    _this.trendList.push(trends);
                }
                if (callback !== undefined && callback !== null) {
                    if (target !== undefined && target !== null) {
                        callback(target);
                    }
                    else {
                        callback();
                    }
                }
            });
            return data;
        };
        Resources.prototype.putTrendData = function (data, _target, callback, target) {
            if (_target === void 0) { _target = null; }
            var result;
            if (_target === null) {
                _target = this;
            }
            if (_target.trendDetail == null) {
                setTimeout(function () { _target.putTrendData(data, _target, callback, target); }, 1000);
                return data;
            }
            if (_target.trendDetail.id !== data.id) {
                setTimeout(function () { _target.putTrendData(data, _target, callback, target); }, 1000);
                return data;
            }
            _target.trendDetail.comments = data.comments;
            _target.trendDetail.detail_date = data.detail_date;
            _target.trendDetail.quantity = data.quantity;
            _target.trendDetail.user_id = target.session.id;
            result = _target.trendDetail.put();
            result.then(function (trends) {
                if (trends !== null && trends !== undefined) {
                    trends.detail_date = new Date(trends.detail_date);
                    $.each(_target.trendList, function (index, value) {
                        if (value.id === trends.id) {
                            _target.trendList.splice(index, 1);
                            _target.trendList.push(trends);
                            return false;
                        }
                    });
                }
                if (callback !== undefined && callback !== null) {
                    if (target !== undefined && target !== null) {
                        callback(target);
                    }
                    else {
                        callback();
                    }
                }
            });
            return data;
        };
        Resources.$inject = [
            "Restangular",
            "toastr",
            "$location",
            "$http"
        ];
        return Resources;
    }(baseService.BaseService));
    salesView.Resources = Resources;
    app.service(salesViewConfig.ServiceName, Resources);
})(salesView || (salesView = {}));
//# sourceMappingURL=resource.js.map