using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StockSalesOrderList.Models.WebApi
{
    public class ContainerApiModel : BaseModel
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("コンテナ名")]
        [Required, MaxLength(256)]
        public string Name { get; set; }

        [DisplayName("削除済")]
        public bool Deleted { get; set; }
    }
}
