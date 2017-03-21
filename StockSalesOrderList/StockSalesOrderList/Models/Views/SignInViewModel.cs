using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StockSalesOrderList.Models
{
    public class SignInViewModel : BaseModel
    {
        [DisplayName("サインインＩＤ")]
        [Required, MaxLength(128)]
        public string SinginId { get; set; }

        [DisplayName("パスワード")]
        [DataType(DataType.Password)]
        [Required, MaxLength(256)]
        public string Password { get; set; }
    }
}
