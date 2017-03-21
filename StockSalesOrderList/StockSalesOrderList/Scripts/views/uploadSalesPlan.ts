/// <reference path="../typings/tsd.d.ts" />

module uploadSalesPlan {
    "use strict";

    const app: ng.IModule = angular.module("myApp", ["ngFileUpload"]);

    class UploadController {
        public static $inject = [
            "Upload"
        ];

        selectTypes: string[];
        successFlg: boolean;
        errorFlg: boolean;
        successMessage: string;
        errorMessage: string;
        updata: File;

        constructor(
            private Upload: angular.angularFileUpload.IUploadService
        ) {
            this.selectTypes = ["application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "application/vnd.ms-excel"];
            this.successFlg = false;
            this.errorFlg = false;
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
            this.successFlg = false;
            this.errorFlg = false;
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
                this.errorFlg = true;
                this.errorMessage = "エクセルファイルを指定してください。";
                this.updata = null;
                return;
            }
            if (this.updata.size >= (2 * 1048576)) {
                this.errorFlg = true;
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

                document.getElementById("uploadOverlay").style.width = "100%";
                document.getElementById("uploadOverlay").style.opacity = "0.5";
                document.getElementById("uploadOverlay").style.backgroundColor = "black";
                document.getElementById("uploadOverlay").style.zIndex = "1040";

                document.getElementById("uploadingMessage").classList.remove('invisible');
                document.getElementById("uploadingMessage").classList.add('nowloading');
                document.getElementById("uploadingMessage").style.zIndex = "1050";

                this.Upload.upload(cfg).then(
                    (resp: ng.IHttpPromiseCallbackArg<any>) => {
                        this.updata = null;
                        this.successFlg = true;
                        this.successMessage = "アップロード成功しました。";

                        document.getElementById("uploadOverlay").style.width = "0%";
                        document.getElementById("uploadOverlay").style.opacity = "0";
                        document.getElementById("uploadingMessage").classList.remove('nowloading');
                        document.getElementById("uploadingMessage").classList.add('invisible');
                    },
                    (reasone: ng.IHttpPromiseCallbackArg<any>) => {
                        this.updata = null;
                        this.errorFlg = true;
                        this.errorMessage = "処理に失敗しました。" + reasone.data;

                        document.getElementById("uploadOverlay").style.width = "0%";
                        document.getElementById("uploadOverlay").style.opacity = "0";
                        document.getElementById("uploadingMessage").classList.remove('nowloading');
                        document.getElementById("uploadingMessage").classList.add('invisible');
                    }
                );
            }
        }
    }

    app.controller("UploadController", UploadController);
}
