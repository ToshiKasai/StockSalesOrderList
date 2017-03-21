/// <reference path="../typings/tsd.d.ts" />
var managed;
(function (managed) {
    "use strict";
    var UserRoleEditController = (function () {
        function UserRoleEditController(resources, state, $stateParams) {
            var _this = this;
            this.resources = resources;
            this.state = state;
            this.userEdit = this.resources.getUserListById($stateParams.param);
            this.selectRole = [];
            this.roleList = this.resources.roleList;
            this.resources.rest.one("users", this.userEdit.id).getList("roles")
                .then(function (data) {
                _this.selectRole = [];
                if (data.length > 0) {
                    for (var i = 0; i < _this.roleList.length; i++) {
                        if (data.indexOf(_this.roleList[i].name) >= 0) {
                            _this.selectRole.push(_this.roleList[i]);
                        }
                    }
                }
            });
            $.material.init();
        }
        UserRoleEditController.prototype.goMenu = function () {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateMenu);
        };
        UserRoleEditController.prototype.goList = function () {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateUserList);
        };
        UserRoleEditController.prototype.submit = function () {
            var _this = this;
            var dataBody = new Array(0);
            for (var i = 0; i < this.selectRole.length; i++) {
                dataBody.push(this.selectRole[i].name);
            }
            this.resources.rest.one("users", this.userEdit.id).post("roles", dataBody).then(function (data) {
                _this.resources.toastr.info("更新しました。");
                _this.goList();
            }, function (res) {
                _this.resources.toastr.error("エラーが発生し、更新が行えませんでした。");
                angular.element("body").animate({ scrollTop: 0 }, "fast");
            });
        };
        UserRoleEditController.$inject = [
            managedConfig.ManagedServiceName,
            "$state",
            "$stateParams"
        ];
        return UserRoleEditController;
    }());
    managed.UserRoleEditController = UserRoleEditController;
    angular.module(managedConfig.AppName).controller(managedConfig.UserRoleController, UserRoleEditController);
})(managed || (managed = {}));
//# sourceMappingURL=userRole.js.map