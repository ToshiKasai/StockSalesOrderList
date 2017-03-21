using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockSalesOrderList.Models.WebApi
{
    public class SalesViewApiModel : BaseModel
    {
        public ProductApiModel Product { get; set; }
        public ICollection<SalesViewsTempModel> SalesList { get; set; }
        public ICollection<ICollection<SalesOfficeTempModel>> OfficeSales { get; set; }
    }
}
