/// <reference path="../typings/tsd.d.ts" />
var managed;
(function (managed) {
    "use strict";
    var MenuController = (function () {
        function MenuController(resources, state, myData) {
            this.resources = resources;
            this.state = state;
            this.resources.resetUserPage();
            this.resources.resetProductPage();
            this.resources.resetGroupPage();
            $.material.init();
        }
        MenuController.prototype.hasUserRole = function () {
            return this.resources.hasUserRole();
        };
        MenuController.prototype.hasMakerRole = function () {
            return this.resources.hasMakerRole();
        };
        MenuController.prototype.hasGroupRole = function () {
            return this.resources.hasGroupRole();
        };
        MenuController.prototype.hasProductRole = function () {
            return this.resources.hasProductRole();
        };
        MenuController.prototype.userList = function () {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateUserList);
        };
        MenuController.prototype.makerList = function () {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateMakerList);
        };
        MenuController.prototype.groupList = function () {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateGroupList);
        };
        MenuController.prototype.productList = function () {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateProductList);
        };
        MenuController.$inject = [
            managedConfig.ManagedServiceName,
            "$state",
            "myData"
        ];
        return MenuController;
    }());
    managed.MenuController = MenuController;
    angular.module(managedConfig.AppName).controller(managedConfig.MenuController, MenuController);
})(managed || (managed = {}));
//# sourceMappingURL=menu.js.map