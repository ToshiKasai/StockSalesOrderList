using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using WebMarkupMin.AspNet.Common;

namespace StockSalesOrderList
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            MvcHandler.DisableMvcResponseHeader = true;
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver
                = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;

            BundleSupport.BundleConfig();
            MarkupHelper.Configure(WebMarkupMin.AspNet4.Common.WebMarkupMinConfiguration.Instance);
            DefaultFilterHelper.Register();
            AttributeAdapter.Register();
        }

        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Add("X-Version", "0.0.1");
            Response.Headers.Add("X-Author", "Minoru Takayama");
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_EndRequest(Object sender, EventArgs e)
        {
            if (Models.DataContext.HasContext)
            {
                Models.DataContext.CurrentContext.Dispose();
            }
        }
    }
}