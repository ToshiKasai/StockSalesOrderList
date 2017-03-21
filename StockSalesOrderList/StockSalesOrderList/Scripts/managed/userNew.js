/// <reference path="../typings/tsd.d.ts" />
var managed;
(function (managed) {
    "use strict";
    var UserNewController = (function () {
        function UserNewController(resources, state, $stateParams) {
            this.resources = resources;
            this.state = state;
            this.errShow = false;
            this.errMessage = "";
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
        UserNewController.prototype.goMenu = function () {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateMenu);
        };
        UserNewController.prototype.goList = function () {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateUserList);
        };
        UserNewController.prototype.submit = function () {
            var _this = this;
            this.errShow = false;
            this.errMessage = "";
            this.resources.rest.all("users").post(this.userNew).then(function (data) {
                _this.resources.toastr.info("登録しました。");
                _this.goList();
            }, function (res) {
                _this.resources.toastr.error("エラーが発生し、登録が行えませんでした。");
                angular.element("body").animate({ scrollTop: 0 }, "fast");
                _this.errShow = true;
                _this.errMessage = _this.resources.errorMessage(res);
            });
        };
        UserNewController.$inject = [
            managedConfig.ManagedServiceName,
            "$state",
            "$stateParams"
        ];
        return UserNewController;
    }());
    managed.UserNewController = UserNewController;
    angular.module(managedConfig.AppName).controller(managedConfig.UserNewController, UserNewController);
})(managed || (managed = {}));
//# sourceMappingURL=userNew.js.map