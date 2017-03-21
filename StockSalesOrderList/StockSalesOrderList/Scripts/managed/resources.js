/// <reference path="../typings/tsd.d.ts" />
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
/**
 * 管理メニュー用リソースサービスの整理
 *
 */
var managed;
(function (managed) {
    "use strict";
    var app = angular.module(managedConfig.ServiceAppName, [
        "restangular",
        "toastr"
    ]);
    app.config(["RestangularProvider", function (RestangularProvider) {
            RestangularProvider.setBaseUrl("api");
        }]);
    var Resources2 = (function (_super) {
        __extends(Resources2, _super);
        function Resources2(restangular, toastrService) {
            _super.call(this);
            this.restangular = restangular;
            this.toastrService = toastrService;
            this.initialize();
        }
        Resources2.prototype.initialize = function () {
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
        };
        Resources2.prototype.resetUserPage = function () {
            this.userPage.start = 0;
            this.userPage.page = 0;
            this.userPage.limit = 100;
            this.userPage.conditions = null;
        };
        Resources2.prototype.resetGroupPage = function () {
            this.groupPage.start = 0;
            this.groupPage.page = 0;
            this.groupPage.limit = 100;
            this.groupPage.conditions = null;
        };
        Resources2.prototype.resetProductPage = function () {
            this.productPage.start = 0;
            this.productPage.page = 0;
            this.productPage.limit = 100;
            this.productPage.conditions = null;
        };
        Object.defineProperty(Resources2.prototype, "rest", {
            get: function () {
                return this.restangular;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Resources2.prototype, "toastr", {
            get: function () {
                return this.toastrService;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Resources2.prototype, "hasUserList", {
            get: function () {
                return (this.userList !== undefined);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Resources2.prototype, "hasGroupList", {
            get: function () {
                return (this.groupList !== undefined);
            },
            enumerable: true,
            configurable: true
        });
        Resources2.prototype.getMyRoles = function () {
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
        Resources2.prototype.getMakerList = function (callback, target) {
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
        Resources2.prototype.getRoleList = function (callback, target) {
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
        Resources2.prototype.getContainerList = function (callback, target) {
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
        Resources2.prototype.getUserPage = function (callback, target) {
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
        Resources2.prototype.getUserList = function (callback, target) {
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
        Resources2.prototype.getUserListById = function (id) {
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
        Resources2.prototype.getGroupPage = function (callback, target) {
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
        Resources2.prototype.getGroupList = function (callback, target) {
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
        Resources2.prototype.getGroupListById = function (id) {
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
        Resources2.prototype.getProductPage = function (callback, target) {
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
        Resources2.prototype.getProductList = function (callback, target) {
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
        Resources2.$inject = [
            "Restangular",
            "toastr"
        ];
        return Resources2;
    }(baseService.BaseService));
    managed.Resources2 = Resources2;
    app.service(managedConfig.ManagedServiceName, managed.Resources);
})(managed || (managed = {}));
//# sourceMappingURL=resources.js.map