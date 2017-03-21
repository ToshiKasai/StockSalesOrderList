var baseService;
(function (baseService) {
    "use strict";
    var Session = (function () {
        function Session() {
            this.id = null;
            this.name = null;
            this.isMyMaker = true;
            this.makerId = null;
            this.groupId = null;
            this.makerCode = null;
            this.groupCode = null;
            this.makerName = null;
            this.groupName = null;
            this.productId = null;
            var nowDate = new Date();
            if (nowDate.getMonth() < 10) {
                this.year = nowDate.getFullYear() - 1;
            }
            else {
                this.year = nowDate.getFullYear();
            }
        }
        Session.prototype.resetYear = function () {
            var nowDate = new Date();
            if (nowDate.getMonth() < 10) {
                this.year = nowDate.getFullYear() - 1;
            }
            else {
                this.year = nowDate.getFullYear();
            }
        };
        return Session;
    }());
    baseService.Session = Session;
    var RADMIN = "admin";
    var RUSER = "user";
    var RMAKER = "maker";
    var RGROUP = "group";
    var RPRODUCT = "product";
    var HttpStatus200Text = "処理に成功しました。";
    var HttpStatus201Text = "新規登録処理に成功しました。";
    var HttpStatus204Text = "処理に成功しました。";
    var HttpStatus304Text = "前のデータから変更はありません。";
    var HttpStatus400Text = "パラメータが正しくありません。";
    var HttpStatus404Text = "データが見つかりません。";
    var HttpStatus409Text = "すでにデータがある等の理由で処理が行えません。";
    var HttpStatus412Text = "データが他ユーザーに変更された等の理由で最新ではありません。再度編集画面を呼び出して処理してください。";
    var HttpStatusOtherText = "何らかのエラーが発生しました。";
    var BaseService = (function () {
        function BaseService() {
            this.roles = [];
        }
        BaseService.prototype.hasAdminRole = function () {
            return this.roles.indexOf(RADMIN) !== -1;
        };
        BaseService.prototype.hasUserRole = function () {
            return this.hasAdminRole() || this.roles.indexOf(RUSER) !== -1;
        };
        BaseService.prototype.hasMakerRole = function () {
            return this.hasAdminRole() || this.roles.indexOf(RMAKER) !== -1;
        };
        BaseService.prototype.hasGroupRole = function () {
            return this.hasAdminRole() || this.roles.indexOf(RGROUP) !== -1;
        };
        BaseService.prototype.hasProductRole = function () {
            return this.hasAdminRole() || this.roles.indexOf(RPRODUCT) !== -1;
        };
        BaseService.prototype.errorMessageByCode = function (res) {
            var res_message = "";
            if (res === 200) {
                res_message = HttpStatus200Text;
            }
            else if (res === 201) {
                res_message = HttpStatus201Text;
            }
            else if (res === 204) {
                res_message = HttpStatus204Text;
            }
            else if (res === 304) {
                res_message = HttpStatus304Text;
            }
            else if (res === 400) {
                res_message = HttpStatus400Text;
            }
            else if (res === 404) {
                res_message = HttpStatus404Text;
            }
            else if (res === 409) {
                res_message = HttpStatus409Text;
            }
            else if (res === 412) {
                res_message = HttpStatus412Text;
            }
            else {
                res_message = HttpStatusOtherText;
            }
            return res_message;
        };
        BaseService.prototype.errorMessage = function (res) {
            var res_message = "";
            res_message = this.errorMessageByCode(res.status);
            if (res.status >= 400) {
                var errData = res.data;
                for (var i in errData.modelState) {
                    if (errData.modelState.hasOwnProperty(i)) {
                        res_message += errData.modelState[i];
                    }
                }
            }
            return res_message;
        };
        return BaseService;
    }());
    baseService.BaseService = BaseService;
})(baseService || (baseService = {}));
//# sourceMappingURL=baseService.js.map