/// <reference path="../typings/tsd.d.ts" />

module baseService {
    "use strict";

    export interface IErrorData {
        message: string;
        modelState: Object;
    }

    export class Session {
        public id?: string;
        public name: string;
        public isMyMaker: boolean;
        public makerId?: string;
        public makerCode?: string;
        public makerName?: string;
        public groupId?: string;
        public groupCode?: string;
        public groupName?: string;
        public productId?: string;
        public year: number;

        constructor() {
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
            let nowDate: Date = new Date();
            if (nowDate.getMonth() < 10) {
                this.year = nowDate.getFullYear();
            } else {
                this.year = nowDate.getFullYear() + 1;
            }
        }

        resetYear(): void {
            let nowDate: Date = new Date();
            if (nowDate.getMonth() < 10) {
                this.year = nowDate.getFullYear();
            } else {
                this.year = nowDate.getFullYear() + 1;
            }
        }
    }

    export interface IPages {
        start: number;
        count: number;
        pages: number;
        page: number;
        limit: number;
        conditions: string;
    }

    const RADMIN: string = "admin";
    const RUSER: string = "user";
    const RMAKER: string = "maker";
    const RGROUP: string = "group";
    const RPRODUCT: string = "product";

    const HttpStatus200Text: string = "処理に成功しました。";
    const HttpStatus201Text: string = "新規登録処理に成功しました。";
    const HttpStatus204Text: string = "処理に成功しました。";
    const HttpStatus304Text: string = "前のデータから変更はありません。";
    const HttpStatus400Text: string = "パラメータが正しくありません。";
    const HttpStatus404Text: string = "データが見つかりません。";
    const HttpStatus409Text: string = "すでにデータがある等の理由で処理が行えません。";
    const HttpStatus412Text: string = "データが他ユーザーに変更された等の理由で最新ではありません。再度編集画面を呼び出して処理してください。";
    const HttpStatusOtherText: string = "何らかのエラーが発生しました。";

    export class BaseService {
        roles: string[];

        constructor(
        ) {
            this.roles = [];
        }

        hasAdminRole(): boolean {
            return this.roles.indexOf(RADMIN) !== -1;
        }

        hasUserRole(): boolean {
            return this.hasAdminRole() || this.roles.indexOf(RUSER) !== -1;
        }

        hasMakerRole(): boolean {
            return this.hasAdminRole() || this.roles.indexOf(RMAKER) !== -1;
        }

        hasGroupRole(): boolean {
            return this.hasAdminRole() || this.roles.indexOf(RGROUP) !== -1;
        }

        hasProductRole(): boolean {
            return this.hasAdminRole() || this.roles.indexOf(RPRODUCT) !== -1;
        }

        errorMessageByCode(res: number): string {
            let res_message: string = "";
            if (res === 200) {
                res_message = HttpStatus200Text;
            } else if (res === 201) {
                res_message = HttpStatus201Text;
            } else if (res === 204) {
                res_message = HttpStatus204Text;
            } else if (res === 304) {
                res_message = HttpStatus304Text;
            } else if (res === 400) {
                res_message = HttpStatus400Text;
            } else if (res === 404) {
                res_message = HttpStatus404Text;
            } else if (res === 409) {
                res_message = HttpStatus409Text;
            } else if (res === 412) {
                res_message = HttpStatus412Text;
            } else {
                res_message = HttpStatusOtherText;
            }
            return res_message;
        }

        errorMessage(res: ng.IHttpPromiseCallbackArg<any>): string {
            let res_message: string = "";
            res_message = this.errorMessageByCode(res.status);
            if (res.status >= 400) {
                let errData: any = res.data;
                for (let i in errData.modelState) {
                    if (errData.modelState.hasOwnProperty(i)) {
                        res_message += errData.modelState[i];
                    }
                }
            }
            return res_message;
        }
    }
}
