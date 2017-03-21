using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockSalesOrderList.Helpers
{
    public sealed class ConvertEncode
    {
        private ConvertEncode()
        {

        }

        public static string ConvertString(string src, System.Text.Encoding srcEnc, System.Text.Encoding descEnc)
        {
            try
            {
                byte[] _src = srcEnc.GetBytes(src);
                byte[] _desc = System.Text.Encoding.Convert(srcEnc, descEnc, _src);
                return descEnc.GetString(_desc);
            }
            catch (Exception)
            {
                return string.Empty;
            }

        }

        public static string ConvertUnicodeToUtf8(string src)
        {
            return ConvertString(src, System.Text.Encoding.Unicode, System.Text.Encoding.UTF8);
        }
    }
}
