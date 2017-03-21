var managed;
(function (managed) {
    "use strict";
    var ProductEditController = (function () {
        function ProductEditController(resources, state, $stateParams) {
            var _this = this;
            this.resources = resources;
            this.state = state;
            this.errShow = false;
            this.errMessage = null;
            this.id = $stateParams.param;
            this.productData = {
                id: this.id,
                code: "TEST001", name: "検証中", quantity: 1, makerModelId: "1", makerCode: "CODE001", makerName: "メーカー",
                isSoldWeight: false, enabled: true, deleted: false
            };
            this.productData = this.resources.getProductListById(this.id);
            this.productData.get().then(function (product) {
                _this.productData = product;
            });
            $.material.init();
        }
        ProductEditController.prototype.goMenu = function () {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateMenu);
        };
        ProductEditController.prototype.goList = function () {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateProductList);
        };
        ProductEditController.prototype.submit = function () {
            var _this = this;
            this.productData.put().then(function (data) {
                _this.resources.toastr.info("更新しました。");
                _this.goList();
            }, function (res) {
                _this.resources.toastr.error("エラーが発生し、更新が行えませんでした。");
                angular.element("body").animate({ scrollTop: 0 }, "fast");
                _this.errShow = true;
                _this.errMessage = _this.resources.errorMessage(res);
            });
        };
        ProductEditController.$inject = [
            managedConfig.ManagedServiceName,
            "$state",
            "$stateParams"
        ];
        return ProductEditController;
    }());
    managed.ProductEditController = ProductEditController;
    angular.module(managedConfig.AppName).controller(managedConfig.ProductEditController, ProductEditController);
})(managed || (managed = {}));
//# sourceMappingURL=productEdit.js.map