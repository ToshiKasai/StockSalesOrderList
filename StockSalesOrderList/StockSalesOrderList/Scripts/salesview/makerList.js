var salesView;
(function (salesView) {
    "use strict";
    var MakerController = (function () {
        function MakerController(resources, state) {
            var _this = this;
            this.resources = resources;
            this.state = state;
            this.session = this.resources.session;
            this.searchText = {
                code: "",
                name: ""
            };
            this.resources.getMakerList(this.makerListSet, this);
            setTimeout(function () { _this.sessionWait(_this); }, 100);
            $.material.init();
        }
        MakerController.prototype.sessionWait = function (target) {
            var _this = this;
            if (target.session.id === null) {
                setTimeout(function () { target.sessionWait(_this); }, 100);
                return;
            }
            target.resources.getUserMakerList(target.session.id, target.myMakerListSet, target);
        };
        MakerController.prototype.makerListSet = function (target) {
            target.makerList = target.resources.makerList;
        };
        MakerController.prototype.myMakerListSet = function (target) {
            target.myMakers = target.resources.userMakers;
        };
        MakerController.prototype.refineBy = function () {
            if (this.searchText.code !== "") {
                return true;
            }
            if (this.searchText.name !== "") {
                return true;
            }
            return false;
        };
        MakerController.prototype.checkMyMaker = function (id) {
            var i;
            if (this.session.isMyMaker === false) {
                return true;
            }
            if (this.myMakers === undefined || this.myMakers === null) {
                return false;
            }
            for (i = 0; i < this.myMakers.length; i++) {
                if (this.myMakers[i].id === id) {
                    return true;
                }
            }
            return false;
        };
        MakerController.prototype.selectMaker = function (maker) {
            this.session.makerId = maker.id;
            this.session.makerCode = maker.code;
            this.session.makerName = maker.name;
            this.scrollTop();
            this.state.go(salesViewConfig.StateGroup);
        };
        MakerController.prototype.scrollTop = function () {
            $("html, body").animate({ scrollTop: 0 }, 500);
        };
        MakerController.$inject = [
            salesViewConfig.ServiceName,
            "$state"
        ];
        return MakerController;
    }());
    salesView.MakerController = MakerController;
    angular.module(salesViewConfig.AppName).controller(salesViewConfig.MakerController, MakerController);
})(salesView || (salesView = {}));
//# sourceMappingURL=makerList.js.map