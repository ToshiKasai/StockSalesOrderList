using Microsoft.AspNet.Identity;
using NPOI.HSSF.UserModel;
using StockSalesOrderList.App_GlobalResources;
using StockSalesOrderList.Models;
using StockSalesOrderList.Models.WebApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace StockSalesOrderList.Services
{
    public class ExportService : CustomExcelService
    {
        #region よく使う書式を定義
        /// <summary>
        /// 上段タイトル向け
        /// </summary>
        protected CellStyle top_title = new CellStyle(border: BORDER.BOX, alignment: HALIGNMENT.CENTER, color: COLOR.SKYBLUE, control: CONTROL.SHRINK, font: FONTSTYLE.PGOTHIC_11_BOLD);
        /// <summary>
        /// yyyy年mm月dd日表示
        /// </summary>
        protected CellStyle date_local = new CellStyle(format: FORMAT.DATE_LOCAL, border: BORDER.BOX, alignment: HALIGNMENT.CENTER);
        /// <summary>
        /// 文字列のセンタリング
        /// </summary>
        protected CellStyle cenetr_string = new CellStyle(format: FORMAT.STRING, border: BORDER.BOX, alignment: HALIGNMENT.CENTER);
        /// <summary>
        /// 上罫線のみ
        /// </summary>
        protected CellStyle top_border = new CellStyle(border: BORDER.TOP_BORDER);
        /// <summary>
        /// センタリングのみ
        /// </summary>
        protected CellStyle center_only = new CellStyle(border: BORDER.BOX, alignment: HALIGNMENT.CENTER);
        /// <summary>
        /// センタリングのみ
        /// </summary>
        protected CellStyle center_bottom = new CellStyle(border: BORDER.BOTTOM, alignment: HALIGNMENT.CENTER);
        /// <summary>
        /// センタリングのみ
        /// </summary>
        protected CellStyle center_mid = new CellStyle(border: BORDER.MID, alignment: HALIGNMENT.CENTER);
        /// <summary>
        /// センタリングのみ
        /// </summary>
        protected CellStyle center_top = new CellStyle(border: BORDER.TOP, alignment: HALIGNMENT.CENTER);
        /// <summary>
        /// 年表示
        /// </summary>
        protected CellStyle year_title = new CellStyle(format: FORMAT.YEAR, border: BORDER.BOX, alignment: HALIGNMENT.CENTER, color: COLOR.LIGHTGREEN, control: CONTROL.SHRINK, font: FONTSTYLE.PGOTHIC_11_BOLD);
        /// <summary>
        /// 月表示
        /// </summary>
        protected CellStyle month_title = new CellStyle(format: FORMAT.MONTH, border: BORDER.BOX, alignment: HALIGNMENT.CENTER, color: COLOR.LIGHTGREEN, control: CONTROL.SHRINK, font: FONTSTYLE.PGOTHIC_11_BOLD);
        /// <summary>
        /// 左タイトル
        /// </summary>
        protected CellStyle left_title = new CellStyle(border: BORDER.BOX, alignment: HALIGNMENT.CENTER, valignment:VALIGNMENT.MIDDLE, control: CONTROL.WRAP, font: FONTSTYLE.PGOTHIC_11_BOLD);
        /// <summary>
        /// 左タイトル
        /// </summary>
        protected CellStyle left_title_top = new CellStyle(border: BORDER.TOP, alignment: HALIGNMENT.CENTER, valignment: VALIGNMENT.MIDDLE, control: CONTROL.WRAP, font: FONTSTYLE.PGOTHIC_11_BOLD);
        /// <summary>
        /// 左タイトル
        /// </summary>
        protected CellStyle left_title_mid = new CellStyle(border: BORDER.MID, alignment: HALIGNMENT.CENTER, valignment: VALIGNMENT.MIDDLE, control: CONTROL.WRAP, font: FONTSTYLE.PGOTHIC_11_BOLD);
        /// <summary>
        /// 左タイトル
        /// </summary>
        protected CellStyle left_title_bottom = new CellStyle(border: BORDER.BOTTOM, alignment: HALIGNMENT.CENTER, valignment: VALIGNMENT.MIDDLE, control: CONTROL.WRAP, font: FONTSTYLE.PGOTHIC_11_BOLD);

        protected CellStyle double_top = new CellStyle(format: FORMAT.DOUBLE, border: BORDER.TOP);
        protected CellStyle double_mid = new CellStyle(format: FORMAT.DOUBLE, border: BORDER.MID);
        protected CellStyle double_bottom = new CellStyle(format: FORMAT.DOUBLE, border: BORDER.BOTTOM);
        protected CellStyle int_top = new CellStyle(format: FORMAT.INT, border: BORDER.TOP);
        protected CellStyle int_mid = new CellStyle(format: FORMAT.INT, border: BORDER.MID);
        protected CellStyle int_bottom = new CellStyle(format: FORMAT.INT, border: BORDER.BOTTOM);
        protected CellStyle percent_mid = new CellStyle(format: FORMAT.PERCENT, border: BORDER.MID);
        protected CellStyle percent_bottom = new CellStyle(format: FORMAT.PERCENT, border: BORDER.BOTTOM);

        protected CellStyle box_string = new CellStyle(format: FORMAT.STRING, border: BORDER.BOX);
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ExportService() : base()
        {
        }

        /// <summary>
        /// 販売在庫データ情報を抽出しEXCELデータ配列を生成
        /// </summary>
        /// <param name="year">年度</param>
        /// <param name="groupid">グループＩＤ</param>
        /// <param name="makerid">メーカーＩＤ</param>
        /// <returns>EXCELデータ配列</returns>
        public byte[] CreateXlsxOneSheetBySalesView(int year, int? groupid, int? makerid)
        {
            SalesViewService service = new SalesViewService();

            try
            {
                // 事務所データ
                ICollection<OfficeModel> offices = dbContext.OfficeModels.Where(ofs => ofs.Deleted == false).Where(ofs => ofs.Code != ContextResources.CompanyCode).ToList();
                int officeCount = offices.Count;

                // メーカー情報とグループ情報
                GroupModel group = null;
                MakerModel maker = null;
                if (groupid.HasValue)
                {
                    group = dbContext.GroupModels.Where(gm => gm.Id == groupid).SingleOrDefault();
                }
                if (group == null && makerid.HasValue)
                {
                    maker = dbContext.MakerModels.Where(mm => mm.Id == makerid).SingleOrDefault();
                }
                else
                {
                    maker = dbContext.MakerModels.Where(mm => mm.Id == group.MakerModelId).SingleOrDefault();
                }

                if (group == null && maker == null)
                {
                    return null;
                }

                // データ抽出
                List<SalesViewApiModel> outputData;
                SalesViewApiParameterModel param = new SalesViewApiParameterModel();
                param.Year = year;
                param.MakerId = makerid;
                param.GroupId = groupid;

                if (service.GetSalesViews(param, out outputData, 18) != System.Net.HttpStatusCode.OK)
                    return null;

                // ブックとシートを用意
                CreateBook();
                GetSheetByName("シート");

                // シートの全体定義を出力
                // 作成日
                WriteCell(0, ONE_ROW.ROW_HEAD, "作成日", top_title);
                WriteCell(1, ONE_ROW.ROW_HEAD, DateTime.Now, date_local);
                MergedCell(1, ONE_ROW.ROW_HEAD, 2, ONE_ROW.ROW_HEAD);

                // メーカーコード
                WriteCell(3, ONE_ROW.ROW_HEAD, "メーカーコード", top_title);
                WriteCell(4, ONE_ROW.ROW_HEAD, maker.Code, cenetr_string);

                // メーカー名
                WriteCell(5, ONE_ROW.ROW_HEAD, "メーカー名", top_title);
                WriteCell(6, ONE_ROW.ROW_HEAD, maker.Name, box_string);
                MergedCell(6, ONE_ROW.ROW_HEAD, 9, ONE_ROW.ROW_HEAD);

                // グループ名
                WriteCell(10, ONE_ROW.ROW_HEAD, "グループ名", top_title);
                if (group == null)
                    WriteCell(11, ONE_ROW.ROW_HEAD, string.Empty, box_string);
                else
                    WriteCell(11, ONE_ROW.ROW_HEAD, group.Name, box_string);
                MergedCell(11, ONE_ROW.ROW_HEAD, 15, ONE_ROW.ROW_HEAD);

                // サインイン
                int userId = GetUserId();
                string userName = dbContext.UserModels.Where(um => um.Id == userId).Select(um => um.Name).SingleOrDefault();
                WriteCell(16, ONE_ROW.ROW_HEAD, "ユーザー名", top_title);
                WriteCell(17, ONE_ROW.ROW_HEAD, userName, box_string);
                MergedCell(17, ONE_ROW.ROW_HEAD, 20, ONE_ROW.ROW_HEAD);

                if (group == null)
                    WriteAppLog(this.ToString(), "データ作成条件：年度[" + year.ToString() + "] メーカー[" + maker.Code + "]");
                else
                    WriteAppLog(this.ToString(), "データ作成条件：年度[" + year.ToString() + "] メーカー[" + maker.Code + "] グループ[" + group.Code + "]");

                DateTime startDate;
                DateTime check;
                int productIndex = 0;
                int baseRowIndex = 0;
                foreach(var productData in outputData)
                {
                    // 最小年月＋１が表示開始年月となる
                    startDate = productData.SalesList.Select(sl => sl.detail_date).Min().AddMonths(1);

                    // 商品データの基準行
                    baseRowIndex = ONE_ROW.ROW_START + productIndex * ONE_ROW.ROWS_ONE_PRODUCT;

                    // 頭に空行を挟む(上線を引く必要がありそう)
                    for (int i = 0; i <= ONE_ROW.COL_SUMMARY; i++)
                        WriteStyle(i, baseRowIndex, top_border);
                    baseRowIndex += 1;

                    // 商品コード
                    WriteCell(0, baseRowIndex, "商品コード", top_title);
                    WriteCell(1, baseRowIndex, productData.Product.Code, cenetr_string);
                    MergedCell(1, baseRowIndex, 2, baseRowIndex);

                    // 商品名
                    WriteCell(3, baseRowIndex, "商品名", top_title);
                    WriteCell(4, baseRowIndex, productData.Product.Name, box_string);
                    MergedCell(4, baseRowIndex, 12, baseRowIndex);

                    // 既定入数
                    WriteCell(13, baseRowIndex, "既定入数", top_title);
                    WriteCell(14, baseRowIndex, productData.Product.Quantity, center_only);

                    // カートン入数
                    WriteCell(15, baseRowIndex, "カートン入数", top_title);
                    MergedCell(15, baseRowIndex, 16, baseRowIndex);
                    if (productData.Product.CartonQuantity.HasValue)
                        WriteCell(17, baseRowIndex, (decimal)productData.Product.CartonQuantity, center_only);
                    else
                        WriteCell(17, baseRowIndex, "未登録", center_only);

                    // パレット入数
                    WriteCell(18, baseRowIndex, "パレット入数", top_title);
                    MergedCell(18, baseRowIndex, 19, baseRowIndex);
                    if (productData.Product.PaletteQuantity.HasValue)
                        WriteCell(20, baseRowIndex, (decimal)productData.Product.PaletteQuantity, center_only);
                    else
                        WriteCell(20, baseRowIndex, "未登録", center_only);

                    // 年月の行
                    baseRowIndex += 1;

                    // 年度＋月の表示
                    check = startDate;
                    WriteCell(0, baseRowIndex, check, year_title);
                    MergedCell(0, baseRowIndex, 1, baseRowIndex);

                    for (int i = 0; i < ONE_ROW.COLS_REPEAT_MONTH; i++)
                    {
                        WriteCell(ONE_ROW.COL_START_MONTH + i, baseRowIndex, check, month_title);
                        check = check.AddMonths(1);
                    }
                    WriteCell(ONE_ROW.COL_SUMMARY, baseRowIndex, "累計", month_title);

                    // 在庫販売データの左側見出し

                    // 在庫関連
                    WriteCell(0, baseRowIndex + ONE_ROW.ROW_STOCK_PLAN, "月初在庫", left_title);
                    MergedCell(0, baseRowIndex + ONE_ROW.ROW_STOCK_PLAN, 0, baseRowIndex + ONE_ROW.ROW_STOCK_ACTUAL);
                    WriteCell(1, baseRowIndex + ONE_ROW.ROW_STOCK_PLAN, "予測", left_title_top);
                    WriteCell(1, baseRowIndex + ONE_ROW.ROW_STOCK_ACTUAL, "実績", left_title_bottom);

                    // 発注関連
                    WriteCell(0, baseRowIndex + ONE_ROW.ROW_ORDER_PLAN, "発注", left_title);
                    MergedCell(0, baseRowIndex + ONE_ROW.ROW_ORDER_PLAN, 0, baseRowIndex + ONE_ROW.ROW_ORDER_ACTUAL);
                    WriteCell(1, baseRowIndex + ONE_ROW.ROW_ORDER_PLAN, "予定", left_title_top);
                    WriteCell(1, baseRowIndex + ONE_ROW.ROW_ORDER_ACTUAL, "実績", left_title_bottom);

                    // 入荷関連
                    WriteCell(0, baseRowIndex + ONE_ROW.ROW_INVOICE_PLAN, "入荷", left_title);
                    MergedCell(0, baseRowIndex + ONE_ROW.ROW_INVOICE_PLAN, 0, baseRowIndex + ONE_ROW.ROW_INVOICE_ADJUSTMENT);
                    WriteCell(1, baseRowIndex + ONE_ROW.ROW_INVOICE_PLAN, "予定", left_title_top);
                    WriteCell(1, baseRowIndex + ONE_ROW.ROW_INVOICE_ACTUAL, "実績", left_title_mid);
                    WriteCell(1, baseRowIndex + ONE_ROW.ROW_INVOICE_REAMING, "残数", left_title_mid);
                    WriteCell(1, baseRowIndex + ONE_ROW.ROW_INVOICE_ADJUSTMENT, "調整", left_title_bottom);

                    // 販売関連
                    WriteCell(0, baseRowIndex + ONE_ROW.ROW_SALES_PRE, "販売", left_title);
                    MergedCell(0, baseRowIndex + ONE_ROW.ROW_SALES_PRE, 0, baseRowIndex + ONE_ROW.ROW_SALES_PLAN_PERCENT);
                    WriteCell(1, baseRowIndex + ONE_ROW.ROW_SALES_PRE, "前年", left_title_top);
                    WriteCell(1, baseRowIndex + ONE_ROW.ROW_SALES_PLAN, "予算", left_title_mid);
                    WriteCell(1, baseRowIndex + ONE_ROW.ROW_SALES_TREND, "動向", left_title_mid);
                    WriteCell(1, baseRowIndex + ONE_ROW.ROW_SALES_ACTUAL, "実績", left_title_mid);
                    WriteCell(1, baseRowIndex + ONE_ROW.ROW_SALES_PRE_PERCENT, "前年比", left_title_mid);
                    WriteCell(1, baseRowIndex + ONE_ROW.ROW_SALES_PLAN_PERCENT, "予実比", left_title_bottom);

                    // データ貼り付け
                    decimal stockPlan = 0;
                    check = startDate;

                    for (int i = 0; i < ONE_ROW.COLS_REPEAT_MONTH; i++)
                    {
                        SalesViewsTempModel work = productData.SalesList.Where(x => x.detail_date == check).SingleOrDefault();
                        if (work == null)
                        {
                            for (int j = ONE_ROW.ROW_STOCK_PLAN; j <= ONE_ROW.ROW_SALES_PLAN_PERCENT; j++)
                            {
                                switch (j)
                                {
                                    case ONE_ROW.ROW_STOCK_PLAN:
                                        WriteCell(ONE_ROW.COL_START_MONTH + i, baseRowIndex + j, stockPlan,
                                            productData.Product.IsSoldWeight ? double_top : int_top);
                                        break;
                                    case ONE_ROW.ROW_SALES_PRE_PERCENT:
                                        WriteCell(ONE_ROW.COL_START_MONTH + i, baseRowIndex + j, 0, percent_mid);
                                        break;
                                    case ONE_ROW.ROW_SALES_PLAN_PERCENT:
                                        WriteCell(ONE_ROW.COL_START_MONTH + i, baseRowIndex + j, 0, percent_bottom);
                                        break;
                                    case ONE_ROW.ROW_ORDER_PLAN:
                                    case ONE_ROW.ROW_INVOICE_PLAN:
                                    case ONE_ROW.ROW_SALES_PRE:
                                        WriteCell(ONE_ROW.COL_START_MONTH + i, baseRowIndex + j, 0,
                                            productData.Product.IsSoldWeight ? double_top : int_top);
                                        break;
                                    case ONE_ROW.ROW_STOCK_ACTUAL:
                                    case ONE_ROW.ROW_ORDER_ACTUAL:
                                    case ONE_ROW.ROW_INVOICE_ADJUSTMENT:
                                        WriteCell(ONE_ROW.COL_START_MONTH + i, baseRowIndex + j, 0,
                                            productData.Product.IsSoldWeight ? double_bottom : int_bottom);
                                        break;
                                    default:
                                        WriteCell(ONE_ROW.COL_START_MONTH + i, baseRowIndex + j, 0,
                                            productData.Product.IsSoldWeight ? double_mid : int_mid);
                                        break;
                                }
                            }
                        }
                        else
                        {
                            // 在庫関連
                            WriteCell(ONE_ROW.COL_START_MONTH + i, baseRowIndex + ONE_ROW.ROW_STOCK_PLAN,
                                stockPlan, productData.Product.IsSoldWeight ? double_top : int_top);

                            WriteCell(ONE_ROW.COL_START_MONTH + i, baseRowIndex + ONE_ROW.ROW_STOCK_ACTUAL,
                                work.zaiko_actual, productData.Product.IsSoldWeight ? double_mid : int_mid);

                            WriteCell(ONE_ROW.COL_START_MONTH + i, baseRowIndex + ONE_ROW.ROW_ORDER_PLAN,
                                work.order_plan, productData.Product.IsSoldWeight ? double_top : int_top);
                            WriteCell(ONE_ROW.COL_START_MONTH + i, baseRowIndex + ONE_ROW.ROW_ORDER_ACTUAL,
                                work.order_actual, productData.Product.IsSoldWeight ? double_bottom : int_bottom);

                            // 入荷関連
                            WriteCell(ONE_ROW.COL_START_MONTH + i, baseRowIndex + ONE_ROW.ROW_INVOICE_PLAN,
                                work.invoice_plan, productData.Product.IsSoldWeight ? double_top : int_top);
                            WriteCell(ONE_ROW.COL_START_MONTH + i, baseRowIndex + ONE_ROW.ROW_INVOICE_ACTUAL,
                                work.invoice_actual, productData.Product.IsSoldWeight ? double_mid : int_mid);
                            WriteCell(ONE_ROW.COL_START_MONTH + i, baseRowIndex + ONE_ROW.ROW_INVOICE_REAMING,
                                work.invoice_zan, productData.Product.IsSoldWeight ? double_mid : int_mid);
                            WriteCell(ONE_ROW.COL_START_MONTH + i, baseRowIndex + ONE_ROW.ROW_INVOICE_ADJUSTMENT,
                                work.invoice_adjust, productData.Product.IsSoldWeight ? double_bottom : int_bottom);

                            // 販売関連
                            WriteCell(ONE_ROW.COL_START_MONTH + i, baseRowIndex + ONE_ROW.ROW_SALES_PRE,
                                work.pre_sales_actual, productData.Product.IsSoldWeight ? double_top : int_top);
                            WriteCell(ONE_ROW.COL_START_MONTH + i, baseRowIndex + ONE_ROW.ROW_SALES_PLAN,
                                work.sales_plan, productData.Product.IsSoldWeight ? double_mid : int_mid);
                            WriteCell(ONE_ROW.COL_START_MONTH + i, baseRowIndex + ONE_ROW.ROW_SALES_TREND,
                                work.sales_trend, productData.Product.IsSoldWeight ? double_mid : int_mid);
                            WriteCell(ONE_ROW.COL_START_MONTH + i, baseRowIndex + ONE_ROW.ROW_SALES_ACTUAL,
                                work.sales_actual, productData.Product.IsSoldWeight ? double_mid : int_mid);

                            // 比率計算
                            WriteCell(ONE_ROW.COL_START_MONTH + i, baseRowIndex + ONE_ROW.ROW_SALES_PRE_PERCENT,
                                work.pre_sales_actual == 0 ? 0 : work.sales_actual / work.pre_sales_actual, percent_mid);
                            WriteCell(ONE_ROW.COL_START_MONTH + i, baseRowIndex + ONE_ROW.ROW_SALES_PLAN_PERCENT,
                                work.sales_plan == 0 ? 0 : work.sales_actual / work.sales_plan, percent_bottom);

                            // 次月の在庫予測の算出
                            if (check <= DateTime.Now.Date)
                                stockPlan = work.zaiko_actual;
                            stockPlan -= (work.sales_plan + work.sales_trend);
                            stockPlan += (work.invoice_plan);
                            stockPlan += productData.SalesList.Where(x => x.detail_date == check.AddMonths(-1)).Select(x => x.invoice_zan).SingleOrDefault();
                            stockPlan -= productData.SalesList.Where(x => x.detail_date == check.AddMonths(-1)).Select(x => x.invoice_adjust).SingleOrDefault();
                        }
                        check = check.AddMonths(1);
                    }

                    // 在庫関連累計
                    WriteCell(ONE_ROW.COL_SUMMARY, baseRowIndex + ONE_ROW.ROW_STOCK_PLAN, "-", center_top);
                    WriteFormula(ONE_ROW.COL_SUMMARY, baseRowIndex + ONE_ROW.ROW_STOCK_ACTUAL,
                        MakeSumFormula(ONE_ROW.COL_START_MONTH, ONE_ROW.COL_SUMMARY - 1, baseRowIndex + ONE_ROW.ROW_STOCK_ACTUAL),
                        productData.Product.IsSoldWeight ? double_bottom : int_bottom);

                    // 発注関連
                    WriteFormula(ONE_ROW.COL_SUMMARY, baseRowIndex + ONE_ROW.ROW_ORDER_PLAN,
                        MakeSumFormula(ONE_ROW.COL_START_MONTH, ONE_ROW.COL_SUMMARY - 1, baseRowIndex + ONE_ROW.ROW_ORDER_PLAN),
                        productData.Product.IsSoldWeight ? double_top : int_top);
                    WriteFormula(ONE_ROW.COL_SUMMARY, baseRowIndex + ONE_ROW.ROW_ORDER_ACTUAL,
                        MakeSumFormula(ONE_ROW.COL_START_MONTH, ONE_ROW.COL_SUMMARY - 1, baseRowIndex + ONE_ROW.ROW_ORDER_ACTUAL),
                        productData.Product.IsSoldWeight ? double_bottom : int_bottom);

                    // 入荷関連
                    WriteFormula(ONE_ROW.COL_SUMMARY, baseRowIndex + ONE_ROW.ROW_INVOICE_PLAN,
                        MakeSumFormula(ONE_ROW.COL_START_MONTH, ONE_ROW.COL_SUMMARY - 1, baseRowIndex + ONE_ROW.ROW_INVOICE_PLAN),
                        productData.Product.IsSoldWeight ? double_top : int_top);
                    WriteFormula(ONE_ROW.COL_SUMMARY, baseRowIndex + ONE_ROW.ROW_INVOICE_ACTUAL,
                        MakeSumFormula(ONE_ROW.COL_START_MONTH, ONE_ROW.COL_SUMMARY - 1, baseRowIndex + ONE_ROW.ROW_INVOICE_ACTUAL),
                        productData.Product.IsSoldWeight ? double_mid : int_mid);
                    WriteCell(ONE_ROW.COL_SUMMARY, baseRowIndex + ONE_ROW.ROW_INVOICE_REAMING, "-", center_mid);
                    WriteCell(ONE_ROW.COL_SUMMARY, baseRowIndex + ONE_ROW.ROW_INVOICE_ADJUSTMENT, "-", center_bottom);

                    // 販売関連
                    WriteFormula(ONE_ROW.COL_SUMMARY, baseRowIndex + ONE_ROW.ROW_SALES_PRE,
                        MakeSumFormula(ONE_ROW.COL_START_MONTH, ONE_ROW.COL_SUMMARY - 1, baseRowIndex + ONE_ROW.ROW_SALES_PRE),
                        productData.Product.IsSoldWeight ? double_top : int_top);
                    WriteFormula(ONE_ROW.COL_SUMMARY, baseRowIndex + ONE_ROW.ROW_SALES_PLAN,
                        MakeSumFormula(ONE_ROW.COL_START_MONTH, ONE_ROW.COL_SUMMARY - 1, baseRowIndex + ONE_ROW.ROW_SALES_PLAN),
                        productData.Product.IsSoldWeight ? double_mid : int_mid);
                    WriteFormula(ONE_ROW.COL_SUMMARY, baseRowIndex + ONE_ROW.ROW_SALES_TREND,
                        MakeSumFormula(ONE_ROW.COL_START_MONTH, ONE_ROW.COL_SUMMARY - 1, baseRowIndex + ONE_ROW.ROW_SALES_TREND),
                        productData.Product.IsSoldWeight ? double_mid : int_mid);
                    WriteFormula(ONE_ROW.COL_SUMMARY, baseRowIndex + ONE_ROW.ROW_SALES_ACTUAL,
                        MakeSumFormula(ONE_ROW.COL_START_MONTH, ONE_ROW.COL_SUMMARY - 1, baseRowIndex + ONE_ROW.ROW_SALES_ACTUAL),
                        productData.Product.IsSoldWeight ? double_mid : int_mid);
                    WriteCell(ONE_ROW.COL_SUMMARY, baseRowIndex + ONE_ROW.ROW_SALES_PRE_PERCENT, "-", center_mid);
                    WriteCell(ONE_ROW.COL_SUMMARY, baseRowIndex + ONE_ROW.ROW_SALES_PLAN_PERCENT, "-", center_bottom);

                    // カウンタアップ
                    productIndex++;
                }

                // 仕上げ
                baseRowIndex = ONE_ROW.ROW_START + productIndex * ONE_ROW.ROWS_ONE_PRODUCT;
                for (int i = 0; i <= ONE_ROW.COL_SUMMARY; i++)
                    WriteStyle(i, baseRowIndex, top_border);

                // ヘッダフッタほか
                TargetSheet.Header.Right = HeaderFooter.Date;
                TargetSheet.Footer.Center = HeaderFooter.Page;
                TargetSheet.Autobreaks = true;
                TargetSheet.FitToPage = true;

                // IPrintSetup print = activeSheet.PrintSetup;
                // activeSheet.PrintSetup.Scale = 90;
                // activeSheet.PrintSetup.FitHeight = 1;
                TargetSheet.PrintSetup.FitWidth = 1;
                /*  8:A3 / 9:A4 / 11:A5 / 12:B4 / 13;B5 */
                TargetSheet.PrintSetup.PaperSize = 9;       // A4用紙
                TargetSheet.PrintSetup.Landscape = false;    // 横向き

                WriteAppLog(this.ToString(), "データ作成成功");

                using (MemoryStream ms = new MemoryStream())
                {
                    WorkBook.Write(ms);
                    CloseBook();
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                WriteAppLog(this.ToString(), ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 販売在庫データ情報を抽出しタブ型のEXCELデータ配列を生成
        /// </summary>
        /// <param name="year">年度</param>
        /// <param name="groupid">グループＩＤ</param>
        /// <param name="makerid">メーカーＩＤ</param>
        /// <returns>EXCELデータ配列</returns>
        public byte[] CreateXlsxBySalesViewTab(int year, int? groupid, int? makerid)
        {
            SalesViewService service = new SalesViewService();

            try
            {
                // 事務所データ
                ICollection<OfficeModel> offices = dbContext.OfficeModels.Where(ofs => ofs.Deleted == false).Where(ofs => ofs.Code != ContextResources.CompanyCode).ToList();
                int officeCount = offices.Count;

                // データ抽出
                List<SalesViewApiModel> outputData;
                SalesViewApiParameterModel param = new SalesViewApiParameterModel();
                param.Year = year;
                param.MakerId = makerid;
                param.GroupId = groupid;

                if (service.GetSalesViews(param, out outputData, 18) != System.Net.HttpStatusCode.OK)
                    return null;

                // とりあえずブック
                CreateBook();

                DateTime startDate;
                DateTime check;
                foreach (var productData in outputData)
                {
                    // 最小年月＋１が表示開始年月となる
                    startDate = productData.SalesList.Select(sl => sl.detail_date).Min().AddMonths(1);
                    check = startDate;

                    // 商品コードをシート名にする
                    GetSheetByName(productData.Product.Code);

                    // 全体書式を設定する
                    // 一番上
                    WriteCell(0, MULTI_CONFIG.ROW_HEAD, "作成日", top_title);
                    WriteCell(1, MULTI_CONFIG.ROW_HEAD, DateTime.Now.Date, date_local);
                    MergedCell(1, MULTI_CONFIG.ROW_HEAD, 2, MULTI_CONFIG.ROW_HEAD);

                    WriteCell(3, MULTI_CONFIG.ROW_HEAD, "商品コード", top_title);
                    WriteCell(4, MULTI_CONFIG.ROW_HEAD, productData.Product.Code, cenetr_string);
                    MergedCell(4, MULTI_CONFIG.ROW_HEAD, 5, MULTI_CONFIG.ROW_HEAD);

                    WriteCell(6, MULTI_CONFIG.ROW_HEAD, "商品名", top_title);
                    WriteCell(7, MULTI_CONFIG.ROW_HEAD, productData.Product.Name, cenetr_string);
                    MergedCell(7, MULTI_CONFIG.ROW_HEAD, 12, MULTI_CONFIG.ROW_HEAD);
                    WriteCell(13, 0, "入数", top_title);
                    WriteCell(14, 0, productData.Product.Quantity, center_only);

                    // 上段年月表示行
                    WriteCell(0, MULTI_CONFIG.ROW_UP_MONTH, check, year_title);
                    MergedCell(0, MULTI_CONFIG.ROW_UP_MONTH, 1, MULTI_CONFIG.ROW_UP_MONTH);
                    WriteCell(0, MULTI_CONFIG.ROW_DOWN_MONTH, check, year_title);
                    MergedCell(0, MULTI_CONFIG.ROW_DOWN_MONTH, 1, MULTI_CONFIG.ROW_DOWN_MONTH);
                    for (int i = 0; i < MULTI_CONFIG.COLS_REPEAT_MONTH; i++)
                    {
                        WriteCell(MULTI_CONFIG.COL_START_MONTH + i, MULTI_CONFIG.ROW_UP_MONTH, check, month_title);
                        WriteCell(MULTI_CONFIG.COL_START_MONTH + i, MULTI_CONFIG.ROW_DOWN_MONTH, check, month_title);
                        check = check.AddMonths(1);
                    }
                    WriteCell(MULTI_CONFIG.COL_SUMMARY, MULTI_CONFIG.ROW_UP_MONTH, "累計", month_title);
                    WriteCell(MULTI_CONFIG.COL_SUMMARY, MULTI_CONFIG.ROW_DOWN_MONTH, "累計", month_title);

                    // 月初在庫タイトル
                    WriteCell(0, MULTI_CONFIG.ROW_STOCK_PLAN, "月初在庫", left_title);
                    MergedCell(0, MULTI_CONFIG.ROW_STOCK_PLAN, 0, MULTI_CONFIG.ROW_STOCK_ACTUAL);
                    WriteCell(1, MULTI_CONFIG.ROW_STOCK_PLAN, "予測", left_title_top);
                    WriteCell(1, MULTI_CONFIG.ROW_STOCK_ACTUAL, "実績", left_title_bottom);

                    // 発注タイトル
                    WriteCell(0, MULTI_CONFIG.ROW_ORDER_PLAN, "発注", left_title);
                    MergedCell(0, MULTI_CONFIG.ROW_ORDER_PLAN, 0, MULTI_CONFIG.ROW_ORDER_ACTUAL);
                    WriteCell(1, MULTI_CONFIG.ROW_ORDER_PLAN, "予定", left_title_top);
                    WriteCell(1, MULTI_CONFIG.ROW_ORDER_ACTUAL, "実績", left_title_bottom);

                    // 入荷タイトル
                    WriteCell(0, MULTI_CONFIG.ROW_INVOICE_PLAN, "入荷", left_title);
                    MergedCell(0, MULTI_CONFIG.ROW_INVOICE_PLAN, 0, MULTI_CONFIG.ROW_INVOICE_ADJUSTMENT);
                    WriteCell(1, MULTI_CONFIG.ROW_INVOICE_PLAN, "予定", left_title_top);
                    WriteCell(1, MULTI_CONFIG.ROW_INVOICE_ACTUAL, "実績", left_title_mid);
                    WriteCell(1, MULTI_CONFIG.ROW_INVOICE_REAMING, "残数", left_title_mid);
                    WriteCell(1, MULTI_CONFIG.ROW_INVOICE_ADJUSTMENT, "調整", left_title_bottom);

                    // 販売タイトル
                    WriteCell(0, MULTI_CONFIG.ROW_SALES_PRE, "販売", left_title);
                    MergedCell(0, MULTI_CONFIG.ROW_SALES_PRE, 0, MULTI_CONFIG.ROW_SALES_PLAN_PERCENT);
                    WriteCell(1, MULTI_CONFIG.ROW_SALES_PRE, "前年", left_title_top);
                    WriteCell(1, MULTI_CONFIG.ROW_SALES_PLAN, "予定", left_title_mid);
                    WriteCell(1, MULTI_CONFIG.ROW_SALES_TREND, "動向", left_title_mid);
                    WriteCell(1, MULTI_CONFIG.ROW_SALES_ACTUAL, "実績", left_title_mid);
                    WriteCell(1, MULTI_CONFIG.ROW_SALES_PRE_PERCENT, "前年比", left_title_mid);
                    WriteCell(1, MULTI_CONFIG.ROW_SALES_PLAN_PERCENT, "予実比", left_title_bottom);

                    // 支店が存在しない場合は出力しない
                    if (officeCount > 0)
                    {
                        int i = 0;
                        foreach (var office in offices)
                        {
                            int baseRow = MULTI_CONFIG.ROW_OFFICE_START + MULTI_CONFIG.ROWS_ONE_OFFICE * i;
                            WriteCell(0, baseRow, office.Name, left_title);
                            MergedCell(0, baseRow + MULTI_CONFIG.ROW_OFFICE_PRE, 0, baseRow + MULTI_CONFIG.ROW_OFFICE_ACTUAL);
                            WriteCell(1, baseRow + MULTI_CONFIG.ROW_OFFICE_PRE, "前年", left_title_top);
                            WriteCell(1, baseRow + MULTI_CONFIG.ROW_OFFICE_ACTUAL, "実績", left_title_bottom);
                            i++;
                        }
                    }

                    decimal stockPlan = 0;
                    check = startDate;
                    for (int i = 0; i < MULTI_CONFIG.COLS_REPEAT_MONTH; i++)
                    {
                        SalesViewsTempModel work = productData.SalesList.Where(x => x.detail_date == check).SingleOrDefault();
                        if (work == null)
                        {
                            for (int j = MULTI_CONFIG.ROW_STOCK_PLAN; j <= MULTI_CONFIG.ROW_SALES_PLAN_PERCENT; j++)
                            {
                                switch (j)
                                {
                                    case MULTI_CONFIG.ROW_STOCK_PLAN:
                                        WriteCell(MULTI_CONFIG.COL_START_MONTH + i, j, stockPlan,
                                            productData.Product.IsSoldWeight ? double_top : int_top);
                                        break;
                                    case MULTI_CONFIG.ROW_SALES_PLAN_PERCENT:
                                        WriteCell(MULTI_CONFIG.COL_START_MONTH + i, j, 0, percent_bottom);
                                        break;
                                    case MULTI_CONFIG.ROW_SALES_PRE_PERCENT:
                                        WriteCell(MULTI_CONFIG.COL_START_MONTH + i, j, 0, percent_mid);
                                        break;
                                    case MULTI_CONFIG.ROW_ORDER_PLAN:
                                    case MULTI_CONFIG.ROW_INVOICE_PLAN:
                                    case MULTI_CONFIG.ROW_SALES_PRE:
                                        WriteCell(MULTI_CONFIG.COL_START_MONTH + i, j, 0,
                                            productData.Product.IsSoldWeight ? double_top : int_top);
                                        break;
                                    case MULTI_CONFIG.ROW_STOCK_ACTUAL:
                                    case MULTI_CONFIG.ROW_ORDER_ACTUAL:
                                    case MULTI_CONFIG.ROW_INVOICE_ADJUSTMENT:
                                        WriteCell(MULTI_CONFIG.COL_START_MONTH + i, j, 0,
                                            productData.Product.IsSoldWeight ? double_bottom : int_bottom);
                                        break;
                                    default:
                                        WriteCell(MULTI_CONFIG.COL_START_MONTH + i, j, 0,
                                            productData.Product.IsSoldWeight ? double_mid : int_mid);
                                        break;
                                }

                            }
                        }
                        else
                        {
                            // 在庫情報
                            WriteCell(MULTI_CONFIG.COL_START_MONTH + i, MULTI_CONFIG.ROW_STOCK_PLAN, stockPlan, productData.Product.IsSoldWeight ? double_top : int_top);
                            WriteCell(MULTI_CONFIG.COL_START_MONTH + i, MULTI_CONFIG.ROW_STOCK_ACTUAL, work.zaiko_actual, productData.Product.IsSoldWeight ? double_bottom : int_bottom);

                            // 発注情報
                            WriteCell(MULTI_CONFIG.COL_START_MONTH + i, MULTI_CONFIG.ROW_ORDER_PLAN, work.order_plan, productData.Product.IsSoldWeight ? double_top : int_top);
                            WriteCell(MULTI_CONFIG.COL_START_MONTH + i, MULTI_CONFIG.ROW_ORDER_ACTUAL, work.order_actual, productData.Product.IsSoldWeight ? double_bottom : int_bottom);

                            // 入荷情報
                            WriteCell(MULTI_CONFIG.COL_START_MONTH + i, MULTI_CONFIG.ROW_INVOICE_PLAN, work.invoice_plan, productData.Product.IsSoldWeight ? double_top : int_top);
                            WriteCell(MULTI_CONFIG.COL_START_MONTH + i, MULTI_CONFIG.ROW_INVOICE_ACTUAL, work.invoice_actual, productData.Product.IsSoldWeight ? double_mid : int_mid);
                            WriteCell(MULTI_CONFIG.COL_START_MONTH + i, MULTI_CONFIG.ROW_INVOICE_REAMING, work.invoice_zan, productData.Product.IsSoldWeight ? double_mid : int_mid);
                            WriteCell(MULTI_CONFIG.COL_START_MONTH + i, MULTI_CONFIG.ROW_INVOICE_ADJUSTMENT, work.invoice_adjust, productData.Product.IsSoldWeight ? double_bottom : int_bottom);

                            // 販売情報
                            WriteCell(MULTI_CONFIG.COL_START_MONTH + i, MULTI_CONFIG.ROW_SALES_PRE, work.pre_sales_actual, productData.Product.IsSoldWeight ? double_top : int_top);
                            WriteCell(MULTI_CONFIG.COL_START_MONTH + i, MULTI_CONFIG.ROW_SALES_PLAN, work.sales_plan, productData.Product.IsSoldWeight ? double_mid : int_mid);
                            WriteCell(MULTI_CONFIG.COL_START_MONTH + i, MULTI_CONFIG.ROW_SALES_TREND, work.sales_trend, productData.Product.IsSoldWeight ? double_mid : int_mid);
                            WriteCell(MULTI_CONFIG.COL_START_MONTH + i, MULTI_CONFIG.ROW_SALES_ACTUAL, work.sales_actual, productData.Product.IsSoldWeight ? double_mid : int_mid);

                            // 前年比
                            WriteCell(MULTI_CONFIG.COL_START_MONTH + i, MULTI_CONFIG.ROW_SALES_PRE_PERCENT, work.pre_sales_actual == 0 ? 0 : work.sales_actual / work.pre_sales_actual, percent_mid);

                            // 予実比
                            WriteCell(MULTI_CONFIG.COL_START_MONTH + i, MULTI_CONFIG.ROW_SALES_PLAN_PERCENT, work.sales_plan == 0 ? 0 : work.sales_actual / work.sales_plan, percent_bottom);

                            // 次月の在庫予測の算出
                            if (check <= DateTime.Now.Date)
                                stockPlan = work.zaiko_actual;
                            stockPlan -= (work.sales_plan + work.sales_trend);
                            stockPlan += (work.invoice_plan);
                            stockPlan += productData.SalesList.Where(x => x.detail_date == check.AddMonths(-1)).Select(x => x.invoice_zan).SingleOrDefault();
                            stockPlan -= productData.SalesList.Where(x => x.detail_date == check.AddMonths(-1)).Select(x => x.invoice_adjust).SingleOrDefault();
                        }

                        // 支店別は件数分
                        if (officeCount > 0)
                        {
                            int j = 0;
                            SalesOfficeTempModel officeWork;
                            foreach (var office in offices)
                            {
                                int baseRow = MULTI_CONFIG.ROW_OFFICE_START + MULTI_CONFIG.ROWS_ONE_OFFICE * j;

                                officeWork = productData.OfficeSales.SelectMany(os => os.Where(ot => ot.office_id == office.Id))
                                    .Where(ot => ot.detail_date == check).SingleOrDefault();

                                // 前年
                                WriteCell(MULTI_CONFIG.COL_START_MONTH + i, baseRow + MULTI_CONFIG.ROW_OFFICE_PRE,
                                    officeWork == null ? 0 : officeWork.pre_sales_actual, productData.Product.IsSoldWeight ? double_top : int_top);
                                // 当年
                                WriteCell(MULTI_CONFIG.COL_START_MONTH + i, baseRow + MULTI_CONFIG.ROW_OFFICE_ACTUAL,
                                    officeWork == null ? 0 : officeWork.sales_actual, productData.Product.IsSoldWeight ? double_bottom : int_bottom);
                                j++;
                            }
                        }
                        check = check.AddMonths(1);
                    }

                    // 累計列
                    // 在庫関連
                    WriteCell(MULTI_CONFIG.COL_SUMMARY, MULTI_CONFIG.ROW_STOCK_PLAN, "-", center_top);
                    WriteFormula(MULTI_CONFIG.COL_SUMMARY, MULTI_CONFIG.ROW_STOCK_ACTUAL,
                        MakeSumFormula(MULTI_CONFIG.COL_START_MONTH, MULTI_CONFIG.COL_SUMMARY - 1, MULTI_CONFIG.ROW_STOCK_ACTUAL),
                        productData.Product.IsSoldWeight ? double_bottom : int_bottom);

                    // 発注関連
                    WriteFormula(MULTI_CONFIG.COL_SUMMARY, MULTI_CONFIG.ROW_ORDER_PLAN,
                        MakeSumFormula(MULTI_CONFIG.COL_START_MONTH, MULTI_CONFIG.COL_SUMMARY - 1, MULTI_CONFIG.ROW_ORDER_PLAN),
                        productData.Product.IsSoldWeight ? double_top : int_top);
                    WriteFormula(MULTI_CONFIG.COL_SUMMARY, MULTI_CONFIG.ROW_ORDER_ACTUAL,
                        MakeSumFormula(MULTI_CONFIG.COL_START_MONTH, MULTI_CONFIG.COL_SUMMARY - 1, MULTI_CONFIG.ROW_ORDER_ACTUAL),
                        productData.Product.IsSoldWeight ? double_bottom : int_bottom);

                    // 入荷関連
                    WriteCell(MULTI_CONFIG.COL_SUMMARY, MULTI_CONFIG.ROW_INVOICE_PLAN, "-", center_top);
                    WriteFormula(MULTI_CONFIG.COL_SUMMARY, MULTI_CONFIG.ROW_INVOICE_PLAN,
                        MakeSumFormula(MULTI_CONFIG.COL_START_MONTH, MULTI_CONFIG.COL_SUMMARY - 1, MULTI_CONFIG.ROW_INVOICE_PLAN),
                        productData.Product.IsSoldWeight ? double_mid : int_mid);
                    WriteFormula(MULTI_CONFIG.COL_SUMMARY, MULTI_CONFIG.ROW_INVOICE_ACTUAL,
                        MakeSumFormula(MULTI_CONFIG.COL_START_MONTH, MULTI_CONFIG.COL_SUMMARY - 1, MULTI_CONFIG.ROW_INVOICE_ACTUAL),
                        productData.Product.IsSoldWeight ? double_mid : int_mid);
                    WriteCell(MULTI_CONFIG.COL_SUMMARY, MULTI_CONFIG.ROW_INVOICE_REAMING, "-", center_mid);
                    WriteCell(MULTI_CONFIG.COL_SUMMARY, MULTI_CONFIG.ROW_INVOICE_ADJUSTMENT, "-", center_bottom);

                    // 販売関連
                    WriteFormula(MULTI_CONFIG.COL_SUMMARY, MULTI_CONFIG.ROW_SALES_PRE,
                        MakeSumFormula(MULTI_CONFIG.COL_START_MONTH, MULTI_CONFIG.COL_SUMMARY - 1, MULTI_CONFIG.ROW_SALES_PRE),
                        productData.Product.IsSoldWeight ? double_top : int_top);
                    WriteFormula(MULTI_CONFIG.COL_SUMMARY, MULTI_CONFIG.ROW_SALES_PLAN,
                        MakeSumFormula(MULTI_CONFIG.COL_START_MONTH, MULTI_CONFIG.COL_SUMMARY - 1, MULTI_CONFIG.ROW_SALES_PLAN),
                        productData.Product.IsSoldWeight ? double_mid : int_mid);
                    WriteFormula(MULTI_CONFIG.COL_SUMMARY, MULTI_CONFIG.ROW_SALES_TREND,
                        MakeSumFormula(MULTI_CONFIG.COL_START_MONTH, MULTI_CONFIG.COL_SUMMARY - 1, MULTI_CONFIG.ROW_SALES_TREND),
                        productData.Product.IsSoldWeight ? double_mid : int_mid);
                    WriteFormula(MULTI_CONFIG.COL_SUMMARY, MULTI_CONFIG.ROW_SALES_ACTUAL,
                        MakeSumFormula(MULTI_CONFIG.COL_START_MONTH, MULTI_CONFIG.COL_SUMMARY - 1, MULTI_CONFIG.ROW_SALES_ACTUAL),
                        productData.Product.IsSoldWeight ? double_mid : int_mid);

                    // 比率関連
                    WriteCell(MULTI_CONFIG.COL_SUMMARY, MULTI_CONFIG.ROW_SALES_PRE_PERCENT, "-", center_mid);
                    WriteCell(MULTI_CONFIG.COL_SUMMARY, MULTI_CONFIG.ROW_SALES_PLAN_PERCENT, "-", center_bottom);

                    // 支店別は件数分
                    if (officeCount > 0)
                    {
                        for (int j = 0; j < officeCount; j++)
                        {
                            int baseRow = MULTI_CONFIG.ROW_OFFICE_START + MULTI_CONFIG.ROWS_ONE_OFFICE * j;
                            WriteFormula(MULTI_CONFIG.COL_SUMMARY, baseRow + MULTI_CONFIG.ROW_OFFICE_PRE,
                                MakeSumFormula(MULTI_CONFIG.COL_START_MONTH, MULTI_CONFIG.COL_SUMMARY - 1, baseRow + MULTI_CONFIG.ROW_OFFICE_PRE),
                                productData.Product.IsSoldWeight ? double_top : int_top);

                            WriteFormula(MULTI_CONFIG.COL_SUMMARY, baseRow + MULTI_CONFIG.ROW_OFFICE_ACTUAL,
                                MakeSumFormula(MULTI_CONFIG.COL_START_MONTH, MULTI_CONFIG.COL_SUMMARY - 1, baseRow + MULTI_CONFIG.ROW_OFFICE_ACTUAL),
                                productData.Product.IsSoldWeight ? double_bottom : int_bottom);
                        }
                    }

                    // 仕上げ
                    for (int i = 0; i <= MULTI_CONFIG.COL_SUMMARY; i++)
                        WriteStyle(i, MULTI_CONFIG.ROW_OFFICE_START + MULTI_CONFIG.ROWS_ONE_OFFICE * officeCount, top_border);

                    // ヘッダフッタほか
                    TargetSheet.Header.Right = HeaderFooter.Date;
                    TargetSheet.Footer.Center = HeaderFooter.Page;
                    TargetSheet.Autobreaks = true;
                    TargetSheet.FitToPage = true;

                    // IPrintSetup print = activeSheet.PrintSetup;
                    // activeSheet.PrintSetup.Scale = 90;
                    TargetSheet.PrintSetup.FitHeight = 1;
                    TargetSheet.PrintSetup.FitWidth = 1;
                    /*  8:A3 / 9:A4 / 11:A5 / 12:B4 / 13;B5 */
                    TargetSheet.PrintSetup.PaperSize = 9;       // A4用紙
                    TargetSheet.PrintSetup.Landscape = true;    // 横向き
                }


                using (MemoryStream ms = new MemoryStream())
                {
                    WorkBook.Write(ms);
                    CloseBook();
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                WriteAppLog(this.ToString(), ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 計算式を作成
        /// </summary>
        /// <param name="columnLeft">左列</param>
        /// <param name="columnRight">右列</param>
        /// <param name="rowIndex">行</param>
        /// <returns>合計計算式</returns>
        private string MakeSumFormula(int columnLeft, int columnRight, int rowIndex)
        {
            string leftCol = Convert.ToChar('A' + columnLeft).ToString();
            string rightCol = Convert.ToChar('A' + columnRight).ToString();
            string row = (rowIndex + 1).ToString();

            string result = "SUM(" + leftCol + row + ":" + rightCol + row + ")";
            return result;
        }
    }
}
