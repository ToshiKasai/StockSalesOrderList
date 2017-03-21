/// <reference path="../typings/tsd.d.ts" />

module managed {
    "use strict";

    export class ProductEditController {
        public static $inject = [
            managedConfig.ManagedServiceName,
            "$state",
            "$stateParams"
        ];

        id: string;
        productData: apiModel.IProductData;

        errShow: boolean;
        errMessage: string;

        constructor(
            private resources: managed.Resources,
            private state: ng.ui.IStateService,
            $stateParams: IExtendsStateParamsService
        ) {
            this.errShow = false;
            this.errMessage = null;


            this.id = $stateParams.param;
            // テスト用初期化
            this.productData = {
                id: this.id,
                code: "TEST001", name: "検証中", quantity: 1, makerModelId: "1", makerCode: "CODE001", makerName: "メーカー",
                isSoldWeight: false, enabled: true, deleted: false
            };
            this.productData = this.resources.getProductListById(this.id);

            (<apiModel.IElementProductData>this.productData).get().then(
                (product: apiModel.IElementProductData) => {
                    this.productData = product;
                }
            );
            $.material.init();
        }

        goMenu(): void {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateMenu);
        }

        goList(): void {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateProductList);
        }

        submit(): void {
            (<apiModel.IElementProductData>this.productData).put().then(
                (data: any): any => {
                    this.resources.toastr.info("更新しました。");
                    this.goList();
                },
                (res: ng.IHttpPromiseCallbackArg<any>): any => {
                    this.resources.toastr.error("エラーが発生し、更新が行えませんでした。");
                    angular.element("body").animate({ scrollTop: 0 }, "fast");
                    this.errShow = true;
                    this.errMessage = this.resources.errorMessage(res);
                }
            );
        }
    }

    angular.module(managedConfig.AppName).controller(managedConfig.ProductEditController, ProductEditController);
}
