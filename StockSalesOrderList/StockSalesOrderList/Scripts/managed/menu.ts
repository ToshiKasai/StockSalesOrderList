/// <reference path="../typings/tsd.d.ts" />

module managed {
    "use strict";

    export class MenuController {
        public static $inject = [
            managedConfig.ManagedServiceName,
            "$state",
            "myData"
        ];

        constructor(
            private resources: managed.Resources,
            private state: ng.ui.IStateService,
            myData: any
        ) {
            this.resources.resetUserPage();
            this.resources.resetProductPage();
            this.resources.resetGroupPage();
            $.material.init();
        }

        hasUserRole(): boolean {
            return this.resources.hasUserRole();
        }

        hasMakerRole(): boolean {
            return this.resources.hasMakerRole();
        }

        hasGroupRole(): boolean {
            return this.resources.hasGroupRole();
        }

        hasProductRole(): boolean {
            return this.resources.hasProductRole();
        }

        userList(): void {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateUserList);
        }

        makerList(): void {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateMakerList);
        }

        groupList(): void {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateGroupList);
        }

        productList(): void {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateProductList);
        }

    }
    angular.module(managedConfig.AppName).controller(managedConfig.MenuController, MenuController);
}
