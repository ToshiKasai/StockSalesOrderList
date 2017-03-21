/// <reference path="../typings/tsd.d.ts" />

module salesView {
    "use strict";
    const app: ng.IModule = angular.module(salesViewConfig.AppName);

    app.config(["$stateProvider", function ($stateProvider: ng.ui.IStateProvider): any {
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
            })
            ;
    }]);

    app.config(["uiSelectConfig", function (uiSelectConfig: ng.ui.select.ISelectConfig): any {
        uiSelectConfig.theme = "bootstrap";
    }]);

    app.config(["RestangularProvider", function (RestangularProvider: restangular.IProvider): any {
        RestangularProvider.setBaseUrl("api");
    }]);

    interface ILoadingCustomBar extends ng.loadingBar.ILoadingBarProvider {
        parentSelector: string;
    }
    app.config(["cfpLoadingBarProvider", function (cfpLoadingBarProvider: ILoadingCustomBar): void {
        cfpLoadingBarProvider.includeSpinner = true;
        cfpLoadingBarProvider.includeBar = false;
        cfpLoadingBarProvider.latencyThreshold = 0;
        cfpLoadingBarProvider.spinnerTemplate = "<div class='nowloading'><div><h4>通信処理中</h4><small>しばらくお待ちください。</small></div></div>";
        cfpLoadingBarProvider.parentSelector = "#loading-bar-container";
    }]);

    app.filter("boolean", function (): any {
        return function (boolean: any, trueFormat: string, falseFormat: string): string {
            let word: string = "";
            if (boolean === true) {
                word = trueFormat;
            } else if (boolean === false) {
                word = falseFormat;
            }
            return word;
        };
    });

    app.filter("nullfilter", function (): any {
        return function (param: any, outstring: string): string {
            let word: string = "";
            if (param === null || param === undefined) {
                word = outstring;
            } else {
                word = param;
            }
            return word;
        };
    });

    app.filter("abbreviate", function (): any {
        return function (text: string, len: number, end: string): string {
            if (len === undefined) {
                len = 10;
            }
            if (end === undefined) {
                end = "…";
            }
            if (text !== undefined && text !== null) {
                if (text.length > len) {
                    return text.substring(0, len - 1) + end;
                } else {
                    return text;
                }
            }
        };
    });

    export interface IExtendsStateParamsService extends ng.ui.IStateParamsService {
        param: string;
    }

    class EditTagNumber implements ng.IDirective {
        public restrict: string;
        public require: string;
        public scope: any;

        public link: (scope: any, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, ngModel: ng.INgModelController) => void;
        constructor() {
            this.init();
        }
        private init(): void {
            this.restrict = "A";
            this.require = "^ngModel";
            this.scope = {
                callback: "&",
                ngModel: "="
            };
            this.link = (scope: any, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, ngModel: ng.INgModelController): void => {
                var fixed: string = "fixed";

                ngModel.$render = (): void => {
                    element.html(ngModel.$viewValue.replace(/(\d)(?=(?:\d{3}){2,}(?:\.|$))|(\d)(\d{3}(?:\.\d*)?$)/g, '$1$2,$3'));
                };
                ngModel.$parsers.push((value: any): number => {
                    var num: number;
                    var fixedNum: any = 0;
                    if (attrs[fixed] !== undefined && attrs[fixed] !== null) {
                        fixedNum = attrs[fixed];
                    }
                    if (typeof value === "number") {
                        num = this.floatFormat(value, fixedNum);
                    } else {
                        num = this.floatFormat(parseFloat(value), fixedNum);
                    }
                    return parseFloat(num.toFixed(fixedNum));
                });
                ngModel.$formatters.push((value: any): any => {
                    if (value === undefined) {
                        return NaN;
                    }
                    var num: number;
                    var fixedNum: any = 0;
                    if (attrs[fixed] !== undefined && attrs[fixed] !== null) {
                        fixedNum = attrs[fixed];
                    }
                    if (typeof value === "number") {
                        num = this.floatFormat(value, fixedNum);
                    } else {
                        num = this.floatFormat(parseFloat(value), fixedNum);
                    }
                    // return Number(num).toString().replace(/(\d)(?=(?:\d{3}){2,}(?:\.|$))|(\d)(\d{3}(?:\.\d*)?$)/g, '$1$2,$3');
                    return num.toFixed(fixedNum);
                    // return num.toFixed(fixedNum).replace(/(\d)(?=(?:\d{3}){2,}(?:\.|$))|(\d)(\d{3}(?:\.\d*)?$)/g, '$1$2,$3');
                });
                // element.on("dblclick", function (): void {
                element.on("click", function (): void {
                    var min: string = "min";
                    var step: string = "step";

                    var clickTarget: JQuery = angular.element(this);
                    var EDITING_PROP: string = "editing";
                    if (!clickTarget.hasClass(EDITING_PROP)) {
                        clickTarget.addClass(EDITING_PROP);

                        scope.oldEditNumber = ngModel.$viewValue;

                        var html: string = "<input type='number' value='" + ngModel.$viewValue + "'";
                        if (attrs[min] !== undefined && attrs[min] != null) {
                            html += " min='" + attrs[min] + "'";
                        }
                        if (attrs[step] !== undefined && attrs[step] != null) {
                            html += " step='" + attrs[step] + "'";
                        }
                        html += " class='form-control' style='font-size:12px;' />";
                        clickTarget.html(html);
                        var inputElement: JQuery = clickTarget.children();
                        inputElement.on("focus", function (): void {
                            inputElement.on("blur", function (): void {
                                var inputValue: any = inputElement.val() || this.defaultValue;
                                var fixedNum: any = 0;
                                if (attrs[fixed] !== undefined && attrs[fixed] !== null) {
                                    fixedNum = attrs[fixed];
                                }
                                var _pow: number = Math.pow(10, fixedNum);
                                var num: number = Math.round(parseFloat(inputValue) * _pow) / _pow;
                                inputValue = num.toFixed(fixedNum);
                                clickTarget.removeClass(EDITING_PROP).text(inputValue.replace(/(\d)(?=(?:\d{3}){2,}(?:\.|$))|(\d)(\d{3}(?:\.\d*)?$)/g, '$1$2,$3'));
                                inputElement.off();
                                scope.$apply(function (): void {
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
                var destroyWatcher: any = scope.$on("$destroy", (): void => {
                    if (angular.equals(destroyWatcher, null)) {
                        return;
                    }
                    element.off();
                    destroyWatcher();
                    destroyWatcher = null;
                });
            };
        }
        private floatFormat(value: number, n: number): number {
            var _pow: number = Math.pow(10, n);
            return Math.round(value * _pow) / _pow;
        }
    }

    app.directive("editTagNumber", [(): any => { return new EditTagNumber(); }]);


    angular.bootstrap(document.body, [app.name]);
}
