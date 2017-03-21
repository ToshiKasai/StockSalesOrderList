using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace StockSalesOrderList.Controllers
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class SubmitCommandAttribute : ActionMethodSelectorAttribute
    {
        private string submitName;
        private string submitValue;
        private static readonly AcceptVerbsAttribute innerAttribute = new AcceptVerbsAttribute(HttpVerbs.Post);

        public SubmitCommandAttribute(string name) : this(name, string.Empty) { }
        public SubmitCommandAttribute(string name, string value)
        {
            submitName = name;
            submitValue = value;
        }

        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            if (!innerAttribute.IsValidForRequest(controllerContext, methodInfo))
                return false;

            var submitted = controllerContext.RequestContext.HttpContext.Request.Form[submitName];
            return string.IsNullOrEmpty(submitValue) ? !string.IsNullOrEmpty(submitted) : string.Equals(submitted, submitValue, StringComparison.InvariantCultureIgnoreCase);
        }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class NoCacheNoStoreMvcAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var response = filterContext.HttpContext.Response;
            response.Cache.SetNoStore();
            response.Cache.SetCacheability(HttpCacheability.Private | HttpCacheability.NoCache);
            response.Cache.SetMaxAge(TimeSpan.Parse("0"));
            response.Cache.SetExpires(DateTime.UtcNow);
            base.OnActionExecuting(filterContext);
        }
    }

    public class MyRequiredHttpsAttribute : RequireHttpsAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");

            if (filterContext.HttpContext != null && filterContext.HttpContext.Request.IsLocal)
                return;

            base.OnAuthorization(filterContext);
        }
    }
}
