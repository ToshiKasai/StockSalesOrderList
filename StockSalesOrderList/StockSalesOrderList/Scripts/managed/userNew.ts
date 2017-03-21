/// <reference path="../typings/tsd.d.ts" />

module managed {
    "use strict";

    export class UserNewController {
        public static $inject = [
            managedConfig.ManagedServiceName,
            "$state",
            "$stateParams"
        ];

        errShow: boolean = false;
        errMessage: string = "";
        userNew: apiModel.IUserData;

        constructor(
            private resources: managed.Resources,
            private state: ng.ui.IStateService,
            $stateParams: IExtendsStateParamsService
        ) {
            this.userNew = {
                userName: null,
                name: null,
                email: null,
                enabled: true,
                newPassword: null,
                expiration: null, passwordSkipCnt: 0, emailConfirmed: false,
                lockoutEnabled: true, accessFailedCount: 0, deleted: false
            };
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

            this.resources.rest.all("users").post(this.userNew).then(
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
        }
    }
    angular.module(managedConfig.AppName).controller(managedConfig.UserNewController, UserNewController);
}
