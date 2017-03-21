using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StockSalesOrderList.Models
{
    public class ChangePasswordViewModel : BaseModel
    {
        [DisplayName("現在のパスワード")]
        [DataType(DataType.Password)]
        [Required, MaxLength(256)]
        public string CurrentPassword { get; set; }

        [DisplayName("新しいパスワード")]
        [DataType(DataType.Password)]
        [Required, MaxLength(256)]
        public string NewPassword { get; set; }

        [DisplayName("確認用パスワード")]
        [DataType(DataType.Password)]
        [Required, MaxLength(256), Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
