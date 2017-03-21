using AutoMapper.QueryableExtensions;
using StockSalesOrderList.Models;
using StockSalesOrderList.Models.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StockSalesOrderList.Controllers.WebApi
{
    /// <summary>
    /// コンテナ情報管理用WEBAPI
    /// </summary>
    [Authorize]
    public class ContainersController : BaseApiController
    {
        [HttpGet]
        public IHttpActionResult Get([FromUri]BaseApiParameterModel param)
        {
            try
            {
                List<ContainerApiModel> result;
                IQueryable<ContainerModel> query;
                query = dbContext.ContainerModels.OrderBy(x => x.Id);
                if (!param.Deleted)
                    query = query.Where(x => x.Deleted == false);

                if (param.Limit.HasValue)
                {
                    if (param.Page.HasValue)
                        query = query.Skip((int)param.Limit * (int)param.Page).Take((int)param.Limit);
                    else
                        query = query.Take((int)param.Limit);
                }
                result = query.ProjectTo<ContainerApiModel>().ToList();

                // if (result == null || result.Count == 0)
                //     return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public IHttpActionResult Get(int id)
        {
            return StatusCode(HttpStatusCode.MethodNotAllowed);
        }

        public IHttpActionResult Post([FromBody]string value)
        {
            return StatusCode(HttpStatusCode.MethodNotAllowed);
        }

        public IHttpActionResult Put(int id, [FromBody]string value)
        {
            return StatusCode(HttpStatusCode.MethodNotAllowed);
        }

        public IHttpActionResult Delete(int id)
        {
            return StatusCode(HttpStatusCode.MethodNotAllowed);
        }
    }
}
