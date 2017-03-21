using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockSalesOrderList.Models.WebApi
{
    public class SalesViewApiParameterModel : BaseApiParameterModel
    {
        public int? GroupId { get; set; }
        public int? MakerId { get; set; }
        public int? Year { get; set; }
    }
}
