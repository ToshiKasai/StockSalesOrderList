using StockSalesOrderList.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMarkupMin.AspNet4.Mvc;

namespace StockSalesOrderList
{
    public class DefaultFilterHelper
    {
        public static void Register()
        {
            RegisterMvcFilters();
            RegisterWebApiFilters();
        }

        public static void RegisterMvcFilters()
        {
            RegisterMvcFilters(System.Web.Mvc.GlobalFilters.Filters);
        }

        public static void RegisterMvcFilters(System.Web.Mvc.GlobalFilterCollection filters)
        {
            filters.Add(new MinifyHtmlAttribute());
            filters.Add(new NoCacheNoStoreMvcAttribute());
        }

        public static void RegisterWebApiFilters()
        {
            RegisterWebApiFilters(System.Web.Http.GlobalConfiguration.Configuration.Filters);
        }

        public static void RegisterWebApiFilters(System.Web.Http.Filters.HttpFilterCollection filters)
        {
            filters.Add(new NoCacheNoStoreApiAttribute());
            filters.Add(new EnableTagAttribute());
        }
    }
}
