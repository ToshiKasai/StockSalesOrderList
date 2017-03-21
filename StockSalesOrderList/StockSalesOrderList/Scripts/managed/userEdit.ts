/// <reference path="../typings/tsd.d.ts" />

module managed {
    "use strict";

    export class UserEditController {
        public static $inject = [
            managedConfig.ManagedServiceName,
            "$state",
            "$stateParams"
        ];

        id: string;
        errShow: boolean = false;
        errMessage: string = "";
        userEdit: apiModel.IElementUserData;

        constructor(
            private resources: managed.Resources,
            private state: ng.ui.IStateService,
            $stateParams: IExtendsStateParamsService
        ) {
            this.id = $stateParams.param;
            this.userEdit = this.resources.getUserListById(this.id);
            this.userEdit.get().then(
                (users: apiModel.IElementUserData) => {
                    this.userEdit = users;
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
            this.state.go(managedConfig.StateUserList);
        }

        submit(): void {
            this.errShow = false;
            this.errMessage = "";

            if (this.userEdit.email === "") {
                this.userEdit.email = null;
            }

            this.userEdit.put().then(
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
    angular.module(managedConfig.AppName).controller(managedConfig.UserEditController, UserEditController);
}
