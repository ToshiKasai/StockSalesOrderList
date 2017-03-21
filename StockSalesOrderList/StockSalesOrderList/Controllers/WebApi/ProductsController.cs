using AutoMapper;
using AutoMapper.QueryableExtensions;
using StockSalesOrderList.App_GlobalResources;
using StockSalesOrderList.Models;
using StockSalesOrderList.Models.WebApi;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StockSalesOrderList.Controllers.WebApi
{
    /// <summary>
    /// 商品情報管理用WEBAPI
    /// </summary>
    [Authorize]
    public class ProductsController : BaseApiController
    {
        #region 商品情報
        [HttpGet]
        public IHttpActionResult Get([FromUri]ProductApiParameterModel param)
        {
            try
            {
                List<ProductApiModel> result;
                IQueryable<ProductModel> query;
                if (param.GroupId.HasValue)
                    query = dbContext.GroupProductModels.Where(gp => gp.Deleted == false).Where(gp => gp.GroupModelId == (int)param.GroupId).Select(gp => gp.ProductModel).OrderBy(p => p.Id);
                else if (param.MakerId.HasValue)
                    query = dbContext.ProductModels.Where(p => p.MakerModelId == (int)param.MakerId).OrderBy(p => p.Id);
                else
                    query = dbContext.ProductModels.OrderBy(p=>p.Id);
                if (!param.Deleted)
                    query = query.Where(p => p.Deleted == false);
                if (param.Enabled)
                    query = query.Where(p => p.Enabled == true);

                if (param.Limit.HasValue)
                {
                    if (param.Page.HasValue)
                        query = query.Skip((int)param.Limit * (int)param.Page).Take((int)param.Limit);
                    else
                        query = query.Take((int)param.Limit);
                }

                result = query.ProjectTo<ProductApiModel>().ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        public IHttpActionResult Get(int id, [FromUri]ProductApiParameterModel param)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(Messages.ApiIllegalParameter);

                ProductModel model;
                model = dbContext.ProductModels.Where(x => x.Id == id).SingleOrDefault();
                if (model == null)
                    return NotFound();

                ProductApiModel result = Mapper.Map<ProductApiModel>(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody]ProductApiModel value)
        {
            return StatusCode(HttpStatusCode.MethodNotAllowed);
        }

        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody]ProductApiModel value)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(Messages.ApiIllegalParameter);

                using (DbContextTransaction tx =
                    dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    value.Id = id;
                    ProductModel product = dbContext.ProductModels.Where(m => m.Id == value.Id).SingleOrDefault();
                    if (product == null)
                    {
                        tx.Rollback();
                        return NotFound();
                    }

                    product.PaletteQuantity = value.PaletteQuantity;
                    product.CartonQuantity = value.CartonQuantity;
                    product.CaseHeight = value.CaseHeight;
                    product.CaseWidth = value.CaseWidth;
                    product.CaseDepth = value.CaseDepth;
                    product.CaseCapacity = value.CaseCapacity;
                    product.LeadTime = value.LeadTime;
                    product.OrderInterval = value.OrderInterval;
                    product.OldProductModelId = value.OldProductModelId;
                    product.Magnification = value.Magnification;
                    product.MinimumOrderQuantity = value.MinimumOrderQuantity;
                    product.Enabled = value.Enabled;
                    dbContext.Entry(product).State = EntityState.Modified;

                    // dbContext.Database.Log = (log) => System.Diagnostics.Debug.WriteLine(log);
                    if (dbContext.SaveChanges() == 0)
                    {
                        tx.Rollback();
                        return Conflict();
                    }

                    tx.Commit();
                    return Ok(value);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            return StatusCode(HttpStatusCode.MethodNotAllowed);
        }
        [HttpGet]
        [Route("api/Products/Pages")]
        public IHttpActionResult GetProductMaxPages([FromUri]ProductApiParameterModel param)
        {
            try
            {
                int maxcount = 0;
                int maxpages = 0;
                IQueryable<ProductModel> query;
                if (param.GroupId.HasValue)
                    query = dbContext.GroupProductModels.Where(gp => gp.Deleted == false).Where(gp => gp.GroupModelId == (int)param.GroupId).Select(gp => gp.ProductModel).OrderBy(p => p.Id);
                else if (param.MakerId.HasValue)
                    query = dbContext.ProductModels.Where(p => p.MakerModelId == (int)param.MakerId).OrderBy(p => p.Id);
                else
                    query = dbContext.ProductModels.OrderBy(p => p.Id);
                if (!param.Deleted)
                    query = query.Where(p => p.Deleted == false);
                if (param.Enabled)
                    query = query.Where(p => p.Enabled == true);
                maxcount = query.Count();

                if (param.Limit.HasValue)
                {
                    maxpages = (int)(maxcount / (int)param.Limit);
                    if ((int)(maxcount % (int)param.Limit) > 0)
                    {
                        maxpages += 1;
                    }
                }

                return Ok(new { count = maxcount, pages = maxpages });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        #endregion

        #region 商品別データ
        [HttpGet]
        [Route("api/Products/{id}/SalesViews/{year}")]
        public IHttpActionResult GetSalesViews(int id, int year, [FromUri]BaseApiParameterModel param)
        {
            return StatusCode(HttpStatusCode.MethodNotAllowed);
        }

        [HttpPost]
        [Route("api/Products/{id}/SalesViews/")]
        [ValidationRequired(prefix = "item")]
        public IHttpActionResult SetSalesViews([FromBody]string item)
        {
            return StatusCode(HttpStatusCode.MethodNotAllowed);
        }
        #endregion
    }
}
