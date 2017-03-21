var managed;
(function (managed) {
    "use strict";
    var UserListController = (function () {
        function UserListController(resources, state) {
            this.resources = resources;
            this.state = state;
            this.searchText = {
                userName: "",
                name: "",
                email: ""
            };
            this.userPage = this.resources.userPage;
            this.resources.getUserList(this.setUserData, this);
            $.material.init();
        }
        UserListController.prototype.setUserData = function (target) {
            target.userList = target.resources.userList;
        };
        UserListController.prototype.changePage = function (page) {
            this.userPage.page = page;
            this.resources.getUserList(this.setUserData, this);
        };
        UserListController.prototype.hasUserRole = function () {
            return this.resources.hasUserRole();
        };
        UserListController.prototype.hasMakerRole = function () {
            return this.resources.hasMakerRole();
        };
        UserListController.prototype.goMenu = function () {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateMenu);
        };
        UserListController.prototype.goNew = function () {
            if (this.hasUserRole() === true) {
                angular.element("body").animate({ scrollTop: 0 }, "fast");
                this.state.go(managedConfig.StateUserNew);
            }
        };
        UserListController.prototype.goEdit = function (id) {
            if (this.hasUserRole() === true) {
                angular.element("body").animate({ scrollTop: 0 }, "fast");
                this.state.go(managedConfig.StateUserEdit, { param: id });
            }
        };
        UserListController.prototype.goRole = function (id) {
            if (this.hasUserRole() === true) {
                angular.element("body").animate({ scrollTop: 0 }, "fast");
                this.state.go(managedConfig.StateUserRole, { param: id });
            }
        };
        UserListController.prototype.goMaker = function (id) {
            if (this.hasMakerRole() === true) {
                angular.element("body").animate({ scrollTop: 0 }, "fast");
                this.state.go(managedConfig.StateUserMaker, { param: id });
            }
        };
        UserListController.prototype.refineBy = function () {
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
        };
        UserListController.$inject = [
            managedConfig.ManagedServiceName,
            "$state"
        ];
        return UserListController;
    }());
    managed.UserListController = UserListController;
    angular.module(managedConfig.AppName).controller(managedConfig.UserListController, UserListController);
})(managed || (managed = {}));
//# sourceMappingURL=userList.js.map