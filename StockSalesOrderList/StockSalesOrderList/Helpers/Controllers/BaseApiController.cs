using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using StockSalesOrderList.App_GlobalResources;
using StockSalesOrderList.Helpers;
using StockSalesOrderList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace StockSalesOrderList.Controllers.WebApi
{
    public class BaseApiController : ApiController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private DataContext _db = DataContext.CurrentContext;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BaseApiController()
        {
            AutoMapperSupport.Configure();
        }

        /// <summary>
        /// DataContext
        /// </summary>
        protected DataContext dbContext
        {
            get
            {
                return _db;
            }
        }

#region 認証機能
        protected int GetUserId()
        {
            return AuthenticationManager.User.Identity.GetUserId<int>();
        }

        protected string GetSigninId()
        {
            return AuthenticationManager.User.Identity.GetUserName();
        }

        protected IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }

        protected ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
            set
            {
                _signInManager = value;
            }
        }

        protected ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }

        protected ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
            set
            {
                _roleManager = value;
            }
        }
#endregion

        protected void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                foreach (var err in error.Split('。'))
                {
                    if (!string.IsNullOrWhiteSpace(err))
                        ModelState.AddModelError("", err.Trim(' ') + "。");
                }
            }
        }

        protected string GetClientIp()
        {
            try
            {
                IEnumerable<string> headerValues;
                string result = string.Empty;
                if (ControllerContext.Request.Headers.TryGetValues("X-Forwarded-For", out headerValues) == true)
                {
                    var xForwardedFor = headerValues.FirstOrDefault();
                    result = xForwardedFor.Split(',').GetValue(0).ToString().Trim();
                }
                else
                {
                    if (ControllerContext.Request.Properties.ContainsKey("MS_HttpContext"))
                    {
                        result = ((HttpContextWrapper)ControllerContext.Request.Properties["MS_HttpContext"]).Request.UserHostAddress;
                    }
                }

                if (result != "::1"/*localhost*/)
                {
                    result = result.Split(':').GetValue(0).ToString().Trim();
                }
                return result;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        #region アプリケーションログ
        protected void WriteAppLog(string process, string message)
        {
            try
            {
                dbContext.Database.ExecuteSqlCommand(
                    ContextResources.WriteLogSql,
                    DateTime.Now, process, GetUserId(), message
                );
            }
            catch (Exception)
            {
            }
        }
        #endregion

    }
}
