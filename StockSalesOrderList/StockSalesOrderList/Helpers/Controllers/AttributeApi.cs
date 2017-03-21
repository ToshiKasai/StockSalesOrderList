using StockSalesOrderList.App_GlobalResources;
using StockSalesOrderList.Helpers;
using StockSalesOrderList.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace StockSalesOrderList.Controllers
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class NoCacheNoStoreApiAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
            if (actionExecutedContext.Response != null)
            {
                if (actionExecutedContext.Response.Headers.CacheControl == null)
                    actionExecutedContext.Response.Headers.CacheControl = new CacheControlHeaderValue();
                actionExecutedContext.Response.Headers.CacheControl.NoCache = true;
                actionExecutedContext.Response.Headers.CacheControl.NoStore = true;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class ValidationRequiredAttribute : ActionFilterAttribute
    {
        public bool IsNullable { get; set; }
        public string prefix { get; set; }

        public ValidationRequiredAttribute()
        {
            IsNullable = false;
            prefix = "";
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!IsNullable && actionContext.ActionArguments.Any(x => x.Value == null))
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest, Messages.ApiNoParameter);
            }
            else if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest, actionContext.ModelState.GetErrorsDelprefix(prefix));
            }

            base.OnActionExecuting(actionContext);
        }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class EnableTagAttribute : ActionFilterAttribute
    {
        private static ConcurrentDictionary<string, EntityTagHeaderValue> etags =
            new ConcurrentDictionary<string, EntityTagHeaderValue>();

        public override void OnActionExecuting(HttpActionContext context)
        {
            if (context != null)
            {
                HttpRequestMessage request = context.Request;
                if (request.Method == HttpMethod.Get)
                {
                    if (IfNoneMatchContainsStoredEtagValue(request))
                    {
                        context.Response = new HttpResponseMessage(HttpStatusCode.NotModified);
                        SetCacheControl(context.Response);
                    }
                }
                if (request.Method == HttpMethod.Put)
                {
                    if (!IfMatchContainsStoredEtagValue(request))
                    {
                        context.Response = new HttpResponseMessage(HttpStatusCode.PreconditionFailed);
                        SetCacheControl(context.Response);
                    }
                }
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext context)
        {
            HttpRequestMessage request = context.Request;
            string key = GetKey(request);

            EntityTagHeaderValue etag;
            if (context.Response != null)
            {
                if (context.Response.StatusCode == HttpStatusCode.OK || context.Response.StatusCode == HttpStatusCode.NoContent)
                {
                    if (!etags.TryGetValue(key, out etag) || request.Method == HttpMethod.Put || request.Method == HttpMethod.Post)
                    {
                        // etag = new EntityTagHeaderValue("\"" + Guid.NewGuid().ToString() + "\"");
                        etag = new EntityTagHeaderValue("\"" + ShortGuid.NewGuid().ToString() + "\"");
                        etags.AddOrUpdate(key, etag, (k, val) => etag);
                    }
                    context.Response.Headers.ETag = etag;
                }
                SetCacheControl(context.Response);
            }
        }

        private static void SetCacheControl(HttpResponseMessage response)
        {
            response.Headers.CacheControl = new CacheControlHeaderValue()
            {
                MaxAge = TimeSpan.FromSeconds(60),
                MustRevalidate = true,
                Private = true
            };
        }

        private static string GetKey(HttpRequestMessage request)
        {
            return request.RequestUri.ToString();
        }

        private bool IfNoneMatchContainsStoredEtagValue(HttpRequestMessage request)
        {
            if (request.Headers.IfNoneMatch.Count == 0)
                return false;

            EntityTagHeaderValue etag;
            if (etags.TryGetValue(GetKey(request), out etag))
                return request.Headers.IfNoneMatch.Select(v => v.Tag).Contains(etag.Tag);

            return false;
        }

        private bool IfMatchContainsStoredEtagValue(HttpRequestMessage request)
        {
            if (request.Headers.IfMatch.Count == 0)
                return false;

            EntityTagHeaderValue etag;
            if (etags.TryGetValue(GetKey(request), out etag))
                return request.Headers.IfMatch.Select(v => v.Tag).Contains(etag.Tag);

            return false;
        }
    }
}
