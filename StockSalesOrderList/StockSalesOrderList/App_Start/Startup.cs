using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using StockSalesOrderList.Models;
using StockSalesOrderList.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: OwinStartupAttribute(typeof(StockSalesOrderList.Startup))]
namespace StockSalesOrderList
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext<UserStore>(() => new UserStore(DataContext.CurrentContext));
            app.CreatePerOwinContext<RoleStore>(() => new RoleStore(DataContext.CurrentContext));

            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(((options, context) => new ApplicationSignInManager(context.Get<ApplicationUserManager>(), context.Authentication)));
            app.CreatePerOwinContext<ApplicationRoleManager>(((options, context) => new ApplicationRoleManager(context.Get<RoleStore>())));

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/SignIn"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, UserModel, int>(
                         validateInterval: TimeSpan.FromMinutes(30),
                         regenerateIdentityCallback: (manager, user) => user.GenerateUserIdentityAsync(manager),
                         getUserIdCallback: (id) => (Int32.Parse(id.GetUserId()))),
                    OnApplyRedirect = ctx => {
                        if (!IsApiRequest(ctx.Request))
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }
                    }
                }
            });
        }
        private static bool IsApiRequest(IOwinRequest request)
        {
            string apiPath = VirtualPathUtility.ToAbsolute("~/api/");
            return request.Uri.LocalPath.StartsWith(apiPath);
        }
    }
}
