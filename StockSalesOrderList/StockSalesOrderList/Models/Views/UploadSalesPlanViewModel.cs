using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockSalesOrderList.Models
{
    public class UploadSalesPlanViewModel : BaseModel
    {
        public HttpPostedFileBase file { get; set; }
    }
}
