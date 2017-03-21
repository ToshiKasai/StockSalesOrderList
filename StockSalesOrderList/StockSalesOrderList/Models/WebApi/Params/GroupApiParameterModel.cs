using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockSalesOrderList.Models.WebApi
{
    public class GroupApiParameterModel : BaseApiParameterModel
    {
        public int? MakerId { get; set; }
    }
}
