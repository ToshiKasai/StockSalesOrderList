var managed;
(function (managed) {
    "use strict";
    var MakerListController = (function () {
        function MakerListController(resources, state) {
            this.resources = resources;
            this.state = state;
            this.searchText = {
                code: "",
                name: ""
            };
            this.makerList = [];
            this.resources.getMakerList(this.initialize, this);
            $.material.init();
        }
        MakerListController.prototype.initialize = function (target) {
            target.makerList = target.resources.makerList;
        };
        MakerListController.prototype.hasMakerRole = function () {
            return this.resources.hasMakerRole();
        };
        MakerListController.prototype.goMenu = function () {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateMenu);
        };
        MakerListController.prototype.refineBy = function () {
            if (this.searchText.code !== "") {
                return true;
            }
            if (this.searchText.name !== "") {
                return true;
            }
            return false;
        };
        MakerListController.prototype.makerSelected = function (id) {
            var _this = this;
            var i = 0;
            for (i = 0; i < this.makerList.length; i++) {
                if (this.makerList[i].id === id) {
                    this.makerList[i].get().then(function (data) {
                        _this.makerList[i] = data;
                        _this.makerList[i].enabled = _this.makerList[i].enabled ? false : true;
                        _this.makerList[i].put().then(function (data) {
                        }, function (res) {
                            _this.resources.toastr.error("申し訳ありませんが切り替えに失敗しました。");
                            _this.makerList[i].enabled = _this.makerList[i].enabled ? false : true;
                        });
                    });
                    break;
                }
            }
        };
        MakerListController.$inject = [
            managedConfig.ManagedServiceName,
            "$state"
        ];
        return MakerListController;
    }());
    managed.MakerListController = MakerListController;
    angular.module(managedConfig.AppName).controller(managedConfig.MakerListController, MakerListController);
})(managed || (managed = {}));
//# sourceMappingURL=makerList.js.map