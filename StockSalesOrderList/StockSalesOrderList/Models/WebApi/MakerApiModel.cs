using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StockSalesOrderList.Models.WebApi
{
    public class MakerApiModel : BaseModel
    {
        public int Id { get; set; }

        [DisplayName("メーカーコード")]
        // [Required, MaxLength(128), RegularExpression(@"[A-Za-z0-9]+")]
        [Required, MaxLength(128)]
        public string Code { get; set; }

        [DisplayName("メーカー名")]
        [Required, MaxLength(256)]
        public string Name { get; set; }

        [DisplayName("使用許可")]
        public bool Enabled { get; set; }

        [DisplayName("削除済")]
        public bool Deleted { get; set; }
    }
}
