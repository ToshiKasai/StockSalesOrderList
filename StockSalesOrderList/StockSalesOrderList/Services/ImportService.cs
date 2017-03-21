using Microsoft.AspNet.Identity;
using StockSalesOrderList.App_GlobalResources;
using StockSalesOrderList.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;

namespace StockSalesOrderList.Services
{
    public class ImportService : CustomExcelService
    {
        // メッセージ定義：ゆくゆくはリソース化
        private const string CErrorBookOpen = "アップロードファイルが処理できません。";
        private const string CErrorSheetOpen = "シートが正常に読み込めませんでした。";
        private const string CReadErrorCode = "商品コードが正常に読み込めませんでした。";
        private const string CIllegalProductCode = "商品コード[@code]は正しい商品コードではありません。";
        private const string CReadErrorDate = "商品コード[@code]の年度指定が判別できません。";
        private const string COfficeError = "システム内の事務所情報が正しくありません。管理者へ問い合わせください。";
        private const string CReadErrorMessage = "商品コード[@code]の年月[@date]の@nameが正常に処理できません。";
        private const string CErrorBookFormat = "アップロードされた書式が正しくない可能性があります。";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ImportService() : base()
        {
        }

        /// <summary>
        /// エラーメッセージを組み立てる
        /// </summary>
        /// <param name="productCode"></param>
        /// <param name="targetDate"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        private string MakerReadErrorMessage(string productCode,DateTime targetDate,string columnName)
        {
            return CReadErrorMessage.Replace("@code", productCode).Replace("@date", targetDate.ToString("yyyy年MM月")).Replace("@name", columnName);
        }

        public bool ReadXlsxToSalesView(Stream dataStream, out string errmessage)
        {
            errmessage = string.Empty;
            try
            {
                int office_id;
                int baseRow;
                int count = 0;
                string productCode;
                ProductModel product;
                int targetYear;
                DateTime targetDate;
                decimal readDecimal;

                if (OpenBook(dataStream) == false)
                {
                    errmessage = CErrorBookOpen;
                    return false;
                }

                office_id = dbContext.OfficeModels.Where(o => o.Code == ContextResources.CompanyCode).Select(o => o.Id).SingleOrDefault();
                if (office_id == 0)
                {
                    errmessage = COfficeError;
                    return false;
                }

                if (base.GetSheetByIndex(0) == null)
                {
                    errmessage = CErrorSheetOpen;
                    return false;
                }

                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    while (true)
                    {
                        // 空白行も加味したベース位置を算出(ベース位置は年月表示行）
                        baseRow = ONE_ROW.ROW_START + count * ONE_ROW.ROWS_ONE_PRODUCT + 2;

                        // 次の条件として「商品コード」の存在を判定する
                        if (!TryReadCell(0, baseRow - 1, out productCode) || productCode != "商品コード")
                        {
                            if (count == 0)
                            {
                                tx.Rollback();
                                errmessage = CErrorBookFormat;
                                return false;
                            }
                            break;
                        }

                        if (!TryReadCell(1, baseRow - 1, out productCode))
                        {
                            tx.Rollback();
                            errmessage = CReadErrorCode;
                            return false;
                        }

                        product = dbContext.ProductModels.Where(pm => pm.Code == productCode).SingleOrDefault();
                        if (product == null)
                        {
                            tx.Rollback();
                            errmessage = CIllegalProductCode.Replace("@code", productCode);
                            return false;
                        }

                        if (!TryGetYear(0, baseRow, out targetYear))
                        {
                            tx.Rollback();
                            errmessage = CReadErrorDate.Replace("@code", productCode);
                            return false;
                        }

                        WriteAppLog(this.ToString(), "商品コード[" + productCode + "] 年度[" + targetYear.ToString() + "] の処理");
                        // 月ループ(10月～9月+10月～3月)
                        targetDate = DateTime.Parse(targetYear.ToString() + "-10-1");
                        for (int colCount = 0; colCount < ONE_ROW.COLS_REPEAT_MONTH; colCount++)
                        {
                            TradeModel trade = dbContext.TradeModels.Where(tm => tm.ProductModelId == product.Id).Where(tm => tm.TargetDate == targetDate).SingleOrDefault();
                            if (trade == null)
                            {
                                trade = new TradeModel();
                                trade.ProductModelId = product.Id;
                                trade.TargetDate = targetDate;
                            }
                            else if (trade.Deleted == true)
                            {
                                trade.Order = 0;
                                trade.OrderPlan = 0;
                                trade.InvoicePlan = 0;
                                trade.Invoice = 0;
                                trade.RemainingInvoice = 0;
                                trade.AdjustmentInvoice = 0;
                                trade.Deleted = false;
                            }
                            if (!TryReadCell(ONE_ROW.COL_START_MONTH + colCount, baseRow + ONE_ROW.ROW_ORDER_PLAN, out readDecimal))
                            {
                                tx.Rollback();
                                errmessage = MakerReadErrorMessage(productCode, targetDate, "発注予定");
                                return false;
                            }
                            trade.OrderPlan = readDecimal;
                            if (!TryReadCell(ONE_ROW.COL_START_MONTH + colCount, baseRow + ONE_ROW.ROW_INVOICE_PLAN, out readDecimal))
                            {
                                tx.Rollback();
                                errmessage = MakerReadErrorMessage(productCode, targetDate, "入荷予定");
                                return false;
                            }
                            trade.InvoicePlan = readDecimal;
                            if (!TryReadCell(ONE_ROW.COL_START_MONTH + colCount, baseRow + ONE_ROW.ROW_INVOICE_ADJUSTMENT, out readDecimal))
                            {
                                tx.Rollback();
                                errmessage = MakerReadErrorMessage(productCode, targetDate, "入荷残数調整");
                                return false;
                            }
                            trade.AdjustmentInvoice = readDecimal;
                            dbContext.SaveChanges();

                            SalesModel sale = dbContext.SalesModels.Where(sm => sm.ProductModelId == product.Id).Where(sm => sm.TargetDate == targetDate).Where(sm => sm.OfficeModelId == office_id).SingleOrDefault();
                            if (sale == null)
                            {
                                sale = new SalesModel();
                                sale.ProductModelId = product.Id;
                                sale.TargetDate = targetDate;
                                sale.OfficeModelId = office_id;
                            }
                            else if (sale.Deleted == true)
                            {
                                sale.Plan = 0;
                                sale.Sales = 0;
                                sale.Deleted = false;
                            }
                            if (!TryReadCell(ONE_ROW.COL_START_MONTH + colCount, baseRow + ONE_ROW.ROW_SALES_PLAN, out readDecimal))
                            {
                                tx.Rollback();
                                errmessage = MakerReadErrorMessage(productCode, targetDate, "販売予算");
                                return false;
                            }
                            sale.Plan = readDecimal;
                            dbContext.SaveChanges();

                            // 次の月へ
                            targetDate = targetDate.AddMonths(1);
                        }

                        // 再計算(インボイス残数の再計算)
                        dbContext.Database.ExecuteSqlCommand(
                            "call _recalculation_invoicedata_id(@p0,@p1,@p2)",
                            DateTime.Parse(targetYear.ToString() + "-10-1"), DateTime.Now.Date, product.Id);

                        // 次商品へ
                        count++;
                    }
                    tx.Commit();
                }

                CloseBook();
                return true;
            }
            catch (Exception ex)
            {
                CloseBook();
                WriteAppLog(this.ToString(), ex.Message);
                errmessage = "内部処理エラーです。繰り返される場合は管理者へ連絡ください。";
                return false;
            }
        }

        /// <summary>
        /// 年度を読み込む
        /// </summary>
        /// <param name="colIndex"></param>
        /// <param name="rowIndex"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private bool TryGetYear(int colIndex, int rowIndex, out int param)
        {
            param = 0;
            try
            {
                DateTime readDate;
                string readString;
                decimal readDecimal;

                if (TryReadCell(colIndex, rowIndex, out readDate))
                {
                    param = readDate.Year;
                }
                else if (TryReadCell(colIndex, rowIndex, out readDecimal))
                {
                    param = (int)readDecimal;
                }
                else if (TryReadCell(colIndex, rowIndex, out readString))
                {
                    readString = readString.Replace("年", "").Trim();
                    param = int.Parse(readString);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}