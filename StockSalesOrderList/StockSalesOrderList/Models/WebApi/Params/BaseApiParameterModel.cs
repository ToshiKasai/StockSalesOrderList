using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace StockSalesOrderList.Models.WebApi
{
    public class BaseApiParameterModel : BaseModel
    {
        public int? Limit { get; set; }

        public int? Page { get; set; }

        [DefaultValue(false)]
        public bool Deleted { get; set; }

        [DefaultValue(true)]
        public bool Enabled { get; set; }
    }
}
