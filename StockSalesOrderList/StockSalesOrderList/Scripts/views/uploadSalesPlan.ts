/// <reference path="../typings/tsd.d.ts" />

module uploadSalesPlan {
    "use strict";

    const app: ng.IModule = angular.module("myApp", ["ngFileUpload"]);

    class UploadController {
        public static $inject = [
            "Upload"
        ];

        selectTypes: string[];
        successMessage: string;
        errorMessage: string;
        updata: File;

        constructor(
            private Upload: angular.angularFileUpload.IUploadService
        ) {
            this.selectTypes = ["application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "application/vnd.ms-excel"];
            this.successMessage = null;
            this.errorMessage = null;
            this.updata = null;
            $.material.init();
        }

        getTypes(): string[] {
            return this.selectTypes;
        }

        hasUploadData(): boolean {
            if (this.updata === null) {
                return false;
            }
            if (this.updata.name === null) {
                return false;
            }
            if (this.updata.type !== "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                && this.updata.type !== "application/vnd.ms-excel") {
                return false;
            }
            return true;
        }

        change($files: any, $file: any, $newFiles: any, $duplicateFiles: any, $invalidFiles: any, $event: any): void {
            this.errorMessage = null;
            this.successMessage = null;
            if (this.updata === null) {
                this.updata = null;
                return;
            }
            if (this.updata.name === null) {
                this.updata = null;
                return;
            }
            if (this.updata.type !== "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                && this.updata.type !== "application/vnd.ms-excel") {
                this.errorMessage = "エクセルファイルを指定してください。";
                this.updata = null;
                return;
            }
            if (this.updata.size >= (2 * 1048576)) {
                this.errorMessage = "ファイルサイズが大きすぎます。２ＭＢ以内にしてください。\n";
                this.errorMessage += "選択ファイルは" + (this.updata.size / 1048576).toFixed(1) + "MBです。";
                this.updata = null;
                return;
            }
        }

        check_up(): void {
            if (this.hasUploadData()) {
                let cfg: angular.angularFileUpload.IFileUploadConfig;
                cfg = {
                    url: "",
                    method: "POST",
                    file: this.updata
                };

                this.Upload.upload(cfg).then(
                    (resp: ng.IHttpPromiseCallbackArg<any>) => {
                        this.updata = null;
                        this.successMessage = "アップロード成功しました。";
                    },
                    (reasone: ng.IHttpPromiseCallbackArg<any>) => {
                        this.updata = null;
                        this.errorMessage = "処理に失敗しました。" + reasone.data;
                    }
                );
            }
        }
    }

    app.controller("UploadController", UploadController);
}
