/// <reference path="../typings/tsd.d.ts" />
var managed;
(function (managed) {
    "use strict";
    var UserEditController = (function () {
        function UserEditController(resources, state, $stateParams) {
            var _this = this;
            this.resources = resources;
            this.state = state;
            this.errShow = false;
            this.errMessage = "";
            this.id = $stateParams.param;
            this.userEdit = this.resources.getUserListById(this.id);
            this.userEdit.get().then(function (users) {
                _this.userEdit = users;
            });
            $.material.init();
        }
        UserEditController.prototype.goMenu = function () {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateMenu);
        };
        UserEditController.prototype.goList = function () {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateUserList);
        };
        UserEditController.prototype.submit = function () {
            var _this = this;
            this.errShow = false;
            this.errMessage = "";
            if (this.userEdit.email === "") {
                this.userEdit.email = null;
            }
            this.userEdit.put().then(function (data) {
                _this.resources.toastr.info("更新しました。");
                _this.goList();
            }, function (res) {
                _this.resources.toastr.error("エラーが発生し、更新が行えませんでした。");
                angular.element("body").animate({ scrollTop: 0 }, "fast");
                _this.errShow = true;
                _this.errMessage = _this.resources.errorMessage(res);
            });
        };
        UserEditController.$inject = [
            managedConfig.ManagedServiceName,
            "$state",
            "$stateParams"
        ];
        return UserEditController;
    }());
    managed.UserEditController = UserEditController;
    angular.module(managedConfig.AppName).controller(managedConfig.UserEditController, UserEditController);
})(managed || (managed = {}));
//# sourceMappingURL=userEdit.js.map