/// <reference path="../typings/tsd.d.ts" />
var managed;
(function (managed) {
    "use strict";
    var GroupEditController = (function () {
        function GroupEditController(resources, state, $stateParams) {
            var _this = this;
            this.resources = resources;
            this.state = state;
            this.errShow = false;
            this.errMessage = "";
            this.id = $stateParams.param;
            this.makerList = this.resources.makerList;
            this.containerList = this.resources.containerList;
            if (this.id === "0") {
                this.groupEdit = {
                    id: "0", code: null, name: null, makerModelId: null, makerCode: null, makerName: null,
                    containerModelId: null, containerName: null, isCapacity: true,
                    containerCapacityBt20Dry: 0, containerCapacityBt40Dry: 0,
                    containerCapacityBt20Reefer: 0, containerCapacityBt40Reefer: 0,
                    deleted: false
                };
            }
            else {
                this.groupEdit = this.resources.getGroupListById(this.id);
                this.isCapacity = this.groupEdit.isCapacity ? "cap" : "pal";
                var i = 0;
                for (i = 0; i < this.makerList.length; i++) {
                    if (this.makerList[i].id === this.groupEdit.makerModelId) {
                        this.selectMaker = this.makerList[i];
                        break;
                    }
                }
                for (i = 0; i < this.containerList.length; i++) {
                    if (this.containerList[i].id === this.groupEdit.containerModelId) {
                        this.selectContainer = this.containerList[i];
                        break;
                    }
                }
                this.groupEdit.get().then(function (groups) {
                    _this.groupEdit = groups;
                });
            }
            $.material.init();
        }
        GroupEditController.prototype.goMenu = function () {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateMenu);
        };
        GroupEditController.prototype.goList = function () {
            angular.element("body").animate({ scrollTop: 0 }, "fast");
            this.state.go(managedConfig.StateGroupList);
        };
        GroupEditController.prototype.makerChange = function ($item, $model) {
            if ($model === undefined) {
                this.groupEdit.makerModelId = "";
            }
            else {
                this.groupEdit.makerModelId = $model.id;
            }
        };
        GroupEditController.prototype.containerChange = function ($item, $model) {
            if ($model === undefined) {
                this.groupEdit.containerModelId = "";
            }
            else {
                this.groupEdit.containerModelId = $model.id;
            }
        };
        GroupEditController.prototype.submit = function () {
            var _this = this;
            this.errShow = false;
            this.errMessage = "";
            if (this.groupEdit.containerCapacityBt20Dry === null) {
                this.groupEdit.containerCapacityBt20Dry = 0;
            }
            if (this.groupEdit.containerCapacityBt40Dry === null) {
                this.groupEdit.containerCapacityBt40Dry = 0;
            }
            if (this.groupEdit.containerCapacityBt20Reefer === null) {
                this.groupEdit.containerCapacityBt20Reefer = 0;
            }
            if (this.groupEdit.containerCapacityBt40Reefer === null) {
                this.groupEdit.containerCapacityBt40Reefer = 0;
            }
            if (this.groupEdit.isCapacity === null) {
                this.groupEdit.isCapacity = true;
            }
            if (this.id === "0") {
                this.groupEdit.id = "0";
                this.resources.rest.all("groups").post(this.groupEdit).then(function (data) {
                    _this.resources.toastr.info("登録しました。");
                    _this.goList();
                }, function (res) {
                    _this.resources.toastr.error("エラーが発生し、登録が行えませんでした。");
                    angular.element("body").animate({ scrollTop: 0 }, "fast");
                    _this.errShow = true;
                    _this.errMessage = _this.resources.errorMessage(res);
                });
            }
            else {
                this.groupEdit.put().then(function (data) {
                    _this.resources.toastr.info("更新しました。");
                    _this.goList();
                }, function (res) {
                    _this.resources.toastr.error("エラーが発生し、更新が行えませんでした。");
                    angular.element("body").animate({ scrollTop: 0 }, "fast");
                    _this.errShow = true;
                    _this.errMessage = _this.resources.errorMessage(res);
                });
            }
        };
        GroupEditController.$inject = [
            managedConfig.ManagedServiceName,
            "$state",
            "$stateParams"
        ];
        return GroupEditController;
    }());
    managed.GroupEditController = GroupEditController;
    angular.module(managedConfig.AppName).controller(managedConfig.GroupEditController, GroupEditController);
})(managed || (managed = {}));
//# sourceMappingURL=groupEdit.js.map