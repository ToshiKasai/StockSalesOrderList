var managed;
(function (managed) {
    "use strict";
    var ProductListController = (function () {
        function ProductListController(resources, state) {
            this.resources = resources;
            this.state = state;
            this.searchText = {
                code: "",
                name: ""
            };
            this.productPage = this.resources.productPage;
            this.productList = [];
            this.resources.getProductList(this.productData, this);
            $.material.init();
        }
        ProductListController.prototype.displayPage = function () {
            var result = [];
            var s = this.productPage.page - 2;
            if (s < 0) {
                s = 0;
            }
            var e = s + 5;
            if (e >= this.productPage.pages) {
                e = this.productPage.pages - 1;
                s = e - 5;
                if (s < 0) {
                    s = 0;
                }
            }
            for (; s <= e; s++) {
                result.push(s);
            }
            return result;
        };
        ProductListController.prototype.productData = function (target) {
            target.productList = target.resources.productList;
        };
        ProductListController.prototype.changePage = function (page) {
            this.productPage.page = page;
            this.resources.getProductList(this.productData, this);
        };
        ProductListController.prototype.jumpPage = function (jump) {
            this.productPage.page += jump;
            if (this.productPage.page < 0) {
                this.productPage.page = 0;
            }
            if (this.productPage.page >= this.productPage.pages) {
                this.productPage.page = this.productPage.pages - 1;
            }
            this.resources.getProductList(this.productData, this);
        };
        ProductListController.prototype.goMenu = function () {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateMenu);
        };
        ProductListController.prototype.goEdit = function (id) {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateProductEdit, { param: id });
        };
        ProductListController.prototype.refineBy = function () {
            if (this.searchText.code !== "") {
                return true;
            }
            if (this.searchText.name !== "") {
                return true;
            }
            return false;
        };
        ProductListController.$inject = [
            managedConfig.ManagedServiceName,
            "$state"
        ];
        return ProductListController;
    }());
    managed.ProductListController = ProductListController;
    angular.module(managedConfig.AppName).controller(managedConfig.ProductListController, ProductListController);
})(managed || (managed = {}));
//# sourceMappingURL=productList.js.map