using Microsoft.AspNet.Identity;
using StockSalesOrderList.Models.WebApi;
using StockSalesOrderList.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace StockSalesOrderList.Controllers.WebApi
{
    /// <summary>
    /// セッションと認証情報管理WEBAPI
    /// </summary>
    [Authorize]
    public class SessionsController : BaseApiController
    {
        [HttpGet]
        [Route("api/Holiday")]
        public IHttpActionResult GetHoliday([FromUri]BaseApiParameterModel param)
        {
            int year = DateTime.Now.Year;
            if (param.Page.HasValue)
            {
                year = (int)param.Page;
            }
            HolidayService service = new HolidayService();
            return Ok(service.getHoliday(year));
        }

        [HttpGet]
        [Route("api/Accounts")]
        public IHttpActionResult GetLoginUser([FromUri]BaseApiParameterModel param)
        {
            string id = string.Empty;
            string name = string.Empty;
            try
            {
                id = User.Identity.GetUserId();
                name = ((ClaimsIdentity)User.Identity).FindFirst(Models.UserModel.ApplicationClaimTypes.ClaimUserName).Value;
            }
            catch (Exception)
            {
            }
            WriteAppLog("Accounts", "取得");
            return Ok(new []{ new { Id = id, Name = name } });
        }

        #region ユーザー権限
        [HttpGet]
        [Route("api/Accounts/Roles")]
        public IEnumerable<string> GetRoleList([FromUri]BaseApiParameterModel param)
        {
            WriteAppLog("Accounts/Roles", "取得");
            return UserManager.GetRoles(GetUserId());
        }

        [HttpGet]
        [Route("api/Accounts/Roles/{role}")]
        public bool GetRole(string role, [FromUri]BaseApiParameterModel param)
        {
            WriteAppLog("Accounts/Roles/" + role, "取得");
            return User.IsInRole(role);
        }
        #endregion


        #region 通常は不許可
        [HttpGet]
        public IHttpActionResult Get()
        {
            return StatusCode(HttpStatusCode.MethodNotAllowed);
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            return StatusCode(HttpStatusCode.MethodNotAllowed);
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody]string value)
        {
            return StatusCode(HttpStatusCode.MethodNotAllowed);
        }

        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody]string value)
        {
            return StatusCode(HttpStatusCode.MethodNotAllowed);
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            return StatusCode(HttpStatusCode.MethodNotAllowed);
        }
        #endregion
    }
}
