/// <reference path="../typings/tsd.d.ts" />

/**
 * 管理メニュー用リソースサービスの整理
 * 
 */
module managed {
    "use strict";

    const app: ng.IModule = angular.module(managedConfig.ServiceAppName, [
        "restangular",
        "toastr"
    ]);

    app.config(["RestangularProvider", function (RestangularProvider: restangular.IProvider): any {
        RestangularProvider.setBaseUrl("api");
    }]);

    export class Resources2 extends baseService.BaseService {
        public static $inject = [
            "Restangular",
            "toastr"
        ];

        // メーカーは全件取得
        makerList: apiModel.IElementMakerData[];
        // ロールは全件取得
        roleList: apiModel.IElementRoleData[];
        // コンテナは全件取得
        containerList: apiModel.IElementContainerData[];

        // ユーザーは部分取得
        userList: apiModel.IElementUserData[];
        userPage: IPages;

        // グループは部分取得
        groupList: apiModel.IElementGroupData[];
        groupPage: IPages;

        // 商品は部分取得(メーカー)
        productList: apiModel.IElementProductData[];
        productPage: IPages;

        constructor(
            private restangular: restangular.IService,
            private toastrService: angular.toastr.IToastrService
        ) {
            super();
            this.initialize();
        }

        private initialize(): void {
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

        resetUserPage(): void {
            this.userPage.start = 0;
            this.userPage.page = 0;
            this.userPage.limit = 100;
            this.userPage.conditions = null;
        }

        resetGroupPage(): void {
            this.groupPage.start = 0;
            this.groupPage.page = 0;
            this.groupPage.limit = 100;
            this.groupPage.conditions = null;
        }

        resetProductPage(): void {
            this.productPage.start = 0;
            this.productPage.page = 0;
            this.productPage.limit = 100;
            this.productPage.conditions = null;
        }

        get rest(): restangular.IService {
            return this.restangular;
        }

        get toastr(): angular.toastr.IToastrService {
            return this.toastrService;
        }

        get hasUserList(): Boolean {
            return (this.userList !== undefined);
        }

        get hasGroupList(): Boolean {
            return (this.groupList !== undefined);
        }

        getMyRoles(): void {
            this.restangular.all("Accounts/Roles").getList().then(
                (roles: string[]) => {
                    if (roles === undefined || roles === null) {
                        this.roles = [];
                    } else {
                        this.roles = roles;
                    }
                }
            );
        }

        getMakerList(callback?: Function, target?: any): apiModel.IElementMakerData[] {
            this.restangular.all("makers").getList({ deleted: true, enabled: false }).then(
                (makers: apiModel.IElementMakerData[]) => {
                    if (makers === undefined || makers === null) {
                        this.makerList = [];
                    } else {
                        this.makerList = makers;
                    }

                    if (callback !== undefined && callback !== null) {
                        if (target !== undefined && target !== null) {
                            callback(target);
                        } else {
                            callback();
                        }
                    }
                }
            );
            return this.makerList;
        }

        getRoleList(callback?: Function, target?: any): apiModel.IElementRoleData[] {
            this.restangular.all("roles").getList().then(
                (roles: apiModel.IElementRoleData[]) => {
                    if (roles === undefined || roles === null) {
                        this.roleList = [];
                    } else {
                        this.roleList = roles;
                    }

                    if (callback !== undefined && callback !== null) {
                        if (target !== undefined && target !== null) {
                            callback(target);
                        } else {
                            callback();
                        }
                    }
                }
            );
            return this.roleList;
        }

        getContainerList(callback?: Function, target?: any): apiModel.IElementContainerData[] {
            this.restangular.all("containers").getList().then(
                (containers: apiModel.IElementContainerData[]) => {
                    if (containers === undefined || containers === null) {
                        this.containerList = [];
                    } else {
                        this.containerList = containers;
                    }
                    if (callback !== undefined && callback !== null) {
                        if (target !== undefined && target !== null) {
                            callback(target);
                        } else {
                            callback();
                        }
                    }
                }
            );
            return this.containerList;
        }

        getUserPage(callback?: Function, target?: any): IPages {
            let result: restangular.IPromise<any>;
            if (this.hasUserRole()) {
                result = this.restangular.one("users/pages").get({ deleted: true, enabled: false, limit: this.userPage.limit });
            } else {
                result = this.restangular.one("users/pages").get({ limit: this.userPage.limit });
            }

            result.then(
                (data: any) => {
                    if (data === undefined || data === null) {
                        this.userPage.count = 0;
                        this.userPage.pages = 0;
                    } else {
                        this.userPage.count = data.count;
                        this.userPage.pages = data.pages;
                    }
                }
            );
            return this.userPage;
        }

        getUserList(callback?: Function, target?: any): apiModel.IElementUserData[] {
            let result: restangular.ICollectionPromise<any>;
            if (this.hasUserRole()) {
                result = this.restangular.all("users")
                    .getList({ deleted: true, enabled: false, limit: this.userPage.limit, page: this.userPage.page });
            } else {
                result = this.restangular.all("users").getList({ limit: this.userPage.limit, page: this.userPage.page });
            }
            result.then(
                (users: apiModel.IElementUserData[]) => {
                    if (users === undefined || users === null) {
                        this.userList = [];
                    } else {
                        this.userList = users;
                    }

                    if (callback !== undefined && callback !== null) {
                        if (target !== undefined && target !== null) {
                            callback(target);
                        } else {
                            callback();
                        }
                    }
                }
            );
            return this.userList;
        }

        getUserListById(id: string): apiModel.IElementUserData {
            if (this.hasUserList === false) {
                return null;
            }

            let count: number = this.userList.length;
            let i: number = 0;

            for (i = 0; i < count; i++) {
                if (this.userList[i].id === id) {
                    break;
                }
            }

            if (i >= count) {
                return null;
            }

            return this.userList[i];
        }

        getGroupPage(callback?: Function, target?: any): IPages {
            this.restangular.one("groups/pages").get({ deleted: true, enabled: false, limit: this.groupPage.limit }).then(
                (data: any) => {
                    if (data === undefined || data === null) {
                        this.groupPage.count = 0;
                        this.groupPage.pages = 0;
                    } else {
                        this.groupPage.count = data.count;
                        this.groupPage.pages = data.pages;
                    }
                }
            );
            return this.groupPage;
        }

        getGroupList(callback?: Function, target?: any): apiModel.IElementGroupData[] {
            this.restangular.all("groups")
                .getList({ deleted: true, enabled: false, limit: this.groupPage.limit, page: this.groupPage.page }).then(
                (groups: apiModel.IElementGroupData[]) => {
                    if (groups === undefined || groups === null) {
                        this.groupList = [];
                    } else {
                        this.groupList = groups;
                    }

                    if (callback !== undefined && callback !== null) {
                        if (target !== undefined && target !== null) {
                            callback(target);
                        } else {
                            callback();
                        }
                    }
                }
                );
            return this.groupList;
        }

        getGroupListById(id: string): apiModel.IElementGroupData {
            if (this.hasGroupList === false) {
                return null;
            }

            let count: number = this.groupList.length;
            let i: number = 0;

            for (i = 0; i < count; i++) {
                if (this.groupList[i].id === id) {
                    break;
                }
            }

            if (i >= count) {
                return null;
            }

            return this.groupList[i];
        }

        getProductPage(callback?: Function, target?: any): IPages {
            let result: restangular.IPromise<any>;
            if (this.productPage.conditions === undefined || this.productPage.conditions === null) {
                result = this.restangular.one("products/pages")
                    .get({ deleted: true, enabled: false, limit: this.productPage.limit });
            } else {
                result = this.restangular.one("products/pages")
                    .get({ deleted: true, enabled: false, limit: this.productPage.limit, MakerId: this.productPage.conditions });
            }
            result.then(
                (data: any) => {
                    if (data === undefined || data === null) {
                        this.productPage.count = 0;
                        this.productPage.pages = 0;
                    } else {
                        this.productPage.count = data.count;
                        this.productPage.pages = data.pages;
                    }
                }
            );
            return this.productPage;
        }

        getProductList(callback?: Function, target?: any): apiModel.IElementProductData[] {
            let result: restangular.IPromise<any>;
            if (this.productPage.conditions === undefined || this.productPage.conditions === null) {
                result = this.restangular.all("products")
                    .getList({ deleted: true, enabled: false, limit: this.productPage.limit, page: this.productPage.page });
            } else {
                result = this.restangular.all("products")
                    .getList({
                        deleted: true, enabled: false,
                        limit: this.productPage.limit, page: this.productPage.page, MakerId: this.productPage.conditions
                    });
            }

            result.then(
                (products: apiModel.IElementProductData[]) => {
                    if (products === undefined || products === null) {
                        this.productList = [];
                    } else {
                        this.productList = products;
                    }
                    if (callback !== undefined && callback !== null) {
                        if (target !== undefined && target !== null) {
                            callback(target);
                        } else {
                            callback();
                        }
                    }
                }
            );
            return this.productList;
        }
    }

    app.service(managedConfig.ManagedServiceName, Resources);
}
