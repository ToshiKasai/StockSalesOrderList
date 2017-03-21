using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSalesOrderList.Models
{
    public interface IRowVersion
    {
        int RowVersion { get; set; }
    }
}
