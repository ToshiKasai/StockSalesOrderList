/// <reference path="../typings/tsd.d.ts" />

module apiModel {
    "use strict";

    export interface IUserData {
        id?: string;
        userName: string;
        name?: string;
        expiration: Date;
        passwordSkipCnt: number;
        email: string;
        emailConfirmed: boolean;
        lockoutEndData?: Date;
        lockoutEnabled: boolean;
        accessFailedCount: number;
        enabled: boolean;
        deleted: boolean;
        newExpiration?: Date;
        newPassword: string;
    }

    export interface IRoleData {
        id?: string;
        name: string;
        displayName: string;
        deleted: boolean;
    }

    export interface IProductData {
        id?: string;
        code: string;
        name: string;
        quantity: number;
        makerModelId: string;
        makerCode: string;
        makerName: string;
        isSoldWeight: boolean;
        paletteQuantity?: number;
        cartonQuantity?: number;
        caseHeight?: number;
        caseWidth?: number;
        caseDepth?: number;
        caseCapacity?: number;
        leadTime?: number;
        orderInterval?: number;
        oldProductModelId?: number;
        magnification?: number;
        minimumOrderQuantity?: number;
        enabled: boolean;
        deleted: boolean;
    }

    export interface IMakerData {
        id?: string;
        code: string;
        name: string;
        enabled: boolean;
        deleted: boolean;
    }

    export interface IGroupData {
        id?: string;
        code: string;
        name: string;
        makerModelId: string;
        makerCode: string;
        makerName: string;
        containerModelId: string;
        containerName: string;
        isCapacity: boolean;
        containerCapacityBt20Dry: number;
        containerCapacityBt40Dry: number;
        containerCapacityBt20Reefer: number;
        containerCapacityBt40Reefer: number;
        deleted: boolean;
    }

    export interface IContainerData {
        id?: string;
        name: string;
        deleted: boolean;
    }

    export interface ISalesVieDetailData {
        product_id: number;
        detail_date: Date;
        zaiko_actual: number;
        order_plan: number;
        order_actual: number;
        invoice_plan: number;
        invoice_actual: number;
        invoice_zan: number;
        invoice_adjust: number;
        pre_sales_actual: number;
        sales_plan: number;
        sales_actual: number;
        sales_trend: number;
    }

    export interface IOfficeSalesData {
        product_id: number;
        detail_date: Date;
        office_id: number;
        office_name: string;
        sales_plan: number;
        sales_actual: number;
    }

    export interface ISalesViewData {
        product: IProductData;
        salesList: Array<ISalesVieDetailData>;
        officeSales: IOfficeSalesData[][];
        zaikoPlan: number[];
        percentPreSales: number[];
        percentPlan: number[];
        invoiceShow: boolean;
    }

    export interface ICurrentStocks {
        warehouseCode: string;
        warehouseName: string;
        stateName: string;
        expirationDate: Date;
        logicalQuantity: number;
        actualQuantity: number;
    }
    export interface ICurrentOrders {
        orderNo: string;
        orderDate: Date;
        order: number;
    }
    export interface ICurrentInvoices {
        invoiceNo: string;
        warehouseCode: string;
        eta: Date;
        customsClearanceDate: Date;
        purchaseDate: Date;
        quantity: number;
    }
    export interface ICurrentData {
        stocks: ICurrentStocks[];
        orders: ICurrentOrders[];
        invoices: ICurrentInvoices[];
        stockMaxDate: Date;
        orderMaxDate: Date;
        invoiceMaxDate: Date;
    }

    export interface ITrendData {
        id?: string;
        product_id: string;
        detail_date: Date;
        quantity: number;
        comments: string;
        user_id: string;
        user_name: string;
    }

    export interface IElementUserData extends restangular.IElement, IUserData {
    }

    export interface IElementRoleData extends restangular.IElement, IRoleData {
    }

    export interface IElementProductData extends restangular.IElement, IProductData {
    }

    export interface IElementMakerData extends restangular.IElement, IMakerData {
    }

    export interface IElementGroupData extends restangular.IElement, IGroupData {
    }

    export interface IElementContainerData extends restangular.IElement, IContainerData {
    }

    export interface IElementTrendData extends restangular.IElement, ITrendData {
    }

    export interface IElementISalesViewData extends restangular.IElement, ISalesViewData {
    }
}
