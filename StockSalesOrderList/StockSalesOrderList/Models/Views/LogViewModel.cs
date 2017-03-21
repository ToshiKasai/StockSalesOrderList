using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StockSalesOrderList.Models
{
    /// <summary>
    /// サインインログ用
    /// </summary>
    public class LogViewModel : BaseModel
    {
        /// <summary>
        /// 検索期間Ｆｒｏｍ
        /// </summary>
        [DisplayName("表示開始日時")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? dateFrom { get; set; }

        /// <summary>
        /// 検索期間Ｔｏ
        /// </summary>
        [DisplayName("表示終了日時")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? dateTo { get; set; }

        /// <summary>
        /// 検索条件
        /// </summary>
        [DisplayName("ユーザーコード")]
        public string SigninCode { get; set; }

        /// <summary>
        /// 表示ログ
        /// </summary>
        public ICollection<SignInLogModel> logs { get; set; }

        /// <summary>
        /// エラーメッセージ
        /// </summary>
        [DisplayName("エラーメッセージ")]
        public string ErrorMessage { get; set; }
    }
}
