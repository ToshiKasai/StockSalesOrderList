/// <reference path="../typings/tsd.d.ts" />

module managed {
    "use strict";

    export class UserMakerController {
        public static $inject = [
            managedConfig.ManagedServiceName,
            "$state",
            "$stateParams"
        ];

        userEdit: apiModel.IElementUserData;
        makerList: apiModel.IElementMakerData[];
        selectMaker: apiModel.IMakerData[];

        constructor(
            private resources: managed.Resources,
            private state: ng.ui.IStateService,
            $stateParams: IExtendsStateParamsService
        ) {
            this.userEdit = this.resources.getUserListById($stateParams.param);
            this.makerList = this.resources.makerList;
            this.selectMaker = [];

            this.resources.rest.one("users", this.userEdit.id).getList("makers")
                .then((data: apiModel.IMakerData[]): any => {
                    this.selectMaker = [];
                    let j: number = 0;
                    if (data.length > 0) {
                        for (let i: number = 0; i < this.makerList.length; i++) {
                            if (this.makerList[i].id === data[j].id) {
                                this.selectMaker.push(this.makerList[i]);
                                j++;
                                if (j >= data.length) {
                                    break;
                                }
                            }
                        }
                    }
                });
            $.material.init();
        }

        goMenu(): void {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateMenu);
        }

        goList(): void {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateUserList);
        }

        submit(): void {
            this.resources.rest.one("users", this.userEdit.id).post("makers", this.selectMaker).then(
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
    angular.module(managedConfig.AppName).controller(managedConfig.UserMakerController, UserMakerController);
}
