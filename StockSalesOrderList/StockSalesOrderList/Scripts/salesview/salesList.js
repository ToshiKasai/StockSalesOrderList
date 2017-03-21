var salesView;
(function (salesView) {
    "use strict";
    var SalesController = (function () {
        function SalesController(resources, state) {
            this.resources = resources;
            this.state = state;
            this.session = this.resources.session;
            this.makeLink();
            this.yearList = this.resources.yearList;
            this.salesList = this.resources.salesList;
            $("[data-toggle='tooltip']").tooltip();
            $.material.init();
        }
        SalesController.prototype.makeLink = function () {
            this.dowloadLink = "SalesView/Download?year=" + this.session.year.toString();
            if (this.session.groupId === null) {
                this.dowloadLink = this.dowloadLink + "&makerid=" + this.session.makerId;
            }
            else {
                this.dowloadLink = this.dowloadLink + "&groupid=" + this.session.groupId;
            }
            this.dowloadTabLink = "SalesView/DownloadTab?year=" + this.session.year.toString();
            if (this.session.groupId === null) {
                this.dowloadTabLink = this.dowloadTabLink + "&makerid=" + this.session.makerId;
            }
            else {
                this.dowloadTabLink = this.dowloadTabLink + "&groupid=" + this.session.groupId;
            }
        };
        SalesController.prototype.productPageSet = function (target) {
            target.productPage = target.resources.productPage;
            target.resources.getSalesList(target.salesListSet, target);
        };
        SalesController.prototype.salesListSet = function (target) {
            target.salesList = target.resources.salesList;
        };
        SalesController.prototype.displayPage = function () {
            var result = [];
            var s = this.productPage.page - 2;
            if (s < 0) {
                s = 0;
            }
            var e = s + 5;
            if (e >= this.productPage.pages) {
                e = this.productPage.pages - 1;
                s = e - 5;
                if (s < 0) {
                    s = 0;
                }
            }
            for (; s <= e; s++) {
                result.push(s);
            }
            return result;
        };
        SalesController.prototype.changePage = function (page) {
            this.productPage.page = page;
            this.resources.getSalesList(this.salesListSet, this);
        };
        SalesController.prototype.jumpPage = function (jump) {
            this.productPage.page += jump;
            if (this.productPage.page < 0) {
                this.productPage.page = 0;
            }
            if (this.productPage.page >= this.productPage.pages) {
                this.productPage.page = this.productPage.pages - 1;
            }
            this.resources.getSalesList(this.salesListSet, this);
        };
        SalesController.prototype.getFixed = function (work) {
            return work.product.isSoldWeight ? 3 : 0;
        };
        SalesController.prototype.selectYears = function () {
            this.resources.getSalesList(this.salesListSet, this);
            this.makeLink();
        };
        SalesController.prototype.goMaker = function () {
            this.scrollTop();
            this.state.go(salesViewConfig.StateMaker);
        };
        SalesController.prototype.goGroup = function () {
            this.scrollTop();
            this.state.go(salesViewConfig.StateGroup);
        };
        SalesController.prototype.goDetail = function (id) {
            this.scrollTop();
            this.session.productId = id;
            this.state.go(salesViewConfig.StateProduct);
        };
        SalesController.prototype.saveData = function (id) {
            var tmp = this.resources.getSalesDataByIdAsCache(id);
            this.resources.setSalesData(tmp, this.savedPlan, this);
        };
        SalesController.prototype.savedPlan = function (target) {
            target.resources.toastr.success("情報を更新しました。");
        };
        SalesController.prototype.scrollTop = function () {
            $("html, body").animate({ scrollTop: 0 }, 500);
        };
        SalesController.prototype.toggleInvoice = function (sl) {
            sl.invoiceShow = sl.invoiceShow ? false : true;
        };
        SalesController.prototype.recalc = function (sl) {
            this.resources.recalculationSalesViewData(sl);
        };
        SalesController.prototype.inputOrder = function (index, sl) {
            var inv = index;
            if (sl.product.leadTime !== null) {
                inv += sl.product.leadTime;
            }
            if (inv < sl.salesList.length) {
                sl.salesList[inv].invoice_plan = sl.salesList[index].order_plan;
            }
            this.resources.recalculationSalesViewDataOffline(sl);
        };
        SalesController.prototype.inputInvoice = function (index, sl) {
            this.resources.recalculationSalesViewDataOffline(sl);
        };
        SalesController.prototype.inputInvoiceReming = function (index, sl) {
            this.resources.recalculationSalesViewDataOffline(sl);
        };
        SalesController.prototype.inputSalesPlan = function (index, sl) {
            this.resources.recalculationSalesViewDataOffline(sl);
        };
        SalesController.prototype.toggleSidemenu = function () {
            if (document.getElementById("sideMenu").style.left === "0px") {
                this.closeSidemenu();
            }
            else {
                this.openSidemenu();
            }
        };
        SalesController.prototype.openSidemenu = function () {
            document.getElementById("sideMenu").style.left = "0px";
            document.getElementById("sideOverlay").style.width = "100%";
            document.getElementById("sideOverlay").style.opacity = "1";
        };
        SalesController.prototype.closeSidemenu = function () {
            document.getElementById("sideMenu").style.left = "-280px";
            document.getElementById("sideOverlay").style.width = "0%";
            document.getElementById("sideOverlay").style.opacity = "0";
        };
        SalesController.prototype.scrollTo = function (id) {
            var dat = "#article-" + id;
            var position = angular.element(dat).position().top;
            $("html, body").animate({ scrollTop: position }, 400);
        };
        SalesController.$inject = [
            salesViewConfig.ServiceName,
            "$state"
        ];
        return SalesController;
    }());
    salesView.SalesController = SalesController;
    angular.module(salesViewConfig.AppName).controller(salesViewConfig.SalesController, SalesController);
})(salesView || (salesView = {}));
//# sourceMappingURL=salesList.js.map