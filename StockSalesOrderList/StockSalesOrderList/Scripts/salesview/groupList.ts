/// <reference path="../typings/tsd.d.ts" />

module salesView {
    "use strict";

    export class GroupController {
        public static $inject = [
            salesViewConfig.ServiceName,
            "$state"
        ];

        session: baseService.Session;
        yearList: number[];
        searchText: any;
        groupList: apiModel.IGroupData[];

        constructor(
            private resources: salesView.Resources,
            private state: ng.ui.IStateService,
        ) {
            this.session = this.resources.session;
            this.searchText = {
                code: "",
                name: ""
            };
            this.yearList = this.resources.yearList;
            this.groupList = [];
            this.resources.getGroupList(this.groupListSet, this);
            $.material.init();
        }

        groupListSet(target: GroupController): void {
            let full: apiModel.IGroupData = {
                id: null, code: "******", name: "すべての商品",
                makerModelId: null, makerCode: null, makerName: null,
                containerModelId: null, containerName: null,
                containerCapacityBt20Dry: 0, containerCapacityBt20Reefer: 0, containerCapacityBt40Dry: 0, containerCapacityBt40Reefer: 0,
                isCapacity: true, deleted:false
            };
            target.groupList = target.resources.groupList;
            target.groupList.push(full);
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

        goMaker(): void {
            this.scrollTop();
            this.state.go(salesViewConfig.StateMaker);
        }

        selectGroup(group: apiModel.IGroupData): void {
            this.session.groupId = group.id;
            this.session.groupCode = group.code;
            this.session.groupName = group.name;

            this.resources.productPage.page = 0;
            this.resources.productPage.limit = 100;
            this.resources.getProductPage();
            this.scrollTop();
            this.resources.getSalesList(this.goSales, this);
        }

        goSales(target: GroupController): void {
            target.scrollTop();
            target.state.go(salesViewConfig.StateSales);
        }

        scrollTop(): void {
            $("html, body").animate({ scrollTop: 0 }, 500);
            // angular.element("body").animate({ scrollTop: 0 }, "fast");
        }
    }
    angular.module(salesViewConfig.AppName).controller(salesViewConfig.GroupController, GroupController);
}
