using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StockSalesOrderList.Models
{
    /// <summary>
    /// アプリケーションログ情報モデル
    /// </summary>
    [Table("application_logs")]
    public class ApplicationLogModel
    {
        [Key, Column("id")]
        public int Id { get; set; }

        [DisplayName("処理日"), Column("processing_date")]
        [DataType(DataType.DateTime)]
        public DateTime ProcessingDate { get; set; }

        [DisplayName("処理名"), Column("process_name")]
        [MaxLength(256)]
        public string ProcessName { get; set; }

        [DisplayName("ユーザーＩＤ"), Column("user_id")]
        [Index("idx_user_id")]
        public int UserModelId { get; set; }

        [DisplayName("処理内容"), Column("message")]
        public string Message { get; set; }

        #region 定型管理項目
        /// <summary>
        /// 削除情報
        /// </summary>
        [DisplayName("削除済"), Column("deleted")]
        [DefaultValue(false), DefaultSqlValue(false)]
        public bool Deleted { get; set; }

        /// <summary>
        /// 作成日時
        /// </summary>
        [DisplayName("作成日時"), Column("created_at")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DefaultSqlValue("CURRENT_TIMESTAMP")]
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// 変更日時
        /// </summary>
        [DisplayName("更新日時"), Column("modified_at")]
        [ConcurrencyCheck]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DefaultSqlValue("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP")]
        public DateTime? ModifiedDateTime { get; set; }
        #endregion

        #region データ連携
        [JsonIgnore]
        [ForeignKey("UserModelId")]
        public virtual UserModel UserModel { get; set; }
        #endregion
    }
}
