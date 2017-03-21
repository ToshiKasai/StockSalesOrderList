using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StockSalesOrderList.Models
{
    public class ResetViewModel : BaseModel
    {
        [DisplayName("新しいパスワード")]
        [DataType(DataType.Password)]
        [Required, MaxLength(256)]
        public string NewPassword { get; set; }

        [DisplayName("確認用パスワード")]
        [DataType(DataType.Password)]
        [Required, MaxLength(256), Compare("NewPassword")]
        public string ConfirmPassword { get; set; }

        public int Key { get; set; }
        public string Token { get; set; }
    }
}
