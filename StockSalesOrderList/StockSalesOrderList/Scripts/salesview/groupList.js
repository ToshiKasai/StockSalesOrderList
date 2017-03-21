/// <reference path="../typings/tsd.d.ts" />
var salesView;
(function (salesView) {
    "use strict";
    var GroupController = (function () {
        function GroupController(resources, state) {
            this.resources = resources;
            this.state = state;
            this.session = this.resources.session;
            this.searchText = {
                code: "",
                name: ""
            };
            this.yearList = this.resources.yearList;
            this.groupList = [];
            this.resources.getGroupList(this.groupListSet, this);
            $.material.init();
        }
        GroupController.prototype.groupListSet = function (target) {
            var full = {
                id: null, code: "******", name: "すべての商品",
                makerModelId: null, makerCode: null, makerName: null,
                containerModelId: null, containerName: null,
                containerCapacityBt20Dry: 0, containerCapacityBt20Reefer: 0, containerCapacityBt40Dry: 0, containerCapacityBt40Reefer: 0,
                isCapacity: true, deleted: false
            };
            target.groupList = target.resources.groupList;
            target.groupList.push(full);
        };
        GroupController.prototype.refineBy = function () {
            if (this.searchText.code !== "") {
                return true;
            }
            if (this.searchText.name !== "") {
                return true;
            }
            return false;
        };
        GroupController.prototype.goMaker = function () {
            this.scrollTop();
            this.state.go(salesViewConfig.StateMaker);
        };
        GroupController.prototype.selectGroup = function (group) {
            this.session.groupId = group.id;
            this.session.groupCode = group.code;
            this.session.groupName = group.name;
            this.resources.productPage.page = 0;
            this.resources.productPage.limit = 100;
            this.resources.getProductPage();
            this.scrollTop();
            this.resources.getSalesList(this.goSales, this);
        };
        GroupController.prototype.goSales = function (target) {
            target.scrollTop();
            target.state.go(salesViewConfig.StateSales);
        };
        GroupController.prototype.scrollTop = function () {
            $("html, body").animate({ scrollTop: 0 }, 500);
            // angular.element("body").animate({ scrollTop: 0 }, "fast");
        };
        GroupController.$inject = [
            salesViewConfig.ServiceName,
            "$state"
        ];
        return GroupController;
    }());
    salesView.GroupController = GroupController;
    angular.module(salesViewConfig.AppName).controller(salesViewConfig.GroupController, GroupController);
})(salesView || (salesView = {}));
//# sourceMappingURL=groupList.js.map