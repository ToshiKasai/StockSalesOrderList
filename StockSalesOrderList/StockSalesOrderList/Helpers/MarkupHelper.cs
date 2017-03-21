using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMarkupMin.AspNet.Common;
using WebMarkupMin.AspNet.Common.Compressors;
using WebMarkupMin.AspNet4.Common;
using WebMarkupMin.Core;

namespace StockSalesOrderList
{
    public class MarkupHelper
    {
        public static void Configure(WebMarkupMinConfiguration configuration)
        {
            configuration.AllowMinificationInDebugMode = true;
            configuration.AllowCompressionInDebugMode = true;

            IHtmlMinificationManager htmlMinificationManager = HtmlMinificationManager.Current;
            HtmlMinificationSettings htmlMinificationSettings = htmlMinificationManager.MinificationSettings;
            htmlMinificationSettings.RemoveRedundantAttributes = true;
            htmlMinificationSettings.RemoveHttpProtocolFromAttributes = true;
            htmlMinificationSettings.RemoveHttpsProtocolFromAttributes = true;

            IHttpCompressionManager httpCompressionManager = HttpCompressionManager.Current;
            httpCompressionManager.CompressorFactories = new List<ICompressorFactory>
            {
                new GZipCompressorFactory(),
                new DeflateCompressorFactory()
            };
        }
    }
}
