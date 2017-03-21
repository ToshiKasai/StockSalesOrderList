using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;

namespace StockSalesOrderList.Helpers
{
    public static class MarkupSupport
    {
        public static string GetLoginName(this HtmlHelper htmlHelper)
        {
            try
            {
                string result = string.Empty;
                if (htmlHelper.ViewContext.HttpContext.Request.IsAuthenticated)
                {
                    ClaimsIdentity userIdentity = (ClaimsIdentity)htmlHelper.ViewContext.HttpContext.User.Identity;
                    result = userIdentity.FindFirst(Models.UserModel.ApplicationClaimTypes.ClaimUserName).Value;
                }
                return htmlHelper.ViewContext.HttpContext.User.Identity.GetUserName() + " / " + result;
            }
            catch (Exception)
            {
                return htmlHelper.ViewContext.HttpContext.User.Identity.GetUserName();
            }
        }

        public static string GetModelPropertyName<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var resolvedLabelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            return resolvedLabelText;
        }
        public static string GetModelPropertyId<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var resolvedLabelText = metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            return resolvedLabelText;
        }

        public static bool HasValidationError(this HtmlHelper htmlHelper, bool exceptProperty = false)
        {
            bool result = false;
            if (!exceptProperty)
            {
                return htmlHelper.ViewData.ModelState.Keys.Count != 0;
            }
            foreach (var key in htmlHelper.ViewData.ModelState.Keys)
            {
                if (key == string.Empty)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public static bool IsValid<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            var fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            ModelState modelState;

            return !htmlHelper.ViewData.ModelState.TryGetValue(fullName, out modelState) || modelState.Errors.Count <= 0;
        }

        public static string ValidateLabelClass<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string errorClass = "has-error")
        {
            if (IsValid(htmlHelper, expression))
                return "form-group label-floating";
            else
                return "form-group label-floating " + errorClass;
        }

        public static MvcHtmlString CustomValidationSummary(this HtmlHelper htmlHelper, bool exceptProperty = false)
        {
            string result = string.Empty;
            foreach (var key in htmlHelper.ViewData.ModelState.Keys)
            {
                if (exceptProperty == false || (exceptProperty == true && key == string.Empty))
                {
                    foreach (var e in htmlHelper.ViewData.ModelState[key].Errors)
                    {
                        if (result.Length > 0)
                        {
                            result += "<br />";
                        }
                        result = result + "<span>" + e.ErrorMessage + "</span>";
                    }
                }
            }
            return MvcHtmlString.Create(result);
        }
    }

    public static class ResourceSupport
    {
        public static MvcHtmlString Resource(this HtmlHelper htmlhelper, string expression, string virtualPath = null, params object[] args)
        {
            try
            {
                if (virtualPath == null)
                    virtualPath = GetVirtualPath(htmlhelper);
                return MvcHtmlString.Create(GetResourceString(htmlhelper.ViewContext.HttpContext, expression, virtualPath, args));
            }
            catch (Exception)
            {
                return MvcHtmlString.Create("");
            }
        }

        public static string ResourceRaw(this HtmlHelper htmlhelper, string expression, string virtualPath = null, params object[] args)
        {
            try
            {
                if (virtualPath == null)
                    virtualPath = GetVirtualPath(htmlhelper);
                return GetResourceString(htmlhelper.ViewContext.HttpContext, expression, virtualPath, args);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string Resource(this Controller controller, string expression, params object[] args)
        {
            return GetResourceString(controller.HttpContext, expression, "~/", args);
        }

        private static string _GetResourceString(HttpContextBase httpContext, string expression, string virtualPath, object[] args)
        {
            try
            {
                var context = new ExpressionBuilderContext(virtualPath);
                var builder = new ResourceExpressionBuilder();
                var fields = (ResourceExpressionFields)builder.ParseExpression(expression, typeof(string), context);
                if (!string.IsNullOrEmpty(fields.ClassKey))
                    return string.Format((string)httpContext.GetGlobalResourceObject(fields.ClassKey, fields.ResourceKey, CultureInfo.CurrentUICulture), args);
                return string.Format((string)httpContext.GetLocalResourceObject(virtualPath, fields.ResourceKey, CultureInfo.CurrentUICulture), args);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private static string GetResourceString(HttpContextBase httpContext, string expression, string virtualPath, object[] args)
        {
            try
            {
                string result = _GetResourceString(httpContext, expression, virtualPath, args);
                if (string.IsNullOrEmpty(result) == false)
                    return result;

                string[] param = expression.Split(',');
                param[0] = param[0].Trim();
                param[1] = param[1].Trim();

                if(param[0]== "AuthResources")
                   return App_GlobalResources.AuthResources.ResourceManager.GetString(param[1]);
                if (param[0] == "ContextResources")
                    return App_GlobalResources.ContextResources.ResourceManager.GetString(param[1]);
                if (param[0] == "Messages")
                    return App_GlobalResources.Messages.ResourceManager.GetString(param[1]);
                return " ";
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private static string GetVirtualPath(HtmlHelper htmlhelper)
        {
            try
            {
                var view = htmlhelper.ViewContext.View as BuildManagerCompiledView;
                if (view != null)
                    return view.ViewPath;
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
