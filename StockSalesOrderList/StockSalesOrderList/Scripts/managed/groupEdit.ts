/// <reference path="../typings/tsd.d.ts" />

module managed {
    "use strict";

    export class GroupEditController {
        public static $inject = [
            managedConfig.ManagedServiceName,
            "$state",
            "$stateParams"
        ];

        id: string;
        errShow: boolean = false;
        errMessage: string = "";
        groupEdit: apiModel.IGroupData;
        makerList: apiModel.IMakerData[];
        containerList: apiModel.IContainerData[];
        selectMaker: apiModel.IMakerData;
        selectContainer: apiModel.IContainerData;
        isCapacity: string;

        constructor(
            private resources: managed.Resources,
            private state: ng.ui.IStateService,
            $stateParams: IExtendsStateParamsService
        ) {
            this.id = $stateParams.param;
            this.makerList = this.resources.makerList;
            this.containerList = this.resources.containerList;

            if (this.id === "0") {
                this.groupEdit = {
                    id: "0", code: null, name: null, makerModelId: null, makerCode: null, makerName: null,
                    containerModelId: null, containerName: null, isCapacity: true,
                    containerCapacityBt20Dry: 0, containerCapacityBt40Dry: 0,
                    containerCapacityBt20Reefer: 0, containerCapacityBt40Reefer: 0,
                    deleted: false
                };
            } else {
                this.groupEdit = this.resources.getGroupListById(this.id);
                this.isCapacity = this.groupEdit.isCapacity ? "cap" : "pal";

                let i:number = 0;
                for (i = 0; i < this.makerList.length; i++) {
                    if (this.makerList[i].id === this.groupEdit.makerModelId) {
                        this.selectMaker = this.makerList[i];
                        break;
                    }
                }
                for (i = 0; i < this.containerList.length; i++) {
                    if (this.containerList[i].id === this.groupEdit.containerModelId) {
                        this.selectContainer = this.containerList[i];
                        break;
                    }
                }

                (<apiModel.IElementGroupData>this.groupEdit).get().then(
                    (groups: apiModel.IElementGroupData) => {
                        this.groupEdit = groups;
                    }
                );
            }

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

        makerChange($item: any, $model: any): void {
            if ($model === undefined) {
                this.groupEdit.makerModelId = "";
            } else {
                this.groupEdit.makerModelId = $model.id;
            }
        }

        containerChange($item: any, $model: any): void {
            if ($model === undefined) {
                this.groupEdit.containerModelId = "";
            } else {
                this.groupEdit.containerModelId = $model.id;
            }
        }

        submit(): void {
            this.errShow = false;
            this.errMessage = "";

            if (this.groupEdit.containerCapacityBt20Dry === null) {
                this.groupEdit.containerCapacityBt20Dry = 0;
            }
            if (this.groupEdit.containerCapacityBt40Dry === null) {
                this.groupEdit.containerCapacityBt40Dry = 0;
            }
            if (this.groupEdit.containerCapacityBt20Reefer === null) {
                this.groupEdit.containerCapacityBt20Reefer = 0;
            }
            if (this.groupEdit.containerCapacityBt40Reefer === null) {
                this.groupEdit.containerCapacityBt40Reefer = 0;
            }
            if (this.groupEdit.isCapacity === null) {
                this.groupEdit.isCapacity = true;
            }

            if (this.id === "0") {
                this.groupEdit.id = "0";
                this.resources.rest.all("groups").post(this.groupEdit).then(
                    (data: any): any => {
                        this.resources.toastr.info("登録しました。");
                        this.goList();
                    },
                    (res: ng.IHttpPromiseCallbackArg<any>): any => {
                        this.resources.toastr.error("エラーが発生し、登録が行えませんでした。");
                        angular.element("body").animate({ scrollTop: 0 }, "fast");
                        this.errShow = true;
                        this.errMessage = this.resources.errorMessage(res);
                    }
                );
            } else {
                (<apiModel.IElementGroupData>this.groupEdit).put().then(
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
    }
    angular.module(managedConfig.AppName).controller(managedConfig.GroupEditController, GroupEditController);
}
