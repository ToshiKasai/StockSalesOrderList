/// <reference path="../typings/tsd.d.ts" />

module managed {
    "use strict";

    export class ProductListController {
        public static $inject = [
            managedConfig.ManagedServiceName,
            "$state"
        ];

        productList: apiModel.IElementProductData[];
        productPage: IPages;
        searchText: any;

        constructor(
            private resources: managed.Resources,
            private state: ng.ui.IStateService
        ) {
            this.searchText = {
                code: "",
                name: ""
            };
            this.productPage = this.resources.productPage;
            this.productList = [];
            this.resources.getProductList(this.productData, this);
            $.material.init();
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

        productData(target: ProductListController): any {
            target.productList = target.resources.productList;
        }

        changePage(page: number): void {
            this.productPage.page = page;
            this.resources.getProductList(this.productData, this);
        }

        jumpPage(jump: number): void {
            this.productPage.page += jump;
            if (this.productPage.page < 0) {
                this.productPage.page = 0;
            }
            if (this.productPage.page >= this.productPage.pages) {
                this.productPage.page = this.productPage.pages-1;
            }
            this.resources.getProductList(this.productData, this);
        }

        goMenu(): void {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateMenu);
        }

        goEdit(id: string): void {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateProductEdit, { param: id });
        }

        refineBy(): boolean {
            if (this.searchText.code !== "") {
                return true;
            }
            if (this.searchText.name !== "") {
                return true;
            }
            return false;
        }

    }

    angular.module(managedConfig.AppName).controller(managedConfig.ProductListController, ProductListController);
}
