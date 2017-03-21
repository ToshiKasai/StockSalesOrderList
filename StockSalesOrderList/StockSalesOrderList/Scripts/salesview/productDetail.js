var salesView;
(function (salesView) {
    "use strict";
    var ChartClass = (function () {
        function ChartClass() {
            this.data = [
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            ];
            this.chartSet();
        }
        ChartClass.prototype.floatFormat = function (value, n) {
            var _pow = Math.pow(10, n);
            return Math.round(value * _pow) / _pow;
        };
        ChartClass.prototype.chartDataSets = function (data) {
            this.chartSalesDataset(data.salesList);
            for (var i = 0; i < 12; i++) {
                this.data[0][i] = data.zaikoPlan[i + 1] === null ? data.salesList[i + 1].zaiko_actual : data.zaikoPlan[i + 1];
            }
        };
        ChartClass.prototype.chartSalesDataset = function (baseData) {
            for (var i = 0; i < 12; i++) {
                this.data[1][i] = baseData[i + 1].zaiko_actual;
                this.data[2][i] = baseData[i + 1].sales_plan + baseData[i + 1].sales_trend;
                this.data[3][i] = baseData[i + 1].sales_actual;
                this.data[4][i] = baseData[i + 1].pre_sales_actual;
            }
        };
        ChartClass.prototype.chartSet = function () {
            this.labels = ["10月", "11月", "12月", "1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月"];
            this.series = ["在庫予測", "在庫数", "販売予定", "販売実績", "前年実績"];
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
            ];
            this.options = {
                title: {
                    display: true,
                    text: "販売情報チャート by M.Takayama"
                },
                legend: {
                    display: true,
                    position: "bottom",
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
                    ]
                }
            };
        };
        return ChartClass;
    }());
    var ProductController = (function () {
        function ProductController(resources, state) {
            this.resources = resources;
            this.state = state;
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
            var nowDate = new Date();
            var minDate = nowDate.getFullYear().toString();
            var maxDate = (nowDate.getFullYear() + 2).toString();
            if (nowDate.getMonth() < 9) {
                minDate = (nowDate.getFullYear() - 1).toString();
            }
            minDate += "/10/1";
            maxDate += "/9/30";
            var hol = new holiday.Holiday();
            var holidays = hol.getHoliday(2017);
            $.datepicker.setDefaults($.datepicker.regional["ja"]);
            $("[data-toggle='datepicker']").datepicker({
                minDate: minDate,
                maxDate: maxDate,
                changeYear: true,
                changeMonth: true,
                closeText: "閉じる",
                currentText: "今日",
                showButtonPanel: true,
                beforeShowDay: function (day) {
                    var result;
                    var holiday = holidays[$.format.date(day, "yyyy/MM/dd")];
                    if (holiday) {
                        result = [true, "date-holiday" + holiday.type, holiday.title];
                    }
                    else {
                        switch (day.getDay()) {
                            case 0:
                                result = [true, "date-sunday"];
                                break;
                            case 6:
                                result = [true, "date-saturday"];
                                break;
                            default:
                                result = [true, ""];
                                break;
                        }
                    }
                    return result;
                }
            });
            $.material.init();
        }
        ProductController.prototype.getFixed = function () {
            return this.salesData.product.isSoldWeight ? 3 : 0;
        };
        ProductController.prototype.setDetailData = function (target) {
            target.salesData = target.resources.salesDetail;
            target.charts.chartDataSets(target.salesData);
        };
        ProductController.prototype.setCurrentData = function (target) {
            target.currentData = target.resources.currentData;
        };
        ProductController.prototype.setTrendData = function (target) {
            target.trendList = target.resources.trendList;
        };
        ProductController.prototype.goMaker = function () {
            this.scrollTop();
            this.state.go(salesViewConfig.StateMaker);
        };
        ProductController.prototype.goGroup = function () {
            this.scrollTop();
            this.state.go(salesViewConfig.StateGroup);
        };
        ProductController.prototype.goSales = function () {
            this.scrollTop();
            this.state.go(salesViewConfig.StateSales);
        };
        ProductController.prototype.scrollTop = function () {
            $("html, body").animate({ scrollTop: 0 }, 500);
        };
        ProductController.prototype.toggleOffice = function () {
            this.showOffice = this.showOffice ? false : true;
        };
        ProductController.prototype.toggleInvoice = function () {
            this.salesData.invoiceShow = this.salesData.invoiceShow ? false : true;
        };
        ProductController.prototype.newTrendEdit = function () {
            this.modalErrMessage = null;
            var model = {
                id: "0", product_id: this.session.productId, detail_date: null, quantity: 0, comments: null,
                user_id: this.session.id, user_name: this.session.name
            };
            var tmp = new Date();
            this.modelDateEdit = tmp.getFullYear().toString() + "/" + (tmp.getMonth() + 1).toString() + "/" + tmp.getDate().toString();
            this.modelQtyEdit = 0;
            this.modelCommentEdit = null;
            this.trendEdit = model;
        };
        ProductController.prototype.setTrendEdit = function (id) {
            this.modalErrMessage = null;
            this.trendEdit = this.resources.getTrendDataByIdAsCache(id);
            if (this.trendEdit.detail_date === null) {
                this.modelDateEdit = null;
            }
            else {
                this.modelDateEdit = this.trendEdit.detail_date.getFullYear().toString() + "/"
                    + (this.trendEdit.detail_date.getMonth() + 1).toString() + "/" + this.trendEdit.detail_date.getDate().toString();
            }
            if (this.trendEdit.quantity === null) {
                this.modelQtyEdit = 0;
            }
            else {
                this.modelQtyEdit = this.trendEdit.quantity;
            }
            this.modelCommentEdit = this.trendEdit.comments;
            $("#inputModal").modal();
        };
        ProductController.prototype.registTrendData = function () {
            this.modalErrMessage = null;
            if (this.modelDateEdit === null || this.modelDateEdit === "") {
                this.modalErrMessage = "日付が未設定です。";
                return;
            }
            var tmp = Date.parse(this.modelDateEdit);
            var chk = new Date(tmp);
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
        };
        ProductController.prototype.doneRegistTrend = function (target) {
            target.resources.getSalesListById(target.session.productId, target.setDetailData, target);
            target.resources.toastr.success("動向情報の更新をしました。");
        };
        ProductController.$inject = [
            salesViewConfig.ServiceName,
            "$state"
        ];
        return ProductController;
    }());
    salesView.ProductController = ProductController;
    angular.module(salesViewConfig.AppName).controller(salesViewConfig.ProductController, ProductController);
})(salesView || (salesView = {}));
//# sourceMappingURL=productDetail.js.map