using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StockSalesOrderList.Models.WebApi
{
    public class UserApiModel : BaseModel
    {
        public int Id { get; set; }

        [DisplayName("ログインＩＤ")]
        [Required, MaxLength(256)]
        public string UserName { get; set; }

        [DisplayName("ユーザー名")]
        [MaxLength(256)]
        public string Name { get; set; }

        [DisplayName("有効期限")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? Expiration { get; set; }

        [DisplayName("パスワード変更スキップ回数")]
        public int PasswordSkipCnt { get; set; }

        [DisplayName("メールアドレス")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress, MaxLength(128)]
        public string Email { get; set; }

        [DisplayName("メールアドレス確認済")]
        public bool EmailConfirmed { get; set; }

        [DisplayName("ロックアウト終了日時")]
        public DateTime? LockoutEndData { get; set; }

        [DisplayName("ロックアウト許可")]
        public bool LockoutEnabled { get; set; }

        [DisplayName("アクセス失敗回数")]
        public int AccessFailedCount { get; set; }

        [DisplayName("使用許可")]
        public bool Enabled { get; set; }

        [DisplayName("削除済")]
        public bool Deleted { get; set; }

        public DateTime? NewExpiration { get; set; }
        public string NewPassword { get; set; }
    }
}
