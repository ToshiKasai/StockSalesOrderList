using AutoMapper.QueryableExtensions;
using StockSalesOrderList.App_GlobalResources;
using StockSalesOrderList.Models;
using StockSalesOrderList.Models.WebApi;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;

namespace StockSalesOrderList.Services
{
    /// <summary>
    /// 在庫販売情報用サービス
    /// </summary>
    public class SalesViewService : BaseService
    {
        /// <summary>
        /// 内部エラーメッセージ用(そのうちリソース化)
        /// </summary>
        private struct ErrorMessage
        {
            public const string CKeyError = "パラメーターエラー：キー情報不足";
            public const string CParamError = "パラメーターエラー";
            public const string CYearError = "パラメーターエラー：年度未指定";
            public const string CProductError = "パラメーターエラー：商品が存在しない";
            public const string CCompleteSingle = "処理成功（条件 Year=@year）";
            public const string CCompleteMulti = "処理成功（条件 @message）";
            public const string CCompleteSet = "処理成功（条件  productId=@product）";
            public const string CException = "内部エラー発生：";
            public const string COfficeIdError = "障害：全社用オフィスＩＤが見つからない";
        }

        /// <summary>
        /// ログ出力メッセージ用(内部処理用)
        /// </summary>
        private string logMessage = string.Empty;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SalesViewService() : base()
        {
            this.logMessage = string.Empty;
        }

        /// <summary>
        /// 条件に合致する商品一覧の抽出
        /// </summary>
        /// <param name="param">条件パラメーター</param>
        /// <returns>商品リスト</returns>
        private List<ProductApiModel> GetProductList(SalesViewApiParameterModel param)
        {
            try
            {
                IQueryable<ProductModel> query;
                if (param.GroupId.HasValue)
                {
                    this.logMessage += " GroupId=" + param.GroupId.ToString();
                    query = dbContext.GroupProductModels.Where(gp => gp.Deleted == false).Where(gp => gp.GroupModelId == (int)param.GroupId).Select(gp => gp.ProductModel).OrderBy(p => p.Id);
                }
                else if (param.MakerId.HasValue)
                {
                    this.logMessage += " MakerId=" + param.MakerId.ToString();
                    query = dbContext.ProductModels.Where(p => p.MakerModelId == (int)param.MakerId).OrderBy(p => p.Id);
                }
                else
                    query = dbContext.ProductModels.OrderBy(p => p.Id);
                if (!param.Deleted)
                {
                    this.logMessage += " Deleted=false";
                    query = query.Where(p => p.Deleted == false);
                }
                if (param.Enabled)
                {
                    this.logMessage += " Enabled=true";
                    query = query.Where(p => p.Enabled == true);
                }

                if (param.Limit.HasValue)
                {
                    if (param.Page.HasValue)
                    {
                        this.logMessage += " Limit=" + param.Limit.ToString() + " Page=" + param.Page.ToString();
                        query = query.Skip((int)param.Limit * (int)param.Page).Take((int)param.Limit);
                    }
                    else
                    {
                        this.logMessage += " Limit=" + param.Limit.ToString();
                        query = query.Take((int)param.Limit);
                    }
                }
                return query.ProjectTo<ProductApiModel>().ToList();
            }
            catch (Exception ex)
            {
                WriteAppLog(this.ToString() + ".GetProductList", "内部エラー発生：" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 販売データを取得して返却用オブジェクトを生成する
        /// </summary>
        /// <param name="param">条件パラメーター</param>
        /// <param name="result">返却用オブジェクト</param>
        /// <param name="productId">対象商品ＩＤ</param>
        /// <param name="months">取得期間(単位は月、省略時は12ヶ月)</param>
        /// <returns>処理結果のステータスコード</returns>
        public HttpStatusCode GetSalesView(SalesViewApiParameterModel param, out SalesViewApiModel result, int productId = 0, int months = 12)
        {
            this.logMessage = string.Empty;
            result = null;

            try
            {
                // パラメータは必須
                if (productId <= 0)
                {
                    WriteAppLog(this.ToString() + ".GetSalesView/" + productId.ToString(), ErrorMessage.CParamError);
                    return HttpStatusCode.BadRequest;
                }

                if (param.Year.HasValue == false)
                {
                    WriteAppLog(this.ToString() + ".GetSalesView/" + productId.ToString(), ErrorMessage.CYearError);
                    return HttpStatusCode.BadRequest;
                }

                ProductApiModel product = dbContext.ProductModels.Where(p => p.Id == productId).ProjectTo<ProductApiModel>().SingleOrDefault();
                if (product == null)
                {
                    WriteAppLog(this.ToString() + ".GetSalesView/" + productId.ToString(), ErrorMessage.CProductError);
                    return HttpStatusCode.BadRequest;
                }

                List<SalesViewsTempModel> salesList;
                DateTime startDate = DateTime.Parse((param.Year - 1).ToString() + "/10/1");
                DateTime endDate = startDate.AddMonths(months);
                salesList = dbContext.Database.SqlQuery<SalesViewsTempModel>(
                    ContextResources.SelectSalesViews, startDate, endDate, product.Id
                ).ToList<SalesViewsTempModel>();

                ICollection<OfficeModel> office = dbContext.OfficeModels.Where(o => o.Deleted == false).OrderBy(o => o.Code).ToList();
                List<SalesOfficeTempModel> salesOfficeList;
                salesOfficeList = dbContext.Database.SqlQuery<SalesOfficeTempModel>(
                    ContextResources.SelectOfficesSalesViews, startDate, endDate, product.Id
                ).ToList<SalesOfficeTempModel>();

                // 在庫予測計算用に１ヶ月多めに取得（貿易のみ）
                DateTime check = startDate.AddMonths(-1);

                result = new SalesViewApiModel();

                result.Product = product;
                result.SalesList = new List<SalesViewsTempModel>();
                result.OfficeSales = new List<ICollection<SalesOfficeTempModel>>();

                for (; check <= endDate; check = check.AddMonths(1))
                {
                    SalesViewsTempModel work = salesList.Where(x => x.product_id == product.Id)
                        .Where(x => x.detail_date == check).SingleOrDefault();
                    if (work == null)
                    {
                        SalesViewsTempModel tempModel = new SalesViewsTempModel();
                        tempModel.product_id = product.Id;
                        tempModel.detail_date = check;
                        result.SalesList.Add(tempModel);
                    }
                    else
                    {
                        result.SalesList.Add(work);
                    }

                    ICollection<SalesOfficeTempModel> workOffice = new List<SalesOfficeTempModel>();
                    foreach (var value in office)
                    {
                        SalesOfficeTempModel wk = salesOfficeList.Where(so => so.detail_date == check).Where(so => so.office_id == value.Id).SingleOrDefault();
                        if (wk == null)
                        {
                            SalesOfficeTempModel tempModel = new SalesOfficeTempModel();
                            tempModel.product_id = product.Id;
                            tempModel.detail_date = check;
                            tempModel.office_id = value.Id;
                            tempModel.office_name = value.Name;
                            tempModel.sales_plan = 0;
                            tempModel.sales_actual = 0;
                            workOffice.Add(tempModel);
                        }
                        else
                        {
                            workOffice.Add(wk);
                        }
                    }
                    result.OfficeSales.Add(workOffice);
                }
                WriteAppLog(this.ToString() + ".GetSalesView/" + productId.ToString(), ErrorMessage.CCompleteSingle.Replace("@year", param.Year.ToString()));
                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                WriteAppLog(this.ToString() + ".GetSalesView/" + productId.ToString(), ErrorMessage.CException + ex.Message);
                return HttpStatusCode.InternalServerError;
            }
        }

        /// <summary>
        /// 販売データを取得して返却用オブジェクトを生成する
        /// </summary>
        /// <param name="param">条件パラメーター</param>
        /// <param name="result">返却用オブジェクト</param>
        /// <param name="months">取得期間(単位は月、省略時は12ヶ月)</param>
        /// <returns>処理結果のステータスコード</returns>
        public HttpStatusCode GetSalesViews(SalesViewApiParameterModel param, out List<SalesViewApiModel> result, int months = 12)
        {
            this.logMessage = string.Empty;
            result = null;

            try
            {
                // 必須パラメーターのチェック
                if (param.GroupId.HasValue == false && param.MakerId.HasValue == false)
                {
                    WriteAppLog(this.ToString() + ".GetSalesViews", ErrorMessage.CKeyError);
                    return HttpStatusCode.BadRequest;
                }
                if (param.Year.HasValue == false)
                {
                    WriteAppLog(this.ToString() + ".GetSalesViews", ErrorMessage.CYearError);
                    return HttpStatusCode.BadRequest;
                }
                this.logMessage += " Year=" + param.Year.ToString();

                // 商品一覧の取得
                List<ProductApiModel> products = this.GetProductList(param);
                List<int> productIds = products.Select(p => p.Id).ToList<int>();

                // 事業所リストの取得
                ICollection<OfficeModel> office = dbContext.OfficeModels.Where(o => o.Deleted == false).OrderBy(o => o.Code).ToList();

                // データ取得準備
                string productList = string.Join(",", productIds);
                string sql = ContextResources.SelectSalesViews.Replace("@p2", productList);

                // 全体の販売実績データ
                List<SalesViewsTempModel> salesList;
                DateTime startDate = DateTime.Parse((param.Year - 1).ToString() + "/10/1");
                DateTime endDate = startDate.AddMonths(months);
                salesList = dbContext.Database.SqlQuery<SalesViewsTempModel>(sql, startDate, endDate).ToList<SalesViewsTempModel>();

                // 事業所別
                List<SalesOfficeTempModel> salesOfficeList;
                sql = ContextResources.SelectOfficesSalesViews.Replace("@p2", productList);
                salesOfficeList = dbContext.Database.SqlQuery<SalesOfficeTempModel>(
                    sql, startDate.AddYears(-1), endDate
                ).ToList<SalesOfficeTempModel>();


                // 返却用の準備
                result = new List<SalesViewApiModel>();
                foreach (var product in products)
                {
                    // 在庫予測計算用に１ヶ月多めに取得（貿易のみ）
                    DateTime check = startDate.AddMonths(-1); ;
                    SalesViewApiModel addModel = new SalesViewApiModel();
                    addModel.Product = product;
                    addModel.SalesList = new List<SalesViewsTempModel>();
                    addModel.OfficeSales = new List<ICollection<SalesOfficeTempModel>>();

                    for (; check <= endDate; check = check.AddMonths(1))
                    {
                        SalesViewsTempModel work = salesList.Where(x => x.product_id == addModel.Product.Id)
                            .Where(x => x.detail_date == check).SingleOrDefault();
                        if (work == null)
                        {
                            SalesViewsTempModel tempModel = new SalesViewsTempModel();
                            tempModel.product_id = addModel.Product.Id;
                            tempModel.detail_date = check;
                            addModel.SalesList.Add(tempModel);
                        }
                        else
                        {
                            addModel.SalesList.Add(work);
                        }

                        ICollection<SalesOfficeTempModel> workOffice = new List<SalesOfficeTempModel>();
                        foreach (var ofs in office)
                        {
                            SalesOfficeTempModel ofsData = salesOfficeList.Where(x => x.product_id == addModel.Product.Id)
                                .Where(x => x.detail_date == check).Where(x => x.office_id == ofs.Id).SingleOrDefault();
                            if (ofsData == null)
                            {
                                SalesOfficeTempModel tempModel = new SalesOfficeTempModel();
                                tempModel.product_id = product.Id;
                                tempModel.detail_date = check;
                                tempModel.office_id = ofs.Id;
                                tempModel.office_name = ofs.Name;
                                workOffice.Add(tempModel);
                            }
                            else
                            {
                                workOffice.Add(ofsData);
                            }
                        }
                        addModel.OfficeSales.Add(workOffice);
                    }
                    result.Add(addModel);
                }
                WriteAppLog(this.ToString() + ".GetSalesViews", ErrorMessage.CCompleteMulti.Replace("@message", this.logMessage));
                return HttpStatusCode.OK;
            }
            catch(Exception ex)
            {
                WriteAppLog(this.ToString() + ".GetSalesViews", ErrorMessage.CException + ex.Message);
                return HttpStatusCode.InternalServerError;
            }
        }

        /// <summary>
        /// 販売データの内容に合わせてデータを更新する
        /// </summary>
        /// <param name="data">更新用データ</param>
        /// <returns>処理結果ステータスコード</returns>
        public HttpStatusCode SetSalesView(SalesViewApiModel data)
        {
            try
            {
                if (data == null)
                {
                    WriteAppLog(this.ToString() + ".SetSalesView", ErrorMessage.CParamError);
                    return HttpStatusCode.BadRequest;
                }

                if (data.Product == null || data.Product.Id <= 0)
                {
                    WriteAppLog(this.ToString() + ".SetSalesView", ErrorMessage.CKeyError);
                    return HttpStatusCode.BadRequest;
                }

                int product_id = dbContext.ProductModels.Where(pd => pd.Id == data.Product.Id).Select(pd => pd.Id).SingleOrDefault();
                if (product_id == 0)
                {
                    WriteAppLog(this.ToString() + ".SetSalesView", ErrorMessage.CProductError);
                    return HttpStatusCode.BadRequest;
                }

                int office_id = dbContext.OfficeModels.Where(o => o.Code == "**").Select(o => o.Id).SingleOrDefault();
                if (office_id == 0)
                {
                    WriteAppLog(this.ToString() + ".SetSalesView", ErrorMessage.COfficeIdError);
                    return HttpStatusCode.InternalServerError;
                }

                // 最小日付と最大日付
                DateTime MinDate = data.SalesList.Select(sl => sl.detail_date).Min();
                DateTime MaxDate = data.SalesList.Select(sl => sl.detail_date).Max();

                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    foreach (var month in data.SalesList)
                    {
                        if (month.detail_date != MinDate)
                        {
                            // 販売予算を更新
                            SalesModel sales = dbContext.SalesModels.Where(sls => sls.ProductModelId == product_id)
                                .Where(sls => sls.TargetDate == month.detail_date)
                                .Where(sls => sls.OfficeModelId == office_id).SingleOrDefault();
                            if (sales != null)
                            {
                                // 削除データが表示される際に不整合を起こさないため
                                if (sales.Deleted)
                                    sales.Sales = 0;

                                sales.Plan = month.sales_plan;
                                sales.Deleted = false;
                            }
                            else
                            {
                                if (month.sales_plan != 0)
                                {
                                    sales = new SalesModel();
                                    sales.ProductModelId = product_id;
                                    sales.TargetDate = month.detail_date;
                                    sales.Plan = month.sales_plan;
                                    sales.OfficeModelId = office_id;
                                    dbContext.SalesModels.Add(sales);
                                }
                            }
                            dbContext.SaveChanges();

                            // 貿易
                            TradeModel trades = dbContext.TradeModels.Where(trd => trd.ProductModelId == product_id)
                                .Where(trd => trd.TargetDate == month.detail_date).SingleOrDefault();
                            if (trades != null)
                            {
                                // 削除データが表示される際に不整合を起こさないため
                                if (trades.Deleted)
                                {
                                    trades.Order = 0;
                                    trades.Invoice = 0;
                                    trades.RemainingInvoice = 0;
                                }

                                trades.OrderPlan = month.order_plan;
                                trades.InvoicePlan = month.invoice_plan;
                                trades.AdjustmentInvoice = month.invoice_adjust;
                                trades.Deleted = false;
                            }
                            else
                            {
                                trades = new TradeModel();
                                trades.ProductModelId = product_id;
                                trades.TargetDate = month.detail_date;
                                trades.OrderPlan = month.order_plan;
                                trades.InvoicePlan = month.invoice_plan;
                                trades.AdjustmentInvoice = month.invoice_adjust;
                                dbContext.TradeModels.Add(trades);
                            }
                            dbContext.SaveChanges();
                        }
                    }
                    tx.Commit();
                }

                // 再計算
                dbContext.Database.ExecuteSqlCommand(
                    "call _recalculation_invoicedata_id(@p0,@p1,@p2)",
                    MinDate, DateTime.Now.Date, product_id);

                WriteAppLog(this.ToString() + ".SetSalesView", ErrorMessage.CCompleteSet.Replace("@product", product_id.ToString()));
                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                WriteAppLog(this.ToString() + ".SetSalesView", ErrorMessage.CException + ex.Message);
                return HttpStatusCode.InternalServerError;
            }
        }
    }
}
