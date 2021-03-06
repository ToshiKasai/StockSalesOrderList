﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StockSalesOrderList.Models
{
    public class RequestMailViewModel : BaseModel
    {
        [DisplayName("サインインＩＤ")]
        [MaxLength(128)]
        public string SinginId { get; set; }

        [DisplayName("メールアドレス")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress, MaxLength(128)]
        public string Email { get; set; }
    }
}
