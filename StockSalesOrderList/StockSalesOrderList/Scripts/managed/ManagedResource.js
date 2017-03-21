var managed;
(function (managed) {
    "use strict";
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
    var app = angular.module(managedConfig.ServiceAppName, [
        "restangular",
        "toastr"
    ]);
    app.config(["RestangularProvider", function (RestangularProvider) {
            RestangularProvider.setBaseUrl("api");
        }]);
    var Resources = (function () {
        function Resources(restangular, toastrService) {
            this.restangular = restangular;
            this.toastrService = toastrService;
            this.roles = [];
            this.getMyRoles();
            this.makerList = [];
            this.getMakerList();
            this.containerList = [];
            this.getContainerList();
            this.userList = [];
            this.userPage = { start: 0, count: 0, pages: 0, page: 0, limit: 100, conditions: null };
            this.getUserPage();
            this.groupList = [];
            this.groupPage = { start: 0, count: 0, pages: 0, page: 0, limit: 100, conditions: null };
            this.getGroupPage();
            this.productList = [];
            this.productPage = { start: 0, count: 0, pages: 0, page: 0, limit: 100, conditions: null };
            this.getProductPage();
        }
        Resources.prototype.resetUserPage = function () {
            this.userPage.start = 0;
            this.userPage.page = 0;
            this.userPage.limit = 100;
            this.userPage.conditions = null;
        };
        Resources.prototype.resetGroupPage = function () {
            this.groupPage.start = 0;
            this.groupPage.page = 0;
            this.groupPage.limit = 100;
            this.groupPage.conditions = null;
        };
        Resources.prototype.resetProductPage = function () {
            this.productPage.start = 0;
            this.productPage.page = 0;
            this.productPage.limit = 100;
            this.productPage.conditions = null;
        };
        Resources.prototype.hasAdminRole = function () {
            return this.roles.indexOf(RADMIN) !== -1;
        };
        Resources.prototype.hasUserRole = function () {
            return this.hasAdminRole() || this.roles.indexOf(RUSER) !== -1;
        };
        Resources.prototype.hasMakerRole = function () {
            return this.hasAdminRole() || this.roles.indexOf(RMAKER) !== -1;
        };
        Resources.prototype.hasGroupRole = function () {
            return this.hasAdminRole() || this.roles.indexOf(RGROUP) !== -1;
        };
        Resources.prototype.hasProductRole = function () {
            return this.hasAdminRole() || this.roles.indexOf(RPRODUCT) !== -1;
        };
        Resources.prototype.errorMessageByCode = function (res) {
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
        Resources.prototype.errorMessage = function (res) {
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
        Object.defineProperty(Resources.prototype, "rest", {
            get: function () {
                return this.restangular;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Resources.prototype, "toastr", {
            get: function () {
                return this.toastrService;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Resources.prototype, "hasUserList", {
            get: function () {
                return (this.userList !== undefined);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Resources.prototype, "hasGroupList", {
            get: function () {
                return (this.groupList !== undefined);
            },
            enumerable: true,
            configurable: true
        });
        Resources.prototype.getMyRoles = function () {
            var _this = this;
            this.restangular.all("Accounts/Roles").getList().then(function (roles) {
                if (roles === undefined || roles === null) {
                    _this.roles = [];
                }
                else {
                    _this.roles = roles;
                }
            });
        };
        Resources.prototype.getMakerList = function (callback, target) {
            var _this = this;
            this.restangular.all("makers").getList({ deleted: true, enabled: false }).then(function (makers) {
                if (makers === undefined || makers === null) {
                    _this.makerList = [];
                }
                else {
                    _this.makerList = makers;
                }
                if (callback !== undefined && callback !== null) {
                    if (target !== undefined && target !== null) {
                        callback(target);
                    }
                    else {
                        callback();
                    }
                }
            });
            return this.makerList;
        };
        Resources.prototype.getRoleList = function (callback, target) {
            var _this = this;
            this.restangular.all("roles").getList().then(function (roles) {
                if (roles === undefined || roles === null) {
                    _this.roleList = [];
                }
                else {
                    _this.roleList = roles;
                }
                if (callback !== undefined && callback !== null) {
                    if (target !== undefined && target !== null) {
                        callback(target);
                    }
                    else {
                        callback();
                    }
                }
            });
            return this.roleList;
        };
        Resources.prototype.getContainerList = function (callback, target) {
            var _this = this;
            this.restangular.all("containers").getList().then(function (containers) {
                if (containers === undefined || containers === null) {
                    _this.containerList = [];
                }
                else {
                    _this.containerList = containers;
                }
                if (callback !== undefined && callback !== null) {
                    if (target !== undefined && target !== null) {
                        callback(target);
                    }
                    else {
                        callback();
                    }
                }
            });
            return this.containerList;
        };
        Resources.prototype.getUserPage = function (callback, target) {
            var _this = this;
            var result;
            if (this.hasUserRole()) {
                result = this.restangular.one("users/pages").get({ deleted: true, enabled: false, limit: this.userPage.limit });
            }
            else {
                result = this.restangular.one("users/pages").get({ limit: this.userPage.limit });
            }
            result.then(function (data) {
                if (data === undefined || data === null) {
                    _this.userPage.count = 0;
                    _this.userPage.pages = 0;
                }
                else {
                    _this.userPage.count = data.count;
                    _this.userPage.pages = data.pages;
                }
            });
            return this.userPage;
        };
        Resources.prototype.getUserList = function (callback, target) {
            var _this = this;
            var result;
            if (this.hasUserRole()) {
                result = this.restangular.all("users")
                    .getList({ deleted: true, enabled: false, limit: this.userPage.limit, page: this.userPage.page });
            }
            else {
                result = this.restangular.all("users").getList({ limit: this.userPage.limit, page: this.userPage.page });
            }
            result.then(function (users) {
                if (users === undefined || users === null) {
                    _this.userList = [];
                }
                else {
                    _this.userList = users;
                }
                if (callback !== undefined && callback !== null) {
                    if (target !== undefined && target !== null) {
                        callback(target);
                    }
                    else {
                        callback();
                    }
                }
            });
            return this.userList;
        };
        Resources.prototype.getUserListById = function (id) {
            if (this.hasUserList === false) {
                return null;
            }
            var count = this.userList.length;
            var i = 0;
            for (i = 0; i < count; i++) {
                if (this.userList[i].id === id) {
                    break;
                }
            }
            if (i >= count) {
                return null;
            }
            return this.userList[i];
        };
        Resources.prototype.getGroupPage = function (callback, target) {
            var _this = this;
            this.restangular.one("groups/pages").get({ deleted: true, enabled: false, limit: this.groupPage.limit }).then(function (data) {
                if (data === undefined || data === null) {
                    _this.groupPage.count = 0;
                    _this.groupPage.pages = 0;
                }
                else {
                    _this.groupPage.count = data.count;
                    _this.groupPage.pages = data.pages;
                }
            });
            return this.groupPage;
        };
        Resources.prototype.getGroupList = function (callback, target) {
            var _this = this;
            this.restangular.all("groups")
                .getList({ deleted: true, enabled: false, limit: this.groupPage.limit, page: this.groupPage.page }).then(function (groups) {
                if (groups === undefined || groups === null) {
                    _this.groupList = [];
                }
                else {
                    _this.groupList = groups;
                }
                if (callback !== undefined && callback !== null) {
                    if (target !== undefined && target !== null) {
                        callback(target);
                    }
                    else {
                        callback();
                    }
                }
            });
            return this.groupList;
        };
        Resources.prototype.getGroupListById = function (id) {
            if (this.hasGroupList === false) {
                return null;
            }
            var count = this.groupList.length;
            var i = 0;
            for (i = 0; i < count; i++) {
                if (this.groupList[i].id === id) {
                    break;
                }
            }
            if (i >= count) {
                return null;
            }
            return this.groupList[i];
        };
        Resources.prototype.getProductPage = function (callback, target) {
            var _this = this;
            var result;
            if (this.productPage.conditions === undefined || this.productPage.conditions === null) {
                result = this.restangular.one("products/pages")
                    .get({ deleted: true, enabled: false, limit: this.productPage.limit });
            }
            else {
                result = this.restangular.one("products/pages")
                    .get({ deleted: true, enabled: false, limit: this.productPage.limit, MakerId: this.productPage.conditions });
            }
            result.then(function (data) {
                if (data === undefined || data === null) {
                    _this.productPage.count = 0;
                    _this.productPage.pages = 0;
                }
                else {
                    _this.productPage.count = data.count;
                    _this.productPage.pages = data.pages;
                }
            });
            return this.productPage;
        };
        Resources.prototype.getProductList = function (callback, target) {
            var _this = this;
            var result;
            if (this.productPage.conditions === undefined || this.productPage.conditions === null) {
                result = this.restangular.all("products")
                    .getList({ deleted: true, enabled: false, limit: this.productPage.limit, page: this.productPage.page });
            }
            else {
                result = this.restangular.all("products")
                    .getList({
                    deleted: true, enabled: false,
                    limit: this.productPage.limit, page: this.productPage.page, MakerId: this.productPage.conditions
                });
            }
            result.then(function (products) {
                if (products === undefined || products === null) {
                    _this.productList = [];
                }
                else {
                    _this.productList = products;
                }
                if (callback !== undefined && callback !== null) {
                    if (target !== undefined && target !== null) {
                        callback(target);
                    }
                    else {
                        callback();
                    }
                }
            });
            return this.productList;
        };
        Resources.prototype.getProductListById = function (id) {
            var count = this.productList.length;
            var i = 0;
            for (i = 0; i < count; i++) {
                if (this.productList[i].id === id) {
                    break;
                }
            }
            if (i >= count) {
                return null;
            }
            return this.productList[i];
        };
        Resources.$inject = [
            "Restangular",
            "toastr"
        ];
        return Resources;
    }());
    managed.Resources = Resources;
    app.service(managedConfig.ManagedServiceName, Resources);
})(managed || (managed = {}));
//# sourceMappingURL=ManagedResource.js.map