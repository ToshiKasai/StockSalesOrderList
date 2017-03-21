/// <reference path="../typings/tsd.d.ts" />
var managed;
(function (managed) {
    "use strict";
    var GroupListController = (function () {
        function GroupListController(resources, state) {
            this.resources = resources;
            this.state = state;
            this.searchText = {
                code: "",
                name: "",
                makerCode: ""
            };
            this.groupPage = this.resources.groupPage;
            this.groupList = [];
            this.resources.getGroupList(this.setGroupData, this);
            $.material.init();
        }
        GroupListController.prototype.displayPage = function () {
            var result = [];
            var s = this.groupPage.page - 2;
            if (s < 0) {
                s = 0;
            }
            var e = s + 5;
            if (e >= this.groupPage.pages) {
                e = this.groupPage.pages - 1;
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
        GroupListController.prototype.setGroupData = function (target) {
            target.groupList = target.resources.groupList;
        };
        GroupListController.prototype.changePage = function (page) {
            this.groupPage.page = page;
            this.resources.getGroupList(this.setGroupData, this);
        };
        GroupListController.prototype.jumpPage = function (jump) {
            this.groupPage.page += jump;
            if (this.groupPage.page < 0) {
                this.groupPage.page = 0;
            }
            if (this.groupPage.page >= this.groupPage.pages) {
                this.groupPage.page = this.groupPage.pages - 1;
            }
            this.resources.getGroupList(this.setGroupData, this);
        };
        GroupListController.prototype.goMenu = function () {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateMenu);
        };
        GroupListController.prototype.goNew = function () {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateGroupEdit, { param: "0" });
        };
        GroupListController.prototype.goEdit = function (id) {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateGroupEdit, { param: id });
        };
        GroupListController.prototype.goProduct = function (id, makerid) {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.productId = id;
            this.resources.productPage.limit = 1000;
            this.resources.productPage.conditions = makerid;
            this.resources.getProductList(this.goProductSet, this);
        };
        GroupListController.prototype.goProductSet = function (target) {
            target.state.go(managedConfig.StateGroupProduct, { param: target.productId });
        };
        GroupListController.prototype.refineBy = function () {
            if (this.searchText.code !== "") {
                return true;
            }
            if (this.searchText.name !== "") {
                return true;
            }
            if (this.searchText.makerCode !== "") {
                return true;
            }
            return false;
        };
        GroupListController.$inject = [
            managedConfig.ManagedServiceName,
            "$state"
        ];
        return GroupListController;
    }());
    managed.GroupListController = GroupListController;
    angular.module(managedConfig.AppName).controller(managedConfig.GroupListController, GroupListController);
})(managed || (managed = {}));
//# sourceMappingURL=groupList.js.map