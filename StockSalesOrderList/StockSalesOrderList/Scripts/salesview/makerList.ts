/// <reference path="../typings/tsd.d.ts" />

module salesView {
    "use strict";

    export class MakerController {
        public static $inject = [
            salesViewConfig.ServiceName,
            "$state"
        ];

        session: baseService.Session;
        searchText: any;
        makerList: apiModel.IMakerData[];
        myMakers: apiModel.IMakerData[];

        constructor(
            private resources: salesView.Resources,
            private state: ng.ui.IStateService,
        ) {
            this.session = this.resources.session;
            this.searchText = {
                code: "",
                name: ""
            };
            this.resources.getMakerList(this.makerListSet, this);
            setTimeout(() => { this.sessionWait(this); }, 100);
            $.material.init();
        }

        sessionWait(target: MakerController): void {
            if (target.session.id === null) {
                setTimeout(() => { target.sessionWait(this); }, 100);
                return;
            }
            target.resources.getUserMakerList(target.session.id, target.myMakerListSet, target);
        }

        makerListSet(target: MakerController): void {
            target.makerList = target.resources.makerList;
        }
        myMakerListSet(target: MakerController): void {
            target.myMakers = target.resources.userMakers;
        }

        refineBy(): boolean {
            if (this.searchText.code !== "") {
                return true;
            }
            if (this.searchText.name !== "") {
                return true;
            }
            return false;
        }

        checkMyMaker(id: string): boolean {
            let i: number;
            if (this.session.isMyMaker === false) {
                return true;
            }
            if (this.myMakers === undefined || this.myMakers === null) {
                return false;
            }
            for (i = 0; i < this.myMakers.length; i++) {
                if (this.myMakers[i].id === id) {
                    return true;
                }
            }
            return false;
        }

        selectMaker(maker: apiModel.IMakerData): void {
            this.session.makerId = maker.id;
            this.session.makerCode = maker.code;
            this.session.makerName = maker.name;
            this.scrollTop();
            this.state.go(salesViewConfig.StateGroup);
        }

        scrollTop(): void {
            $("html, body").animate({ scrollTop: 0 }, 500);
            // angular.element("body").animate({ scrollTop: 0 }, "fast");
        }
    }
    angular.module(salesViewConfig.AppName).controller(salesViewConfig.MakerController, MakerController);
}
