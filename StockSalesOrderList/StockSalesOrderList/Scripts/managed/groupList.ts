/// <reference path="../typings/tsd.d.ts" />

module managed {
    "use strict";

    export class GroupListController {
        public static $inject = [
            managedConfig.ManagedServiceName,
            "$state"
        ];

        groupList: Array<apiModel.IGroupData>;
        searchText: any;
        groupPage: IPages;
        productId: string;

        constructor(
            private resources: managed.Resources,
            private state: ng.ui.IStateService
        ) {
            this.searchText = {
                code: "",
                name: "",
                makerCode: ""
            };
            this.groupPage = this.resources.groupPage;
            this.groupList = [];
            this.resources.getGroupList(this.setGroupData, this);
            $.material.init();
        }

        displayPage(): number[] {
            let result: number[] = [];
            let s: number = this.groupPage.page - 2;
            if (s < 0) {
                s = 0;
            }
            let e: number = s + 5;
            if (e >= this.groupPage.pages) {
                e = this.groupPage.pages - 1;
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

        setGroupData(target: GroupListController): any {
            target.groupList = target.resources.groupList;
        }

        changePage(page: number): void {
            this.groupPage.page = page;
            this.resources.getGroupList(this.setGroupData, this);
        }

        jumpPage(jump: number): void {
            this.groupPage.page += jump;
            if (this.groupPage.page < 0) {
                this.groupPage.page = 0;
            }
            if (this.groupPage.page >= this.groupPage.pages) {
                this.groupPage.page = this.groupPage.pages - 1;
            }
            this.resources.getGroupList(this.setGroupData, this);
        }

        goMenu(): void {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateMenu);
        }

        goNew(): void {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateGroupEdit, { param: "0" });
        }

        goEdit(id: string): void {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateGroupEdit, { param: id });
        }

        goProduct(id: string, makerid: string): void {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.productId = id;
            this.resources.productPage.limit = 1000;
            this.resources.productPage.conditions = makerid;
            this.resources.getProductList(this.goProductSet, this);
        }

        goProductSet(target: GroupListController): void {
            target.state.go(managedConfig.StateGroupProduct, { param: target.productId });
        }

        refineBy(): boolean {
            if (this.searchText.code !== "") {
                return true;
            }
            if (this.searchText.name !== "") {
                return true;
            }
            if (this.searchText.makerCode !== "") {
                return true;
            }
            return false;
        }
    }

    angular.module(managedConfig.AppName).controller(managedConfig.GroupListController, GroupListController);
}
