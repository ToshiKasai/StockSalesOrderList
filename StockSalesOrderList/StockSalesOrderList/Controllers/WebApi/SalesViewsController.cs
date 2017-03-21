using AutoMapper;
using AutoMapper.QueryableExtensions;
using StockSalesOrderList.App_GlobalResources;
using StockSalesOrderList.Models;
using StockSalesOrderList.Models.WebApi;
using StockSalesOrderList.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StockSalesOrderList.Controllers.WebApi
{
    /// <summary>
    /// 在庫販売情報表示用WEBAPI(独自仕様)
    /// </summary>
    [Authorize]
    public class SalesViewsController : BaseApiController
    {
        private SalesViewService service = null;

        public SalesViewsController() : base()
        {
            this.service = new SalesViewService();
        }

        [HttpGet]
        public IHttpActionResult Get([FromUri]SalesViewApiParameterModel param)
        {
            List<SalesViewApiModel> result;
            HttpStatusCode code = service.GetSalesViews(param, out result);

            if (code == HttpStatusCode.OK)
                return Ok(result);
            else
                return StatusCode(code);
        }

        public IHttpActionResult Get(int id, [FromUri]SalesViewApiParameterModel param)
        {
            SalesViewApiModel result;
            HttpStatusCode code = service.GetSalesView(param, out result, id);

            if (code == HttpStatusCode.OK)
                return Ok(result);
            else
                return StatusCode(code);
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody]SalesViewApiModel value)
        {
            HttpStatusCode code = service.SetSalesView(value);

            if (code != HttpStatusCode.OK)
                return StatusCode(code);

            SalesViewApiModel resultModel;
            SalesViewApiParameterModel param = new SalesViewApiParameterModel();
            param.Year = value.SalesList.Select(x => x.detail_date).Min().Year;
            param.Year += 1;
            code = service.GetSalesView(param, out resultModel, value.Product.Id);

            if (code == HttpStatusCode.OK)
                return Ok(resultModel);
            else
                return StatusCode(code);
        }

        [HttpPut]
        public IHttpActionResult Put(int? id, [FromBody]SalesViewApiModel value)
        {
            return StatusCode(HttpStatusCode.MethodNotAllowed);
        }

        public IHttpActionResult Delete(int id)
        {
            return StatusCode(HttpStatusCode.MethodNotAllowed);
        }

        [HttpGet]
        [Route("api/SalesViews/{id}/Current")]
        public IHttpActionResult GetCurrent(int id, [FromUri]SalesViewApiParameterModel param)
        {
            ProductModel product = dbContext.ProductModels.Where(p => p.Id == id).SingleOrDefault();
            if (product == null)
            {
                WriteAppLog("SalesViews/" + id.ToString() + "/Current", "パラメーターエラー：商品が存在しない");
                return BadRequest();
            }

            var stocks = dbContext.CurrentStockModels
                .Where(cs => cs.Deleted == false)
                .Where(cs => cs.ProductModelId == product.Id)
                .Select(cs => new { cs.WarehouseCode, cs.WarehouseName, cs.StateName, cs.ExpirationDate, cs.LogicalQuantity, cs.ActualQuantity })
                .OrderBy(no=>no.WarehouseCode).ThenBy(no=>no.ExpirationDate).ThenBy(no=>no.StateName)
                .ToList();

            var stockMaxDate = dbContext.CurrentStockModels
                .Where(cs => cs.Deleted == false)
                .Where(cs => cs.ProductModelId == product.Id)
                .Select(cs => cs.ModifiedDateTime).Max();

            var orders = dbContext.OrderModels
                .Where(od => od.Deleted == false)
                .Where(od => od.ProductModelId == product.Id)
                .Select(od => new { od.OrderNo, od.OrderDate, od.Order }).ToList();

            var orderMaxDate = dbContext.OrderModels
                .Where(od => od.Deleted == false)
                .Where(od => od.ProductModelId == product.Id)
                .Select(od => od.ModifiedDateTime).Max();

            var invoices = dbContext.InvoiceModels
                .Where(iv => iv.Deleted == false)
                .Where(iv => iv.ProductModelId == product.Id)
                .Select(iv => new { iv.InvoiceNo, iv.WarehouseCode, iv.ETA, iv.CustomsClearanceDate, iv.PurchaseDate, iv.Quantity }).ToList();

            var invoiceMaxDate = dbContext.InvoiceModels
                .Where(iv => iv.Deleted == false)
                .Where(iv => iv.ProductModelId == product.Id)
                .Select(iv => iv.ModifiedDateTime).Max();

            WriteAppLog("SalesViews/" + id.ToString() + "/Current", "処理成功");
            return Ok(new { stocks, orders, invoices, stockMaxDate, orderMaxDate, invoiceMaxDate });
        }

#region 販売動向
        [HttpGet]
        [Route("api/SalesViews/{id}/Trends")]
        public IHttpActionResult GetSalesTrend(int id, [FromUri]SalesViewApiParameterModel param)
        {
            ProductModel product = dbContext.ProductModels.Where(p => p.Id == id).SingleOrDefault();
            if (product == null)
            {
                WriteAppLog("SalesViews/" + id.ToString() + "/Trends", "パラメーターエラー：商品が存在しない");
                return BadRequest();
            }

            // パラメータは必須
            if (param.Year.HasValue == false)
            {
                WriteAppLog("SalesViews/" + id.ToString() + "/Trends", "パラメーターエラー：年度未指定");
                return StatusCode(HttpStatusCode.BadRequest);
            }

            DateTime startDate = DateTime.Parse((param.Year - 1).ToString() + "/10/1");
            DateTime endDate = startDate.AddYears(1);

            var trends = dbContext.SalesTrendModels
                .Where(st => st.ProductModelId == product.Id).Where(st => st.Deleted == false)
                .Where(st => st.TargetDate >= startDate).Where(st => st.TargetDate < endDate)
                .Select(st => new {
                    id = st.Id,
                    product_id = st.ProductModelId,
                    detail_date = st.TargetDate,
                    quantity = st.Sales,
                    comments = st.Comments,
                    user_id = st.UserModelId,
                    user_name = st.UserModel.Name
                }).ToList();

            WriteAppLog("SalesViews/" + id.ToString() + "/Trends", "処理成功（条件 Year=" + param.Year.ToString() + "）");
            return Ok(trends);
        }

        [HttpGet]
        [Route("api/SalesViews/{id}/Trends/{tid}")]
        public IHttpActionResult GetSalesTrend(int id, int tid, [FromUri]SalesViewApiParameterModel param)
        {
            try
            {
                var trends = dbContext.SalesTrendModels
                    .Where(st => st.Id == tid)
                    .Select(st => new
                    {
                        id = st.Id,
                        product_id = st.ProductModelId,
                        detail_date = st.TargetDate,
                        quantity = st.Sales,
                        comments = st.Comments,
                        user_id = st.UserModelId,
                        user_name = st.UserModel.Name
                    }).SingleOrDefault();

                if (trends == null)
                {
                    WriteAppLog("SalesViews/" + id.ToString() + "/Trends/" + tid.ToString(), "データが存在しない");
                    return NotFound();
                }

                WriteAppLog("SalesViews/" + id.ToString() + "/Trends/" + tid.ToString(), "処理成功");
                return Ok(trends);
            }
            catch (Exception)
            {
                WriteAppLog("SalesViews/" + id.ToString() + "/Trends/" + tid.ToString(), "不正な処理が発生");
                return BadRequest();
            }
        }


        [HttpPost]
        [Route("api/SalesViews/{id}/Trends")]
        public IHttpActionResult PostSalesTrend(int id, [FromBody]TrendApiModel model, [FromUri]SalesViewApiParameterModel param)
        {
            if (model == null || model.Id != 0)
            {
                WriteAppLog("POST SalesViews/" + id.ToString() + "/Trends", "パラメーターエラー");
                return BadRequest();
            }

            SalesTrendModel addmodel = new SalesTrendModel();
            addmodel.ProductModelId = model.Product_id;
            addmodel.TargetDate = model.Detail_date.Date;
            addmodel.Sales = model.Quantity;
            addmodel.Comments = model.Comments;
            addmodel.UserModelId = model.User_id;
            addmodel.Deleted = false;

            dbContext.Database.ExecuteSqlCommand(ContextResources.IncrementResetSalesTrend);
            dbContext.SalesTrendModels.Add(addmodel);
            dbContext.SaveChanges();
            dbContext.Database.ExecuteSqlCommand(ContextResources.IncrementResetSalesTrend);

            model.Id = addmodel.Id;
            model.User_name = dbContext.UserModels.Where(um => um.Id == addmodel.UserModelId).Select(um => um.Name).SingleOrDefault();
            if (model.Id == 0)
            {
                WriteAppLog("POST SalesViews/" + id.ToString() + "/Trends", "登録処理に失敗");
                return BadRequest();
            }

            WriteAppLog("POST SalesViews/" + id.ToString() + "/Trends", "処理成功");
            return Ok(model);
        }

        [HttpPut]
        [Route("api/SalesViews/{id}/Trends/{tid}")]
        public IHttpActionResult PutSalesTrend(int id, int tid, [FromBody]TrendApiModel model, [FromUri]SalesViewApiParameterModel param)
        {
            if (tid == 0 || model == null || model.Id == 0)
            {
                WriteAppLog("PUT SalesViews/" + id.ToString() + "/Trends/" + tid.ToString(), "パラメーターエラー");
                return BadRequest();
            }

            SalesTrendModel work = dbContext.SalesTrendModels.Where(st => st.Id == tid).SingleOrDefault();
            if (work == null)
            {
                WriteAppLog("PUT SalesViews/" + id.ToString() + "/Trends/" + tid.ToString(), "データが存在しない");
                return NotFound();
            }

            work.TargetDate = model.Detail_date;
            work.Sales = model.Quantity;
            work.Comments = model.Comments;
            work.UserModelId = model.User_id;
            dbContext.SaveChanges();
            model.User_name = dbContext.UserModels.Where(um => um.Id == model.User_id).Select(um => um.Name).SingleOrDefault();

            WriteAppLog("PUT SalesViews/" + id.ToString() + "/Trends/" + tid.ToString(), "処理成功");
            return Ok(model);
        }

        [HttpDelete]
        [Route("api/SalesViews/{id}/Trends/{tid}")]
        public IHttpActionResult DeleteSalesTrend(int id, int tid, [FromUri]SalesViewApiParameterModel param)
        {
            if (tid == 0)
            {
                WriteAppLog("DELETE SalesViews/" + id.ToString() + "/Trends/" + tid.ToString(), "パラメーターエラー");
                return BadRequest();
            }

            SalesTrendModel work = dbContext.SalesTrendModels.Where(st => st.Id == tid).SingleOrDefault();
            if (work == null)
            {
                WriteAppLog("DELETE SalesViews/" + id.ToString() + "/Trends/" + tid.ToString(), "データが存在しない");
                return NotFound();
            }

            work.Deleted = true;
            dbContext.SaveChanges();

            WriteAppLog("DELETE SalesViews/" + id.ToString() + "/Trends/" + tid.ToString(), "処理成功");

            return Ok();
        }
#endregion
    }
}
