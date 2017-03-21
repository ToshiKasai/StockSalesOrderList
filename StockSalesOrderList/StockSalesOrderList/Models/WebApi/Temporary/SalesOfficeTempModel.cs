using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockSalesOrderList.Models.WebApi
{
    public class SalesOfficeTempModel : BaseModel
    {
        public int product_id { get; set; }
        public DateTime detail_date { get; set; }
        public int office_id { get; set; }
        public string office_name { get; set; }
        public decimal pre_sales_plan { get; set; }
        public decimal pre_sales_actual { get; set; }
        public decimal sales_plan { get; set; }
        public decimal sales_actual { get; set; }
    }
}
