var managed;
(function (managed) {
    "use strict";
    var UserMakerController = (function () {
        function UserMakerController(resources, state, $stateParams) {
            var _this = this;
            this.resources = resources;
            this.state = state;
            this.userEdit = this.resources.getUserListById($stateParams.param);
            this.makerList = this.resources.makerList;
            this.selectMaker = [];
            this.resources.rest.one("users", this.userEdit.id).getList("makers")
                .then(function (data) {
                _this.selectMaker = [];
                var j = 0;
                if (data.length > 0) {
                    for (var i = 0; i < _this.makerList.length; i++) {
                        if (_this.makerList[i].id === data[j].id) {
                            _this.selectMaker.push(_this.makerList[i]);
                            j++;
                            if (j >= data.length) {
                                break;
                            }
                        }
                    }
                }
            });
            $.material.init();
        }
        UserMakerController.prototype.goMenu = function () {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateMenu);
        };
        UserMakerController.prototype.goList = function () {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateUserList);
        };
        UserMakerController.prototype.submit = function () {
            var _this = this;
            this.resources.rest.one("users", this.userEdit.id).post("makers", this.selectMaker).then(function (data) {
                _this.resources.toastr.info("更新しました。");
                _this.goList();
            }, function (res) {
                _this.resources.toastr.error("エラーが発生し、更新が行えませんでした。");
                angular.element("body").animate({ scrollTop: 0 }, "fast");
            });
        };
        UserMakerController.$inject = [
            managedConfig.ManagedServiceName,
            "$state",
            "$stateParams"
        ];
        return UserMakerController;
    }());
    managed.UserMakerController = UserMakerController;
    angular.module(managedConfig.AppName).controller(managedConfig.UserMakerController, UserMakerController);
})(managed || (managed = {}));
//# sourceMappingURL=userMaker.js.map