/// <reference path="../typings/tsd.d.ts" />

module managed {
    "use strict";

    export class UserListController {
        public static $inject = [
            managedConfig.ManagedServiceName,
            "$state"
        ];

        userList: Array<apiModel.IUserData>;
        userPage: IPages;
        searchText: any;

        constructor(
            private resources: managed.Resources,
            private state: ng.ui.IStateService
        ) {
            this.searchText = {
                userName: "",
                name: "",
                email:""
            };
            this.userPage = this.resources.userPage;
            this.resources.getUserList(this.setUserData, this);
            $.material.init();
        }

        setUserData(target: UserListController): any {
            target.userList = target.resources.userList;
        }

        changePage(page: number): void {
            this.userPage.page = page;
            this.resources.getUserList(this.setUserData, this);
        }

        hasUserRole(): boolean {
            return this.resources.hasUserRole();
        }

        hasMakerRole(): boolean {
            return this.resources.hasMakerRole();
        }

        goMenu(): void {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateMenu);
        }

        goNew(): void {
            if (this.hasUserRole() === true) {
                angular.element("body").animate({ scrollTop: 0 }, "fast");
                this.state.go(managedConfig.StateUserNew);
            }
        }

        goEdit(id: string): void {
            if (this.hasUserRole() === true) {
                angular.element("body").animate({ scrollTop: 0 }, "fast");
                this.state.go(managedConfig.StateUserEdit, { param: id });
            }
        }

        goRole(id: string): void {
            if (this.hasUserRole() === true) {
                angular.element("body").animate({ scrollTop: 0 }, "fast");
                this.state.go(managedConfig.StateUserRole, { param: id });
            }
        }

        goMaker(id: string): void {
            if (this.hasMakerRole() === true) {
                angular.element("body").animate({ scrollTop: 0 }, "fast");
                this.state.go(managedConfig.StateUserMaker, { param: id });
            }
        }

        refineBy(): boolean {
            if (this.searchText.userName !== "") {
                return true;
            }
            if (this.searchText.name !== "") {
                return true;
            }
            if (this.searchText.email !== "") {
                return true;
            }
            return false;
        }
    }

    angular.module(managedConfig.AppName).controller(managedConfig.UserListController, UserListController);
}
