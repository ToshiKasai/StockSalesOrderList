/// <reference path="../typings/tsd.d.ts" />

module salesView {
    "use strict";

    class ChartClass {
        labels: Array<string>;
        series: Array<string>;
        data: Array<Array<number>>;
        datasetOverride: Array<ChartDataSets>;
        options: ChartOptions;

        constructor() {
            this.data = [
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            ];
            this.chartSet();
        }

        private floatFormat(value: number, n: number): number {
            var _pow: number = Math.pow(10, n);
            return Math.round(value * _pow) / _pow;
        }

        chartDataSets(data: apiModel.ISalesViewData): void {
            this.chartSalesDataset(data.salesList);
            for (var i: number = 0; i < 12; i++) {
                this.data[0][i] = data.zaikoPlan[i + 1] === null ? data.salesList[i + 1].zaiko_actual : data.zaikoPlan[i + 1];
                // this.data[5][i] = this.floatFormat(data.percentPlan[i + 1] * 100, 1);
                // this.data[6][i] = this.floatFormat(data.percentPreSales[i + 1] * 100, 1);
            }
        }
        chartSalesDataset(baseData: Array<apiModel.ISalesVieDetailData>): void {
            for (var i: number = 0; i < 12; i++) {
                this.data[1][i] = baseData[i + 1].zaiko_actual;
                this.data[2][i] = baseData[i + 1].sales_plan + baseData[i + 1].sales_trend;
                this.data[3][i] = baseData[i + 1].sales_actual;
                this.data[4][i] = baseData[i + 1].pre_sales_actual;
                this.data[5][i] = baseData[i + 1].invoice_plan;
                this.data[6][i] = baseData[i + 1].invoice_actual;
            }
        }

        chartSet(): void {
            this.labels = ["10月", "11月", "12月", "1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月"];
            this.series = ["在庫予測", "在庫数", "販売予定", "販売実績", "前年実績", "入荷予定", "入荷実績"]; // , "予実比", "前年比"];
            this.datasetOverride = [
                {
                    yAxisID: "y-axis-1", fill: true, lineTension: 0,
                    backgroundColor: "rgba(76,175,80, 0.1)",
                    borderColor: "rgba(76,175,80, 1)",
                },
                {
                    yAxisID: "y-axis-1", fill: true, lineTension: 0,
                    backgroundColor: "rgba(3,169,244, 0.1)",
                    borderColor: "rgba(3,169,244, 1)",
                },
                {
                    yAxisID: "y-axis-1", fill: true, lineTension: 0,
                    backgroundColor: "rgba(156,39,176, 0.1)",
                    borderColor: "rgba(156,39,176, 1)",
                },
                {
                    yAxisID: "y-axis-1", fill: true, lineTension: 0,
                    backgroundColor: "rgba(233,30,99, 0.1)",
                    borderColor: "rgba(233,30,99, 1)",
                },
                {
                    yAxisID: "y-axis-1", fill: true, lineTension: 0,
                    backgroundColor: "rgba(158,158,158, 0.1)",
                    borderColor: "rgba(158,158,158, 1)",
                },
                {
                    yAxisID: "y-axis-1", fill: false, lineTension: 0,
                    backgroundColor: "rgba(121, 85, 72, 0.1)",
                    borderColor: "rgba(121, 85, 72, 1)",
                },
                {
                    yAxisID: "y-axis-1", fill: false, lineTension: 0,
                    backgroundColor: "rgba(103, 58, 183, 0.1)",
                    borderColor: "rgba(103, 58, 183, 1)",
                }
            ];
            this.options = {
                title: {
                    display: true,
                    text: "販売情報チャート by M.Takayama"
                },
                legend: {
                    display: true,
                    position: "bottom",
                    /*labels: {
                        generateLabels: function (chart: any): any {
                            return chart.data.datasets.map(function (dataset: ChartDataSets, i: number): any {
                                return {
                                    text: dataset.label,
                                    fillStyle: dataset.backgroundColor,
                                    hidden: !chart.isDatasetVisible(i),
                                    lineCap: dataset.borderCapStyle,
                                    lineDash: [],
                                    lineDashOffset: 0,
                                    lineJoin: dataset.borderJoinStyle,
                                    lineWidth: dataset.pointBorderWidth,
                                    strokeStyle: dataset.borderColor,
                                    pointStyle: dataset.pointStyle,
                                    datasetIndex: i  // extra data used for toggling the datasets
                                };
                            });
                        }
                    }*/
                },
                scales: {
                    yAxes: [
                        {
                            id: "y-axis-1",
                            type: "linear",
                            display: true,
                            position: "left",
                            ticks: {
                                min: -100
                            }
                        },
/*                        {
                            id: "y-axis-2",
                            type: "linear",
                            display: true,
                            position: "right",
                            ticks: {
                                min: -100,
                                max: 500,
                                autoSkip: true,
                            }
                        }*/
                    ]
                }
            };
        }
    }

    export class ProductController {
        public static $inject = [
            salesViewConfig.ServiceName,
            "$state"
        ];

        session: baseService.Session;
        salesData: apiModel.ISalesViewData;
        charts: ChartClass;
        showOffice: boolean;
        officeMax: number;
        currentData: apiModel.ICurrentData;

        trendList: apiModel.IElementTrendData[];
        trendEdit: apiModel.IElementTrendData;

        modelDateEdit: string;
        modelQtyEdit: number;
        modelCommentEdit: string;
        modalErrMessage: string;

        constructor(
            private resources: salesView.Resources,
            private state: ng.ui.IStateService,
        ) {
            this.session = this.resources.session;
            this.salesData = this.resources.getSalesDataByIdAsCache(this.session.productId);
            this.charts = new ChartClass();
            this.charts.chartDataSets(this.salesData);
            this.showOffice = false;
            this.officeMax = 5;
            this.currentData = null;

            this.trendEdit = null;
            this.trendList = [];
            this.modelDateEdit = null;
            this.modalErrMessage = null;

            this.resources.getSalesListById(this.session.productId, this.setDetailData, this);
            this.resources.getCurrentData(this.session.productId, this.setCurrentData, this);
            this.resources.getTrendList(this.session.productId, this.setTrendData, this);

            let nowDate:Date = new Date();
            let minDate:string = nowDate.getFullYear().toString();
            let maxDate: string = (nowDate.getFullYear() + 2).toString();
            if (nowDate.getMonth() < 9) {
                minDate = (nowDate.getFullYear() - 1).toString();
            }
            minDate += "/10/1";
            maxDate += "/9/30";

            let hol: holiday.Holiday = new holiday.Holiday();

            var holidays: Array<holiday.ItemHoliday> = hol.getHoliday(2017);
            // console.info(holidays);

            $.datepicker.setDefaults($.datepicker.regional["ja"]);
            $("[data-toggle='datepicker']").datepicker(
                {
                    minDate: minDate,
                    maxDate: maxDate,
                    changeYear: true,
                    changeMonth: true,
                    closeText: "閉じる",
                    currentText: "今日",
                    showButtonPanel: true,
                    beforeShowDay: function (day: Date):any {
                        var result:any;
                        var holiday: any = holidays[$.format.date(day, "yyyy/MM/dd")];
                        // 祝日・非営業日定義に存在するか？
                        if (holiday) {
                            result = [true, "date-holiday" + holiday.type, holiday.title];
                        } else {
                            switch (day.getDay()) {
                                case 0: // 日曜日か？
                                    result = [true, "date-sunday"];
                                    break;
                                case 6: // 土曜日か？
                                    result = [true, "date-saturday"];
                                    break;
                                default:
                                    result = [true, ""];
                                    break;
                            }
                        }
                        return result;
                    }
                }
            );
            $.material.init();
        }

        getFixed(): number {
            return this.salesData.product.isSoldWeight ? 3 : 0;
        }

        setDetailData(target: ProductController): void {
            target.salesData = target.resources.salesDetail;
            target.charts.chartDataSets(target.salesData);
        }

        setCurrentData(target: ProductController): void {
            target.currentData = target.resources.currentData;
        }

        setTrendData(target: ProductController): void {
            target.trendList = target.resources.trendList;
        }

        goMaker(): void {
            this.scrollTop();
            this.state.go(salesViewConfig.StateMaker);
        }

        goGroup(): void {
            this.scrollTop();
            this.state.go(salesViewConfig.StateGroup);
        }

        goSales(): void {
            this.scrollTop();
            this.state.go(salesViewConfig.StateSales);
        }

        scrollTop(): void {
            // angular.element("body").animate({ scrollTop: 0 }, "fast");
            $("html, body").animate({ scrollTop: 0 }, 500);
        }

        toggleOffice(): void {
            this.showOffice = this.showOffice ? false : true;
        }

        toggleInvoice(): void {
            this.salesData.invoiceShow = this.salesData.invoiceShow ? false : true;
        }

        newTrendEdit(): void {
            this.modalErrMessage = null;
            let model: apiModel.ITrendData = {
                id:"0", product_id: this.session.productId, detail_date: null, quantity: 0, comments: null,
                user_id: this.session.id, user_name: this.session.name
            };
            let tmp: Date = new Date();
            this.modelDateEdit = tmp.getFullYear().toString() + "/" + (tmp.getMonth() + 1).toString() + "/" + tmp.getDate().toString();
            this.modelQtyEdit = 0;
            this.modelCommentEdit = null;
            // this.modelDateEdit = $("[data-toggle='datepicker']").datepicker('formatDate', tmp);
            // $("[data-toggle='datepicker']").datepicker("setDate", this.modelDateEdit);
            this.trendEdit = <apiModel.IElementTrendData>model;
        }

        setTrendEdit(id: string): void {
            this.modalErrMessage = null;
            this.trendEdit = this.resources.getTrendDataByIdAsCache(id);
            if (this.trendEdit.detail_date === null) {
                this.modelDateEdit = null;
            } else {
                 this.modelDateEdit = this.trendEdit.detail_date.getFullYear().toString() + "/"
                    + (this.trendEdit.detail_date.getMonth() + 1).toString() + "/" + this.trendEdit.detail_date.getDate().toString();
            }
            if (this.trendEdit.quantity === null) {
                this.modelQtyEdit = 0;
            } else {
                this.modelQtyEdit = this.trendEdit.quantity;
            }
            this.modelCommentEdit = this.trendEdit.comments;

            $("#inputModal").modal();
        }

        registTrendData(): void {
            this.modalErrMessage = null;
            if (this.modelDateEdit === null || this.modelDateEdit === "") {
                this.modalErrMessage = "日付が未設定です。";
                return;
            }
            let tmp: number = Date.parse(this.modelDateEdit);
            let chk: Date = new Date(tmp);
            if (chk.toString() === "Invalid Date") {
                this.modalErrMessage = "正しい日付ではありません。";
                return;
            }
            if (this.resources.isNumber(this.modelQtyEdit) === false) {
                this.modalErrMessage = "正しい数値ではありません。";
                return;
            }

            this.trendEdit.detail_date = chk;
            this.trendEdit.quantity = this.modelQtyEdit;
            this.trendEdit.comments = this.modelCommentEdit;
            $("#inputModal").modal("hide");
            this.resources.setTrendDataById(this.trendEdit.id, this.trendEdit, this.doneRegistTrend, this);
        }

        doneRegistTrend(target: ProductController): void {
            target.resources.getSalesListById(target.session.productId, target.setDetailData, target);
            target.resources.toastr.success("動向情報の更新をしました。");
        }
    }
    angular.module(salesViewConfig.AppName).controller(salesViewConfig.ProductController, ProductController);
}
