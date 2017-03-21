var salesView;
(function (salesView) {
    "use strict";
    var app = angular.module(salesViewConfig.AppName);
    app.config(["$stateProvider", function ($stateProvider) {
            $stateProvider
                .state(salesViewConfig.StateMaker, {
                url: "",
                templateUrl: "SalesView/MakerView",
                controller: salesViewConfig.MakerController,
                controllerAs: "$ctrl"
            })
                .state(salesViewConfig.StateGroup, {
                templateUrl: "SalesView/GroupView",
                controller: salesViewConfig.GroupController,
                controllerAs: "$ctrl"
            })
                .state(salesViewConfig.StateSales, {
                templateUrl: "SalesView/SalesView",
                controller: salesViewConfig.SalesController,
                controllerAs: "$ctrl"
            })
                .state(salesViewConfig.StateProduct, {
                templateUrl: "SalesView/ProcustDetail",
                controller: salesViewConfig.ProductController,
                controllerAs: "$ctrl"
            });
        }]);
    app.config(["uiSelectConfig", function (uiSelectConfig) {
            uiSelectConfig.theme = "bootstrap";
        }]);
    app.config(["RestangularProvider", function (RestangularProvider) {
            RestangularProvider.setBaseUrl("api");
        }]);
    app.config(["cfpLoadingBarProvider", function (cfpLoadingBarProvider) {
            cfpLoadingBarProvider.includeSpinner = true;
            cfpLoadingBarProvider.includeBar = false;
            cfpLoadingBarProvider.latencyThreshold = 0;
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
    app.filter("nullfilter", function () {
        return function (param, outstring) {
            var word = "";
            if (param === null || param === undefined) {
                word = outstring;
            }
            else {
                word = param;
            }
            return word;
        };
    });
    app.filter("abbreviate", function () {
        return function (text, len, end) {
            if (len === undefined) {
                len = 10;
            }
            if (end === undefined) {
                end = "…";
            }
            if (text !== undefined && text !== null) {
                if (text.length > len) {
                    return text.substring(0, len - 1) + end;
                }
                else {
                    return text;
                }
            }
        };
    });
    var EditTagNumber = (function () {
        function EditTagNumber() {
            this.init();
        }
        EditTagNumber.prototype.init = function () {
            var _this = this;
            this.restrict = "A";
            this.require = "^ngModel";
            this.scope = {
                callback: "&",
                ngModel: "="
            };
            this.link = function (scope, element, attrs, ngModel) {
                var fixed = "fixed";
                ngModel.$render = function () {
                    element.html(ngModel.$viewValue.replace(/(\d)(?=(?:\d{3}){2,}(?:\.|$))|(\d)(\d{3}(?:\.\d*)?$)/g, '$1$2,$3'));
                };
                ngModel.$parsers.push(function (value) {
                    var num;
                    var fixedNum = 0;
                    if (attrs[fixed] !== undefined && attrs[fixed] !== null) {
                        fixedNum = attrs[fixed];
                    }
                    if (typeof value === "number") {
                        num = _this.floatFormat(value, fixedNum);
                    }
                    else {
                        num = _this.floatFormat(parseFloat(value), fixedNum);
                    }
                    return parseFloat(num.toFixed(fixedNum));
                });
                ngModel.$formatters.push(function (value) {
                    if (value === undefined) {
                        return NaN;
                    }
                    var num;
                    var fixedNum = 0;
                    if (attrs[fixed] !== undefined && attrs[fixed] !== null) {
                        fixedNum = attrs[fixed];
                    }
                    if (typeof value === "number") {
                        num = _this.floatFormat(value, fixedNum);
                    }
                    else {
                        num = _this.floatFormat(parseFloat(value), fixedNum);
                    }
                    return num.toFixed(fixedNum);
                });
                element.on("click", function () {
                    var min = "min";
                    var step = "step";
                    var clickTarget = angular.element(this);
                    var EDITING_PROP = "editing";
                    if (!clickTarget.hasClass(EDITING_PROP)) {
                        clickTarget.addClass(EDITING_PROP);
                        scope.oldEditNumber = ngModel.$viewValue;
                        var html = "<input type='number' value='" + ngModel.$viewValue + "'";
                        if (attrs[min] !== undefined && attrs[min] != null) {
                            html += " min='" + attrs[min] + "'";
                        }
                        if (attrs[step] !== undefined && attrs[step] != null) {
                            html += " step='" + attrs[step] + "'";
                        }
                        html += " class='form-control' style='font-size:12px;' />";
                        clickTarget.html(html);
                        var inputElement = clickTarget.children();
                        inputElement.on("focus", function () {
                            inputElement.on("blur", function () {
                                var inputValue = inputElement.val() || this.defaultValue;
                                var fixedNum = 0;
                                if (attrs[fixed] !== undefined && attrs[fixed] !== null) {
                                    fixedNum = attrs[fixed];
                                }
                                var _pow = Math.pow(10, fixedNum);
                                var num = Math.round(parseFloat(inputValue) * _pow) / _pow;
                                inputValue = num.toFixed(fixedNum);
                                clickTarget.removeClass(EDITING_PROP).text(inputValue.replace(/(\d)(?=(?:\d{3}){2,}(?:\.|$))|(\d)(\d{3}(?:\.\d*)?$)/g, '$1$2,$3'));
                                inputElement.off();
                                scope.$apply(function () {
                                    ngModel.$setViewValue(inputValue);
                                });
                                if (scope.oldEditNumber !== ngModel.$viewValue) {
                                    if (scope.callback !== undefined && scope.callback != null) {
                                        scope.callback();
                                        scope.$apply();
                                    }
                                }
                            });
                        });
                        inputElement[0].focus();
                    }
                });
                var destroyWatcher = scope.$on("$destroy", function () {
                    if (angular.equals(destroyWatcher, null)) {
                        return;
                    }
                    element.off();
                    destroyWatcher();
                    destroyWatcher = null;
                });
            };
        };
        EditTagNumber.prototype.floatFormat = function (value, n) {
            var _pow = Math.pow(10, n);
            return Math.round(value * _pow) / _pow;
        };
        return EditTagNumber;
    }());
    app.directive("editTagNumber", [function () { return new EditTagNumber(); }]);
    angular.bootstrap(document.body, [app.name]);
})(salesView || (salesView = {}));
//# sourceMappingURL=router.js.map