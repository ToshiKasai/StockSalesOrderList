/// <reference path="../typings/tsd.d.ts" />

module salesView {
    "use strict";

    export class SalesController {
        public static $inject = [
            salesViewConfig.ServiceName,
            "$state"
        ];

        session: baseService.Session;
        yearList: number[];
        productPage: baseService.IPages;
        salesList: apiModel.ISalesViewData[];
        dowloadLink: string;
        dowloadTabLink: string;

        planEdit: apiModel.IElementISalesViewData;

        constructor(
            private resources: salesView.Resources,
            private state: ng.ui.IStateService
        ) {
            this.session = this.resources.session;
            this.makeLink();
            this.yearList = this.resources.yearList;

            this.salesList = this.resources.salesList;
            $("[data-toggle='tooltip']").tooltip();
            $.material.init();
        }

        makeLink(): void {
            this.dowloadLink = "SalesView/Download?year=" + this.session.year.toString();
            if (this.session.groupId === null) {
                this.dowloadLink = this.dowloadLink + "&makerid=" + this.session.makerId;
            } else {
                this.dowloadLink = this.dowloadLink + "&groupid=" + this.session.groupId;
            }
            this.dowloadTabLink = "SalesView/DownloadTab?year=" + this.session.year.toString();
            if (this.session.groupId === null) {
                this.dowloadTabLink = this.dowloadTabLink + "&makerid=" + this.session.makerId;
            } else {
                this.dowloadTabLink = this.dowloadTabLink + "&groupid=" + this.session.groupId;
            }
        }

        productPageSet(target: SalesController): void {
            target.productPage = target.resources.productPage;
            target.resources.getSalesList(target.salesListSet, target);
        }

        salesListSet(target: SalesController): void {
            target.salesList = target.resources.salesList;
        }

        displayPage(): number[] {
            let result: number[] = [];
            let s: number = this.productPage.page - 2;
            if (s < 0) {
                s = 0;
            }
            let e: number = s + 5;
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
        }

        changePage(page: number): void {
            this.productPage.page = page;
            this.resources.getSalesList(this.salesListSet, this);
        }

        jumpPage(jump: number): void {
            this.productPage.page += jump;
            if (this.productPage.page < 0) {
                this.productPage.page = 0;
            }
            if (this.productPage.page >= this.productPage.pages) {
                this.productPage.page = this.productPage.pages - 1;
            }
            this.resources.getSalesList(this.salesListSet, this);
        }

        getFixed(work: apiModel.ISalesViewData): number {
            return work.product.isSoldWeight ? 3 : 0;
        }

        selectYears(): void {
            this.resources.getSalesList(this.salesListSet, this);
            this.makeLink();
        }

        goMaker(): void {
            this.scrollTop();
            this.state.go(salesViewConfig.StateMaker);
        }

        goGroup(): void {
            this.scrollTop();
            this.state.go(salesViewConfig.StateGroup);
        }

        goDetail(id: string): void {
            this.scrollTop();
            this.session.productId = id;
            this.state.go(salesViewConfig.StateProduct);
        }

        saveData(id: string): void {
            let tmp: apiModel.ISalesViewData = this.resources.getSalesDataByIdAsCache(id);
            this.resources.setSalesData(tmp, this.savedPlan, this);
        }
        savedPlan(target: SalesController): void {
            target.resources.toastr.success("情報を更新しました。");
        }

        scrollTop(): void {
            $("html, body").animate({ scrollTop: 0 }, 500);
            // angular.element("body").animate({ scrollTop: 0 }, "fast");
        }

        toggleInvoice(sl: apiModel.ISalesViewData): void {
            sl.invoiceShow = sl.invoiceShow ? false : true;
        }

        recalc(sl: apiModel.ISalesViewData): void {
            this.resources.recalculationSalesViewData(sl);
        }

        inputOrder(index:number, sl: apiModel.ISalesViewData): void {
            var inv: number = index;
            if (sl.product.leadTime !== null) {
                inv += sl.product.leadTime;
            }
            if (inv < sl.salesList.length) {
                sl.salesList[inv].invoice_plan = sl.salesList[index].order_plan;
            }
        }

        toggleSidemenu(): void {
            if (document.getElementById("sideMenu").style.left === "0px") {
                this.closeSidemenu();
            } else {
                this.openSidemenu();
            }
        }
        openSidemenu(): void {
            document.getElementById("sideMenu").style.left = "0px";
            document.getElementById("sideOverlay").style.width = "100%";
            document.getElementById("sideOverlay").style.opacity = "1";
        }
        closeSidemenu(): void {
            document.getElementById("sideMenu").style.left = "-280px";
            document.getElementById("sideOverlay").style.width = "0%";
            document.getElementById("sideOverlay").style.opacity = "0";
        }
        scrollTo(id: string): void {
            var dat: string = "#article-" + id;
            var position: number = angular.element(dat).position().top;
            // angular.element("body").animate({ scrollTop: position }, 400);
            $("html, body").animate({ scrollTop: position }, 400);
        }
    }
    angular.module(salesViewConfig.AppName).controller(salesViewConfig.SalesController, SalesController);
}
