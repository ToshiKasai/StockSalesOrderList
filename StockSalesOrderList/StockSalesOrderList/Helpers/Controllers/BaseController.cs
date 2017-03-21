using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using StockSalesOrderList.App_GlobalResources;
using StockSalesOrderList.Helpers;
using StockSalesOrderList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace StockSalesOrderList.Controllers
{
    public class BaseController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private DataContext _db = DataContext.CurrentContext;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BaseController()
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
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        protected ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
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
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
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

        protected ActionResult RedirectToHome()
        {
            return RedirectToAction("Index", "Home");
        }

        protected string GetClientIp()
        {
            string result = string.Empty;

            try
            {
                var xForwardedFor = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (String.IsNullOrEmpty(xForwardedFor) == false)
                {
                    result = xForwardedFor.Split(',').GetValue(0).ToString().Trim();
                }
                else
                {
                    result = Request.UserHostAddress;
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

        #region コンテンツ付ステータスコード
        protected ActionResult HttpStatucCodeWithContent(string content, int statuscode, string statusdescription = null)
        {
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = statuscode;
            if(statusdescription!=null)
                Response.StatusDescription = Url.Encode(statusdescription);
            return Content(content);
        }
        protected ActionResult HttpStatucCodeWithContent(string content, HttpStatusCode statuscode, string statusdescription = null)
        {
            return HttpStatucCodeWithContent(content, (int)statuscode, statusdescription);
        }
        #endregion

        #region アプリケーションログ
        protected void WriteAppLog(string process, string message)
        {
            try
            {
                dbContext.Database.ExecuteSqlCommand(
                    ContextResources.WriteLogSql,
                    DateTime.Now,process,GetUserId(),message
                );
            }
            catch (Exception)
            {
            }
        }
        #endregion
    }
}
