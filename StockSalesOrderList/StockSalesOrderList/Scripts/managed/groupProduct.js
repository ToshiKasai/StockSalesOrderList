var managed;
(function (managed) {
    "use strict";
    var GroupProductController = (function () {
        function GroupProductController(resources, state, $stateParams) {
            var _this = this;
            this.resources = resources;
            this.state = state;
            this.id = $stateParams.param;
            this.productList = this.resources.productList;
            this.groupEdit = this.resources.getGroupListById(this.id);
            this.selectProduct = [];
            this.resources.rest.one("groups", this.groupEdit.id).getList("products")
                .then(function (data) {
                _this.selectProduct = [];
                var j = 0;
                if (data.length > 0) {
                    for (var i = 0; i < _this.productList.length; i++) {
                        if (_this.productList[i].id === data[j].id) {
                            _this.selectProduct.push(_this.productList[i]);
                            j++;
                            if (j >= data.length) {
                                break;
                            }
                        }
                    }
                }
            });
            this.resources.productPage.limit = 100;
            this.resources.productPage.conditions = null;
            $.material.init();
        }
        GroupProductController.prototype.goMenu = function () {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateMenu);
        };
        GroupProductController.prototype.goList = function () {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateGroupList);
        };
        GroupProductController.prototype.submit = function () {
            var _this = this;
            this.resources.rest.one("groups", this.groupEdit.id).post("products", this.selectProduct).then(function (data) {
                _this.resources.toastr.info("更新しました。");
                _this.goList();
            }, function (res) {
                _this.resources.toastr.error("エラーが発生し、更新が行えませんでした。");
                angular.element("body").animate({ scrollTop: 0 }, "fast");
            });
        };
        GroupProductController.$inject = [
            managedConfig.ManagedServiceName,
            "$state",
            "$stateParams"
        ];
        return GroupProductController;
    }());
    managed.GroupProductController = GroupProductController;
    angular.module(managedConfig.AppName).controller(managedConfig.GroupProductController, GroupProductController);
})(managed || (managed = {}));
//# sourceMappingURL=groupProduct.js.map