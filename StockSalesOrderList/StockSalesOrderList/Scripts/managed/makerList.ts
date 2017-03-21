/// <reference path="../typings/tsd.d.ts" />

module managed {
    "use strict";

    export class MakerListController {
        public static $inject = [
            managedConfig.ManagedServiceName,
            "$state"
        ];

        makerList: apiModel.IElementMakerData[];
        searchText: any;

        constructor(
            private resources: managed.Resources,
            private state: ng.ui.IStateService
        ) {
            this.searchText = {
                code: "",
                name: ""
            };
            this.makerList = [];
            this.resources.getMakerList(this.initialize, this);
            $.material.init();
        }

        initialize(target: MakerListController): any {
            target.makerList = target.resources.makerList;
        }

        hasMakerRole(): boolean {
            return this.resources.hasMakerRole();
        }

        goMenu(): void {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateMenu);
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

        makerSelected(id: string): void {
            let i: number = 0;
            for (i = 0; i < this.makerList.length; i++) {
                if (this.makerList[i].id === id) {

                    this.makerList[i].get().then(
                        (data: any) => {
                            this.makerList[i] = data;
                            this.makerList[i].enabled = this.makerList[i].enabled ? false : true;
                            this.makerList[i].put().then(
                                (data: any): any => {
                                    // this.resources.toastr.info("更新しました。");
                                },
                                (res: ng.IHttpPromiseCallbackArg<any>): any => {
                                    this.resources.toastr.error("申し訳ありませんが切り替えに失敗しました。");
                                    this.makerList[i].enabled = this.makerList[i].enabled ? false : true;
                                }
                            );
                        }
                    );

                    break;
                }
            }

        }
    }

    angular.module(managedConfig.AppName).controller(managedConfig.MakerListController, MakerListController);
}
