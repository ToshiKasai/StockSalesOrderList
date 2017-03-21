/// <reference path="../typings/tsd.d.ts" />

module managed {
    "use strict";

    export class GroupProductController {
        public static $inject = [
            managedConfig.ManagedServiceName,
            "$state",
            "$stateParams"
        ];

        id: string;
        groupEdit: apiModel.IElementGroupData;
        productList: apiModel.IElementProductData[];
        selectProduct: apiModel.IProductData[];

        constructor(
            private resources: managed.Resources,
            private state: ng.ui.IStateService,
            $stateParams: IExtendsStateParamsService
        ) {
            this.id = $stateParams.param;
            this.productList = this.resources.productList;
            this.groupEdit = this.resources.getGroupListById(this.id);
            this.selectProduct = [];

            this.resources.rest.one("groups", this.groupEdit.id).getList("products")
                .then((data: apiModel.IProductData[]): any => {
                    this.selectProduct = [];
                    let j: number = 0;
                    if (data.length > 0) {
                        for (let i: number = 0; i < this.productList.length; i++) {
                            //if (this.productList[i].makerModelId !== this.groupEdit.id) {
                            //    continue;
                            //}
                            if (this.productList[i].id === data[j].id) {
                                this.selectProduct.push(this.productList[i]);
                                j++;
                                if (j >= data.length) {
                                    break;
                                }
                            }
                        }
                    }
                });
            this.resources.productPage.limit = 100;
            this.resources.productPage.conditions = null;
            $.material.init();
        }

        goMenu(): void {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateMenu);
        }

        goList(): void {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateGroupList);
        }

        submit(): void {
            this.resources.rest.one("groups", this.groupEdit.id).post("products", this.selectProduct).then(
                (data: any): any => {
                    this.resources.toastr.info("更新しました。");
                    this.goList();
                },
                (res: ng.IHttpPromiseCallbackArg<any>): any => {
                    this.resources.toastr.error("エラーが発生し、更新が行えませんでした。");
                    angular.element("body").animate({ scrollTop: 0 }, "fast");
                }
            );
        }
    }
    angular.module(managedConfig.AppName).controller(managedConfig.GroupProductController, GroupProductController);
}
