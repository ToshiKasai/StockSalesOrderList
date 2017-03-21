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
    /// メーカー情報管理用WEBAPI
    /// </summary>
    [Authorize]
    public class MakersController : BaseApiController
    {
        #region メーカー情報
        [HttpGet]
        public IHttpActionResult Get([FromUri]BaseApiParameterModel param)
        {
            try
            {
                List<MakerApiModel> result;
                IQueryable<MakerModel> query;
                query = dbContext.MakerModels.OrderBy(x => x.Id);
                if (!param.Deleted)
                    query = query.Where(x => x.Deleted == false);
                if (param.Enabled)
                    query = query.Where(x => x.Enabled == true);

                if (param.Limit.HasValue)
                {
                    if (param.Page.HasValue)
                        query = query.Skip((int)param.Limit * (int)param.Page).Take((int)param.Limit);
                    else
                        query = query.Take((int)param.Limit);
                }
                result = query.ProjectTo<MakerApiModel>().ToList();

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

                MakerApiModel result = dbContext.MakerModels.Where(x => x.Id == id)
                    .ProjectTo<MakerApiModel>().SingleOrDefault();
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
        public IHttpActionResult Post([FromBody]MakerApiModel value)
        {
            return StatusCode(HttpStatusCode.MethodNotAllowed);
        }

        [HttpPut]
        [ValidationRequired(prefix = "value")]
        public IHttpActionResult Put(int id, [FromBody]MakerApiModel value)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(Messages.ApiIllegalParameter);

                using (DbContextTransaction tx =
                    dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    value.Id = id;
                    MakerModel maker = dbContext.MakerModels.Where(m => m.Id == value.Id).SingleOrDefault();
                    if (maker == null)
                    {
                        tx.Rollback();
                        return NotFound();
                    }

                    maker.Enabled = value.Enabled;
                    dbContext.Entry(maker).State = EntityState.Modified;
                    if (dbContext.SaveChanges() == 0) {
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
        [Route("api/Makers/Pages")]
        public IHttpActionResult GetGroupMaxPages([FromUri]BaseApiParameterModel param)
        {
            try
            {
                int maxcount = 0;
                int maxpages = 0;
                IQueryable<MakerModel> query;
                query = dbContext.MakerModels.OrderBy(x => x.Id);
                if (!param.Deleted)
                    query = query.Where(x => x.Deleted == false);
                if (param.Enabled)
                    query = query.Where(x => x.Enabled == true);
                maxcount = query.Count();

                if (param.Limit.HasValue)
                {
                    maxpages = (int)(maxcount / (int)param.Limit);
                    if((int)(maxcount % (int)param.Limit) > 0)
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

        #region グループ情報
        [HttpGet]
        [Route("api/Makers/{id}/Groups")]
        public IHttpActionResult GetGroupList(int id, [FromUri]BaseApiParameterModel param)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(Messages.ApiIllegalParameter);

                List<GroupApiModel> result;
                IQueryable<GroupModel> query;
                query = dbContext.GroupModels.Where(g => g.MakerModelId == id);
                if (!param.Deleted)
                    query = query.Where(g => g.Deleted == false);
                result = query.OrderBy(g => g.Id).ProjectTo<GroupApiModel>().ToList();

                //if (result == null || result.Count == 0)
                //    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        #endregion
    }
}
