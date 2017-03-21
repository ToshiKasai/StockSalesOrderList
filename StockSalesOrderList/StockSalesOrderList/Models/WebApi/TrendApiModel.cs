
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockSalesOrderList.Models.WebApi
{
    public class TrendApiModel : BaseModel
    {
        public int Id { get; set; }
        public int Product_id { get; set; }
        public DateTime Detail_date { get; set; }
        public decimal Quantity { get; set; }
        public string Comments { get; set; }
        public int User_id { get; set; }
        public string User_name { get; set; }
    }
}
