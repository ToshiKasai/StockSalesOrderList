/// <reference path="../typings/tsd.d.ts" />
var uploadSalesPlan;
(function (uploadSalesPlan) {
    "use strict";
    var app = angular.module("myApp", ["ngFileUpload"]);
    var UploadController = (function () {
        function UploadController(Upload) {
            this.Upload = Upload;
            this.selectTypes = ["application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "application/vnd.ms-excel"];
            this.successFlg = false;
            this.errorFlg = false;
            this.successMessage = null;
            this.errorMessage = null;
            this.updata = null;
            $.material.init();
        }
        UploadController.prototype.getTypes = function () {
            return this.selectTypes;
        };
        UploadController.prototype.hasUploadData = function () {
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
        };
        UploadController.prototype.change = function ($files, $file, $newFiles, $duplicateFiles, $invalidFiles, $event) {
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
        };
        UploadController.prototype.check_up = function () {
            var _this = this;
            if (this.hasUploadData()) {
                var cfg = void 0;
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
                this.Upload.upload(cfg).then(function (resp) {
                    _this.updata = null;
                    _this.successFlg = true;
                    _this.successMessage = "アップロード成功しました。";
                    document.getElementById("uploadOverlay").style.width = "0%";
                    document.getElementById("uploadOverlay").style.opacity = "0";
                    document.getElementById("uploadingMessage").classList.remove('nowloading');
                    document.getElementById("uploadingMessage").classList.add('invisible');
                }, function (reasone) {
                    _this.updata = null;
                    _this.errorFlg = true;
                    _this.errorMessage = "処理に失敗しました。" + reasone.data;
                    document.getElementById("uploadOverlay").style.width = "0%";
                    document.getElementById("uploadOverlay").style.opacity = "0";
                    document.getElementById("uploadingMessage").classList.remove('nowloading');
                    document.getElementById("uploadingMessage").classList.add('invisible');
                });
            }
        };
        UploadController.$inject = [
            "Upload"
        ];
        return UploadController;
    }());
    app.controller("UploadController", UploadController);
})(uploadSalesPlan || (uploadSalesPlan = {}));
//# sourceMappingURL=uploadSalesPlan.js.map