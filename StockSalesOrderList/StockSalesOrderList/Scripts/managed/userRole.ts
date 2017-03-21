/// <reference path="../typings/tsd.d.ts" />

module managed {
    "use strict";

    export class UserRoleEditController {
        public static $inject = [
            managedConfig.ManagedServiceName,
            "$state",
            "$stateParams"
        ];

        userEdit: apiModel.IElementUserData;
        roleList: apiModel.IElementRoleData[];
        selectRole: apiModel.IRoleData[];

        constructor(
            private resources: managed.Resources,
            private state: ng.ui.IStateService,
            $stateParams: IExtendsStateParamsService
        ) {
            this.userEdit = this.resources.getUserListById($stateParams.param);
            this.selectRole = [];

            this.roleList = this.resources.roleList;

            this.resources.rest.one("users", this.userEdit.id).getList("roles")
                .then((data: Array<string>): any => {
                    this.selectRole = [];
                    if (data.length > 0) {
                        for (let i: number = 0; i < this.roleList.length; i++) {
                            if (data.indexOf(this.roleList[i].name) >= 0) {
                                this.selectRole.push(this.roleList[i]);
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
            var dataBody: Array<string> = new Array(0);
            for (var i: number = 0; i < this.selectRole.length; i++) {
                dataBody.push(this.selectRole[i].name);
            }
            this.resources.rest.one("users", this.userEdit.id).post("roles", dataBody).then(
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
    angular.module(managedConfig.AppName).controller(managedConfig.UserRoleController, UserRoleEditController);
}
