using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using StockSalesOrderList.App_GlobalResources;
using StockSalesOrderList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockSalesOrderList.Services
{
    public class BaseService
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private DataContext _db = DataContext.CurrentContext;

        public BaseService()
        {
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
        #endregion

        protected string GetClientIp()
        {
            string result = string.Empty;

            try
            {
                var xForwardedFor = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (String.IsNullOrEmpty(xForwardedFor) == false)
                {
                    result = xForwardedFor.Split(',').GetValue(0).ToString().Trim();
                }
                else
                {
                    result = HttpContext.Current.Request.UserHostAddress;
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

        #region 経過月数・経過年数
        /// <summary>
        /// 経過日数を算出
        /// </summary>
        /// <param name="baseDay">基準日付</param>
        /// <param name="day">対象日付</param>
        /// <returns>経過日数</returns>
        protected int ElapsedDays(DateTime baseDay, DateTime day)
        {
            if (day < baseDay)
                throw new ArgumentException();

            TimeSpan monthSpan = baseDay - day;
            return monthSpan.Days;
        }

        /// <summary>
        /// 単純経過月数を算出
        /// </summary>
        /// <param name="baseDay">基準日付</param>
        /// <param name="day">対象日付</param>
        /// <returns>単純経過月数</returns>
        protected int ElapsedMonths(DateTime baseDay, DateTime day)
        {
            if (day < baseDay)
                throw new ArgumentException();

            return (day.Year - baseDay.Year) * 12 + day.Month - baseDay.Month;
        }

        /// <summary>
        /// 経過月数を算出
        /// </summary>
        /// <param name="baseDay">基準日付</param>
        /// <param name="day">対象日付</param>
        /// <returns>経過満月数</returns>
        protected int ElapsedFullMonths(DateTime baseDay, DateTime day)
        {
            if (day < baseDay)
                throw new ArgumentException();

            var diff = this.ElapsedMonths(baseDay, day);
            if (baseDay.Day <= day.Day)
                return diff;
            else if (day.Day == DateTime.DaysInMonth(day.Year, day.Month) && day.Day <= baseDay.Day)
                return diff;
            else
                return diff - 1;
        }

        /// <summary>
        /// 経過年数を算出
        /// </summary>
        /// <param name="baseDay">基準日付</param>
        /// <param name="day">対象日付</param>
        /// <returns>経過年数</returns>
        protected int ElapsedYears(DateTime baseDay, DateTime day)
        {
            return this.ElapsedFullMonths(baseDay, day) / 12;
        }
        #endregion
    }
}
