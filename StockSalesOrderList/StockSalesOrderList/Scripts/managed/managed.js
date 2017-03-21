var managed;
(function (managed) {
    "use strict";
    var app = angular.module(managedConfig.AppName);
    app.config(["$stateProvider", function ($stateProvider) {
            $stateProvider
                .state(managedConfig.StateMenu, {
                url: "",
                templateUrl: "Managed/MenuView",
                controller: managedConfig.MenuController,
                controllerAs: "$ctrl",
                resolve: {
                    myData: [managedConfig.ManagedServiceName, function (resources) {
                            return resources.getRoleList();
                        }]
                }
            })
                .state(managedConfig.StateUserList, {
                templateUrl: "Managed/UserList",
                controller: managedConfig.UserListController,
                controllerAs: "$ctrl"
            })
                .state(managedConfig.StateUserEdit, {
                templateUrl: "Managed/UserEdit",
                controller: managedConfig.UserEditController,
                controllerAs: "$ctrl",
                params: {
                    param: null
                }
            })
                .state(managedConfig.StateUserNew, {
                templateUrl: "Managed/UserNew",
                controller: managedConfig.UserNewController,
                controllerAs: "$ctrl"
            })
                .state(managedConfig.StateUserRole, {
                templateUrl: "Managed/UserRole",
                controller: managedConfig.UserRoleController,
                controllerAs: "$ctrl",
                params: {
                    param: null
                }
            })
                .state(managedConfig.StateUserMaker, {
                templateUrl: "Managed/UserMaker",
                controller: managedConfig.UserMakerController,
                controllerAs: "$ctrl",
                params: {
                    param: null
                }
            })
                .state(managedConfig.StateMakerList, {
                templateUrl: "Managed/MakerList",
                controller: managedConfig.MakerListController,
                controllerAs: "$ctrl"
            })
                .state(managedConfig.StateGroupList, {
                templateUrl: "Managed/GroupList",
                controller: managedConfig.GroupListController,
                controllerAs: "$ctrl"
            })
                .state(managedConfig.StateGroupEdit, {
                templateUrl: "Managed/GroupEdit",
                controller: managedConfig.GroupEditController,
                controllerAs: "$ctrl",
                params: {
                    param: null
                }
            })
                .state(managedConfig.StateGroupProduct, {
                templateUrl: "Managed/GroupProduct",
                controller: managedConfig.GroupProductController,
                controllerAs: "$ctrl",
                params: {
                    param: null
                }
            })
                .state(managedConfig.StateProductList, {
                templateUrl: "Managed/ProductList",
                controller: managedConfig.ProductListController,
                controllerAs: "$ctrl"
            })
                .state(managedConfig.StateProductEdit, {
                templateUrl: "Managed/ProductEdit",
                controller: managedConfig.ProductEditController,
                controllerAs: "$ctrl",
                params: {
                    param: null
                }
            });
        }]);
    app.config(["uiSelectConfig", function (uiSelectConfig) {
            uiSelectConfig.theme = "bootstrap";
        }]);
    app.config(["cfpLoadingBarProvider", function (cfpLoadingBarProvider) {
            cfpLoadingBarProvider.includeSpinner = true;
            cfpLoadingBarProvider.includeBar = true;
            cfpLoadingBarProvider.latencyThreshold = 500;
            cfpLoadingBarProvider.spinnerTemplate = "<div class='nowloading'><div><h4>通信処理中</h4><small>しばらくお待ちください。</small></div></div>";
            cfpLoadingBarProvider.parentSelector = "#loading-bar-container";
        }]);
    app.filter("boolean", function () {
        return function (boolean, trueFormat, falseFormat) {
            var word = "";
            if (boolean === true) {
                word = trueFormat;
            }
            else if (boolean === false) {
                word = falseFormat;
            }
            return word;
        };
    });
    app.run(["$rootScope", function ($rootScope) {
            $rootScope.range = function (n) {
                var arr = [];
                for (var i = 0; i < n; ++i) {
                    arr.push(i);
                }
                return arr;
            };
        }]);
    angular.bootstrap(document.body, [app.name]);
})(managed || (managed = {}));
//# sourceMappingURL=managed.js.map