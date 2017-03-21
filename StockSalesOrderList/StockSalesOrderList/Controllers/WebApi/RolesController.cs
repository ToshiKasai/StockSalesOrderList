using AutoMapper;
using AutoMapper.QueryableExtensions;
using StockSalesOrderList.App_GlobalResources;
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
    /// ロール情報管理用WEBAPI
    /// </summary>
    [Authorize]
    public class RolesController : BaseApiController
    {
        [HttpGet]
        public IHttpActionResult Get([FromUri]BaseApiParameterModel param)
        {
            try
            {
                List<RoleApiModel> result;
                IQueryable<RoleModel> query;
                query = dbContext.RoleModels.OrderBy(r => r.Id);
                if (!User.IsInRole("admin"))
                    query = query.Where(r => r.Name != "admin");
                if (!param.Deleted)
                    query = query.Where(r => r.Deleted == false);

                result = query.ProjectTo<RoleApiModel>().ToList();
                return Ok(result);
            }
            catch(Exception ex)
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

                RoleModel model;
                model = dbContext.RoleModels.Where(x => x.Id == id).SingleOrDefault();
                if (model == null)
                    return NotFound();

                RoleApiModel result = Mapper.Map<RoleApiModel>(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody]RoleApiModel value)
        {
            return StatusCode(HttpStatusCode.MethodNotAllowed);
        }

        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody]RoleApiModel value)
        {
            return StatusCode(HttpStatusCode.MethodNotAllowed);
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            return StatusCode(HttpStatusCode.MethodNotAllowed);
        }
    }
}
