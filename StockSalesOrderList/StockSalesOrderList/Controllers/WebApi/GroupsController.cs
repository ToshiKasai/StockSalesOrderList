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
    /// グループ情報管理用WEBAPI
    /// </summary>
    [Authorize]
    public class GroupsController : BaseApiController
    {
        #region グループ情報
        [HttpGet]
        public IHttpActionResult Get([FromUri]GroupApiParameterModel param)
        {
            try
            {
                List<GroupApiModel> result;
                IQueryable<GroupModel> query;
                query = dbContext.GroupModels.OrderBy(x => x.Id);
                if (!param.Deleted)
                    query = query.Where(x => x.Deleted == false);
                if (param.MakerId.HasValue)
                    query = query.Where(x => x.MakerModelId == (int)param.MakerId);

                if (param.Limit.HasValue)
                {
                    if (param.Page.HasValue)
                        query = query.Skip((int)param.Limit * (int)param.Page).Take((int)param.Limit);
                    else
                        query = query.Take((int)param.Limit);
                }
                result = query.ProjectTo<GroupApiModel>().ToList();

                // if (result == null || result.Count == 0)
                //     return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        public IHttpActionResult Get(int id, [FromUri]BaseApiParameterModel param)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(Messages.ApiIllegalParameter);

                GroupApiModel result = dbContext.GroupModels.Where(x => x.Id == id)
                    .ProjectTo<GroupApiModel>().SingleOrDefault();
                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [ValidationRequired(prefix = "value")]
        public IHttpActionResult Post([FromBody]GroupApiModel value)
        {
            try
            {
                dbContext.Database.ExecuteSqlCommand(ContextResources.IncrementResetGroup);
                using (DbContextTransaction tx =
                    dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    GroupModel group = dbContext.GroupModels.Where(x => x.Code == value.Code).SingleOrDefault();
                    if (group != null)
                    {
                        tx.Rollback();
                        return Conflict();
                    }

                    group = new GroupModel();
                    group.Code = value.Code;
                    group.Name = value.Name;
                    group.MakerModelId = value.MakerModelId;
                    group.ContainerModelId = value.ContainerModelId;
                    group.IsCapacity = value.IsCapacity;
                    group.ContainerCapacityBt20Dry = value.ContainerCapacityBt20Dry;
                    group.ContainerCapacityBt20Reefer = value.ContainerCapacityBt20Reefer;
                    group.ContainerCapacityBt40Dry = value.ContainerCapacityBt40Dry;
                    group.ContainerCapacityBt40Reefer = value.ContainerCapacityBt40Reefer;
                    group.Deleted = value.Deleted;

                    dbContext.GroupModels.Add(group);
                    if (dbContext.SaveChanges() == 0)
                    {
                        tx.Rollback();
                        return BadRequest(ModelState.GetErrorsDelprefix("value"));
                    }
                    dbContext.Database.ExecuteSqlCommand(ContextResources.IncrementResetGroup);
                    tx.Commit();
                    return Created(Request.RequestUri + "/" + group.Id, group);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [ValidationRequired(prefix = "value")]
        public IHttpActionResult Put(int id, [FromBody]GroupApiModel value)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(Messages.ApiIllegalParameter);

                using (DbContextTransaction tx =
                    dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    value.Id = id;
                    GroupModel group = dbContext.GroupModels.Where(m => m.Id == value.Id).SingleOrDefault();
                    if (group == null)
                    {
                        tx.Rollback();
                        return NotFound();
                    }

                    group.Code = value.Code;
                    group.Name = value.Name;
                    group.MakerModelId = value.MakerModelId;
                    group.ContainerModelId = value.ContainerModelId;
                    group.IsCapacity = value.IsCapacity;
                    group.ContainerCapacityBt20Dry = value.ContainerCapacityBt20Dry;
                    group.ContainerCapacityBt20Reefer = value.ContainerCapacityBt20Reefer;
                    group.ContainerCapacityBt40Dry = value.ContainerCapacityBt40Dry;
                    group.ContainerCapacityBt40Reefer = value.ContainerCapacityBt40Reefer;
                    group.Deleted = value.Deleted;

                    dbContext.Entry(group).State = EntityState.Modified;
                    if (dbContext.SaveChanges() == 0)
                    {
                        tx.Rollback();
                        return BadRequest(ModelState.GetErrorsDelprefix("value"));
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
        [Route("api/Groups/Pages")]
        public IHttpActionResult GetGroupMaxPages([FromUri]GroupApiParameterModel param)
        {
            try
            {
                int maxcount = 0;
                int maxpages = 0;
                IQueryable<GroupModel> query;
                query = dbContext.GroupModels.OrderBy(x => x.Id);
                if (!param.Deleted)
                    query = query.Where(x => x.Deleted == false);
                if (param.MakerId.HasValue)
                    query = query.Where(x => x.MakerModelId == (int)param.MakerId);
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

        #region グループ商品情報
        [HttpGet]
        [Route("api/Groups/{id}/Products")]
        public IHttpActionResult GetProductList(int id, [FromUri]BaseApiParameterModel param)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(Messages.ApiIllegalParameter);
                }

                if (dbContext.GroupModels.Where(g => g.Id == id).SingleOrDefault() == null) {
                    return BadRequest(Messages.ApiIllegalParameter);
                }

                IList<ProductApiModel> groupList = dbContext.GroupProductModels
                    .Where(um => um.GroupModelId == id).Where(um => um.Deleted == false)
                    .Select(um => um.ProductModel).OrderBy(m => m.Id).ProjectTo<ProductApiModel>().ToList();
                return Ok(groupList);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("api/Groups/{id}/Products")]
        [ValidationRequired(prefix = "ProductList")]
        public IHttpActionResult SetProductList(int id, [FromBody]List<ProductApiModel> ProductList)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(Messages.ApiIllegalParameter);
                }

                GroupModel group = dbContext.GroupModels.Where(g => g.Id == id).SingleOrDefault();
                if (group == null)
                {
                    return BadRequest(Messages.ApiIllegalParameter);
                }

                dbContext.Database.ExecuteSqlCommand(ContextResources.IncrementResetGroupProduct);

                using (DbContextTransaction tx =
                    dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {

                    List<int> setProductList = ProductList.Select(m => m.Id).OrderBy(x => x).ToList();
                    IQueryable<GroupProductModel> groupProducts = dbContext.GroupProductModels
                        .Where(x => x.GroupModelId == id).OrderBy(x => x.ProductModelId);
                    List<int> productIdList = dbContext.ProductModels.Where(x => x.MakerModelId == group.MakerModelId)
                        .Select(x => x.Id).OrderBy(x => x).ToList<int>();

                    foreach (int item in productIdList)
                    {
                        GroupProductModel check = groupProducts.Where(x => x.ProductModelId == item).FirstOrDefault();
                        if (check == null && setProductList.Contains(item))
                        {
                            check = new GroupProductModel();
                            check.GroupModelId = id;
                            check.ProductModelId = item;
                            dbContext.GroupProductModels.Add(check);
                        }
                        else if (check != null && setProductList.Contains(item) && check.Deleted == true)
                        {
                            check.Deleted = false;
                            dbContext.Entry(check).State = EntityState.Modified;
                        }
                        else if (check != null && !setProductList.Contains(item) && check.Deleted == false)
                        {
                            check.Deleted = true;
                            dbContext.Entry(check).State = EntityState.Modified;
                        }
                    }
                    dbContext.SaveChanges();
                    tx.Commit();
                }
                dbContext.Database.ExecuteSqlCommand(ContextResources.IncrementResetGroupProduct);

                return Ok(ProductList);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        #endregion
    }
}
