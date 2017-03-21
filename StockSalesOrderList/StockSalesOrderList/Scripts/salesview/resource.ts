/// <reference path="../typings/tsd.d.ts" />

module salesView {
    "use strict";

    const app: ng.IModule = angular.module(salesViewConfig.ServiceAppName, [
        "restangular",
        "toastr"
    ]);

    app.config([
        "RestangularProvider",
        function (RestangularProvider: restangular.IProvider): any {
            RestangularProvider.setBaseUrl("api");
        }
    ]);

    export class Resources extends baseService.BaseService {
        public static $inject = [
            "Restangular",
            "toastr",
            "$location",
            "$http"
        ];

        session: baseService.Session;
        makerList: apiModel.IMakerData[];
        userMakers: apiModel.IMakerData[];
        groupList: apiModel.IGroupData[];
        productPage: baseService.IPages;
        salesList: apiModel.ISalesViewData[];
        salesDetail: apiModel.IElementISalesViewData;
        currentData: apiModel.ICurrentData;

        trendList: apiModel.IElementTrendData[];
        trendDetail: apiModel.IElementTrendData;

        yearList: number[];

        constructor(
            private restangular: restangular.IService,
            private toastrService: angular.toastr.IToastrService,
            private $location: angular.ILocationService,
            private $http: angular.IHttpService
        ) {
            super();
            this.initialize();
        }

        private initialize(): void {
            this.session = new baseService.Session();
            this.restangular.all("Accounts").getList().then(
                (acounts: any[]) => {
                    this.session.id = acounts[0].id;
                    this.session.name = acounts[0].name;
                }
            );
            this.restangular.all("Accounts/Roles").getList().then(
                (roles: string[]) => {
                    this.roles = roles;
                }
            );

            // 年度選択肢の範囲
            this.yearList = [];
            let nowDate: Date = new Date();
            let baseYear: number;
            if (nowDate.getMonth() < 10) {
                baseYear = nowDate.getFullYear();
            } else {
                baseYear = nowDate.getFullYear() + 1;
            }
            for (let i: number = 2015; i < baseYear + 2; i++) {
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
        }

        get rest(): restangular.IService {
            return this.restangular;
        }

        get toastr(): angular.toastr.IToastrService {
            return this.toastrService;
        }

        get location(): angular.ILocationService {
            return this.$location;
        }

        get http(): angular.IHttpService {
            return this.$http;
        }

        isNumber(numVal: any): boolean {
            let pattern: RegExp = /^[-]?([1-9]\d*|0)(\.\d+)?$/;
            return pattern.test(numVal);
            // return /^[+,-]?\d(\.\d+)?$/.test(numVal);
            // return /^[+,-]?([1-9]\d*|0)$/.test(numVal);
        }

        getMakerList(callback?: Function, target?: any): apiModel.IMakerData[] {
            this.restangular.all("makers").getList().then(
                (makers: apiModel.IMakerData[]) => {
                    if (makers === null || makers === undefined) {
                        this.makerList = [];
                    } else {
                        this.makerList = makers;
                    }
                    if (callback !== undefined && callback !== null) {
                        if (target !== undefined && target !== null) {
                            callback(target);
                        } else {
                            callback();
                        }
                    }
                }
            );
            return this.makerList;
        }

        getUserMakerList(id: string, callback?: Function, target?: any): apiModel.IMakerData[] {
            this.restangular.one("users", id).getList("makers").then(
                (mymakers: apiModel.IMakerData[]) => {
                    this.userMakers = mymakers;
                    if (callback !== undefined && callback !== null) {
                        if (target !== undefined && target !== null) {
                            callback(target);
                        } else {
                            callback();
                        }
                    }
                }
            );
            return this.userMakers;
        }

        getGroupList(callback?: Function, target?: any): apiModel.IGroupData[] {
            this.restangular.all("groups").getList({ MakerId: this.session.makerId }).then(
                (groups: apiModel.IGroupData[]) => {
                    this.groupList = groups;
                    if (callback !== undefined && callback !== null) {
                        if (target !== undefined && target !== null) {
                            callback(target);
                        } else {
                            callback();
                        }
                    }
                }
            );
            return this.groupList;
        }

        getProductPage(callback?: Function, target?: any): baseService.IPages {
            let result: restangular.IPromise<any>;
            if (this.session.groupId !== null) {
                result = this.restangular.one("products/pages")
                    .get({ limit: this.productPage.limit, GroupId: this.session.groupId });
            } else {
                result = this.restangular.one("products/pages")
                    .get({ limit: this.productPage.limit, MakerId: this.session.makerId });
            }
            result.then(
                (data: any) => {
                    if (data === undefined || data === null) {
                        this.productPage.count = 0;
                        this.productPage.pages = 0;
                    } else {
                        this.productPage.count = data.count;
                        this.productPage.pages = data.pages;
                    }
                    if (callback !== undefined && callback !== null) {
                        if (target !== undefined && target !== null) {
                            callback(target);
                        } else {
                            callback();
                        }
                    }
                }
            );
            return this.productPage;
        }

        // 販売在庫情報を一覧取得
        getSalesList(callback?: Function, target?: any): apiModel.ISalesViewData[] {
            let result: restangular.ICollectionPromise<any>;
            if (this.session.groupId !== null) {
                result = this.restangular.all("salesviews")
                    .getList({
                        year: this.session.year, GroupId: this.session.groupId,
                        page: this.productPage.page, limit: this.productPage.limit
                    });
            } else {
                result = this.restangular.all("salesviews")
                    .getList({
                        year: this.session.year, MakerId: this.session.makerId,
                        page: this.productPage.page, limit: this.productPage.limit
                    });
            }
            result.then(
                (salesviews: apiModel.ISalesViewData[]) => {
                    this.salesList = salesviews;

                    this.salesList.forEach((sales: apiModel.ISalesViewData, index: number, array: apiModel.ISalesViewData[]) => {
                        sales.invoiceShow = false;
                        this.recalculationSalesViewData(sales);
                    });

                    if (callback !== undefined && callback !== null) {
                        if (target !== undefined && target !== null) {
                            callback(target);
                        } else {
                            callback();
                        }
                    }
                }
            );
            return this.salesList;
        }

        // キャッシュ情報より販売在庫情報を取得（サーバーアクセスはしない）
        getSalesDataByIdAsCache(id: string): apiModel.ISalesViewData {
            let result: apiModel.ISalesViewData = null;
            this.salesList.forEach((sales: apiModel.ISalesViewData, index: number, array: apiModel.ISalesViewData[]) => {
                if (sales.product.id === id) {
                    result = sales;
                }
            });
            return result;
        }

        // サーバーへの登録とキャッシュの更新
        setSalesData(data: apiModel.ISalesViewData, callback?: Function, target?: any): apiModel.ISalesViewData {
            return this.postSalesData(data, callback, target);
        }

        // サーバーへの登録とキャッシュの更新
        setSalesDataById(id: string, data: apiModel.ISalesViewData): apiModel.ISalesViewData {
            let send: apiModel.IElementISalesViewData;
            send = $.extend(true, {}, data);
            if (this.salesDetail === null || this.salesDetail === undefined) {
                this.getSalesListById(id);
            }
            if (this.salesDetail.product.id !== id) {
                this.getSalesListById(id);
            }

            let cnt: number = this.salesDetail.salesList.length;
            for (let i: number = 0; i < cnt; i++) {
                this.salesDetail.salesList[i].sales_plan = send.salesList[i].sales_plan;
            }

            this.salesDetail.put().then(
                (value: any) => {
                    let tmp: apiModel.ISalesViewData = this.getSalesDataByIdAsCache(id);
                    for (let i: number = 0; i < cnt; i++) {
                        tmp.salesList[i].sales_plan = send.salesList[i].sales_plan;
                    }
                    this.recalculationSalesViewData(tmp);
                }
            );

            return data;
        }

        private postSalesData(data: apiModel.ISalesViewData, callback?: Function, target?: any): apiModel.ISalesViewData {
            let result: restangular.IPromise<any>;
            result = this.restangular.all("salesviews").post(data);
            result.then(
                (sales: apiModel.IElementISalesViewData) => {
                    if (sales !== null && sales !== undefined) {
                        let tmp: apiModel.ISalesViewData = this.getSalesDataByIdAsCache(sales.product.id);
                        let cnt: number = tmp.salesList.length;
                        for (let i: number = 0; i < cnt; i++) {
                            tmp.salesList[i].order_plan = sales.salesList[i].order_plan;
                            tmp.salesList[i].invoice_plan = sales.salesList[i].invoice_plan;
                            tmp.salesList[i].invoice_zan = sales.salesList[i].invoice_zan;
                            tmp.salesList[i].invoice_adjust = sales.salesList[i].invoice_adjust;
                            tmp.salesList[i].sales_plan = sales.salesList[i].sales_plan;
                            tmp.salesList[i].sales_trend = sales.salesList[i].sales_trend;
                        }
                        this.recalculationSalesViewData(tmp);
                    }
                    if (callback !== undefined && callback !== null) {
                        if (target !== undefined && target !== null) {
                            callback(target);
                        } else {
                            callback();
                        }
                    }
                }
            );
            return data;
        }


        // 販売在庫情報をサーバーより取得とキャッシュの更新
        getSalesListById(id: string, callback?: Function, target?: any): apiModel.ISalesViewData {
            let result: restangular.IPromise<any>;
            result = this.restangular.one("salesviews", id).get({ year: this.session.year });
            result.then(
                (salesview: apiModel.IElementISalesViewData) => {
                    this.salesDetail = salesview;
                    this.recalculationSalesViewData(this.salesDetail);
                    let cnt: number = this.salesDetail.salesList.length;
                    let tmp: apiModel.ISalesViewData = this.getSalesDataByIdAsCache(this.salesDetail.product.id);
                    for (let i: number = 0; i < cnt; i++) {
                        tmp.salesList[i].order_plan = this.salesDetail.salesList[i].order_plan;
                        tmp.salesList[i].invoice_plan = this.salesDetail.salesList[i].invoice_plan;
                        tmp.salesList[i].invoice_zan = this.salesDetail.salesList[i].invoice_zan;
                        tmp.salesList[i].invoice_adjust = this.salesDetail.salesList[i].invoice_adjust;
                        tmp.salesList[i].sales_plan = this.salesDetail.salesList[i].sales_plan;
                        tmp.salesList[i].sales_trend = this.salesDetail.salesList[i].sales_trend;
                    }
                    this.recalculationSalesViewData(tmp);

                    if (callback !== undefined && callback !== null) {
                        if (target !== undefined && target !== null) {
                            callback(target);
                        } else {
                            callback();
                        }
                    }
                }
            );
            return this.getSalesDataByIdAsCache(id);
        }

        // 
        recalculationSalesViewDataOffline(work: apiModel.ISalesViewData): void {
            // インボイス残を計算
            for (let i: number = 0; i < 13; i++) {
                if (i >= 1) {
                    work.salesList[i].invoice_zan = (work.salesList[i - 1].invoice_zan - work.salesList[i - 1].invoice_adjust) + work.salesList[i].invoice_plan - work.salesList[i].invoice_actual;
                }
            }

            this.recalculationSalesViewData(work);
        }

        // 在庫予定や比率の算出（単一）
        recalculationSalesViewData(work: apiModel.ISalesViewData): void {
            let now: Date = new Date();
            let test: Date;
            if (work.zaikoPlan === undefined || work.zaikoPlan === null) {
                work.zaikoPlan = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
            }
            if (work.percentPreSales === undefined || work.percentPreSales === null) {
                work.percentPreSales = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
            }
            if (work.percentPlan === undefined || work.percentPlan === null) {
                work.percentPlan = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
            }
            for (let i: number = 0; i < 13; i++) {
                if (i === 0 || i === 1) {
                    work.zaikoPlan[i] = null;
                } else {
                    test = new Date(work.salesList[i - 1].detail_date);
                    if (test <= now) {
                        work.zaikoPlan[i] = work.salesList[i - 1].zaiko_actual;
                    } else {
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
                } else {
                    work.percentPreSales[i] = 0;
                }

                if ((work.salesList[i].sales_plan + work.salesList[i].sales_trend) !== 0) {
                    work.percentPlan[i] = work.salesList[i].sales_actual / (work.salesList[i].sales_plan + work.salesList[i].sales_trend);
                } else {
                    work.percentPlan[i] = 0;
                }
            }
        }

        getCurrentData(id: string, callback?: Function, target?: any): apiModel.ICurrentData {
            let result: restangular.IPromise<any>;
            result = this.restangular.one("salesviews", id).one("Current").get();
            result.then(
                (currents: apiModel.ICurrentData) => {
                    this.currentData = currents;
                    if (callback !== undefined && callback !== null) {
                        if (target !== undefined && target !== null) {
                            callback(target);
                        } else {
                            callback();
                        }
                    }
                }
            );
            return this.currentData;
        }

        // 販売動向情報：一覧取得
        getTrendList(id: string, callback?: Function, target?: any): apiModel.IElementTrendData[] {
            let result: restangular.ICollectionPromise<any>;
            result = this.restangular.one("salesviews", id).all("Trends").getList({ year: this.session.year });
            result.then(
                (trends: apiModel.IElementTrendData[]) => {
                    this.trendList = trends;

                    $.each(this.trendList, (index: number, value: apiModel.IElementTrendData) => {
                        if (value !== null) {
                            value.detail_date = new Date(value.detail_date);
                        }
                    });

                    if (callback !== undefined && callback !== null) {
                        if (target !== undefined && target !== null) {
                            callback(target);
                        } else {
                            callback();
                        }
                    }
                }
            );
            return this.trendList;
        }

        // キャッシュ情報より販売動向情報を取得（サーバーアクセスはしない）
        getTrendDataByIdAsCache(id: string): apiModel.IElementTrendData {
            let result: apiModel.IElementTrendData = null;
            this.trendList.forEach((trend: apiModel.IElementTrendData, index: number, array: apiModel.IElementTrendData[]) => {
                if (trend.id === id) {
                    result = trend;
                }
            });
            return result;
        }

        // 販売動向情報をサーバーより取得とキャッシュの更新
        getTrendDataById(id: string, callback?: Function, target?: any): apiModel.IElementTrendData {
            let result: restangular.IPromise<any>;
            result = this.restangular.one("salesviews", 0).one("Trends", id).get();
            result.then(
                (trend: apiModel.IElementTrendData) => {
                    this.trendDetail = trend;
                    this.trendDetail.detail_date = new Date(this.trendDetail.detail_date);

                    if (callback !== undefined && callback !== null) {
                        if (target !== undefined && target !== null) {
                            callback(target);
                        } else {
                            callback();
                        }
                    }
                }
            );
            return this.getTrendDataByIdAsCache(id);
        }

        // サーバーへの登録とキャッシュの更新(PUT/POST兼用)
        setTrendDataById(id: string, data: apiModel.IElementTrendData, callback?: Function, target?: any): apiModel.IElementTrendData {
            let send: apiModel.IElementTrendData;
            send = $.extend(true, {}, data);
            if (id === null || id === "0") {
                this.postTrendData(send, callback, target);
            } else {
                this.trendDetail = null;
                this.getTrendDataById(id);
                this.putTrendData(send, null, callback, target);
            }
            return data;
        }

        private postTrendData(data: apiModel.IElementTrendData, callback?: Function, target?: any): apiModel.IElementTrendData {
            let result: restangular.IPromise<any>;
            result = this.restangular.one("salesviews", 0).all("Trends").post(data);
            result.then(
                (trends: apiModel.IElementTrendData) => {
                    if (trends !== null && trends !== undefined) {
                        trends.detail_date = new Date(trends.detail_date);
                        this.trendList.push(trends);
                    }
                    if (callback !== undefined && callback !== null) {
                        if (target !== undefined && target !== null) {
                            callback(target);
                        } else {
                            callback();
                        }
                    }
                }
            );
            return data;
        }

        private putTrendData(data: apiModel.IElementTrendData,
            _target: Resources = null, callback?: Function, target?: any): apiModel.IElementTrendData {
            let result: restangular.IPromise<any>;
            if (_target === null) {
                _target = this;
            }
            if (_target.trendDetail == null) {
                setTimeout(() => { _target.putTrendData(data, _target, callback, target); }, 1000);
                return data;
            }
            if (_target.trendDetail.id !== data.id) {
                setTimeout(() => { _target.putTrendData(data, _target, callback, target); }, 1000);
                return data;
            }

            _target.trendDetail.comments = data.comments;
            _target.trendDetail.detail_date = data.detail_date;
            _target.trendDetail.quantity = data.quantity;
            _target.trendDetail.user_id = target.session.id;

            result = _target.trendDetail.put();
            result.then(
                (trends: apiModel.IElementTrendData) => {
                    if (trends !== null && trends !== undefined) {
                        trends.detail_date = new Date(trends.detail_date);
                        $.each(_target.trendList, (index: number, value: apiModel.IElementTrendData) => {
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
                        } else {
                            callback();
                        }
                    }
                }
            );
            return data;
        }
    }
    app.service(salesViewConfig.ServiceName, Resources);
}
