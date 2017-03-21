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
    /// サインインログ情報モデル
    /// </summary>
    [Table("signin_logs")]
    public class SignInLogModel
    {
        public SignInLogModel()
        {
            this.ProcessingDate = DateTime.Now;
        }

        [Key, Column("id")]
        public int Id { get; set; }

        [DisplayName("処理日"), Column("processing_date")]
        [DataType(DataType.DateTime)]
        public DateTime ProcessingDate { get; set; }

        [DisplayName("クライアントＩＰ"), Column("client_ip")]
        [MaxLength(50)]
        public string ClientIp { get; set; }

        [DisplayName("ユーザーコード"), Column("user_code")]
        [MaxLength(256)]
        [Index("idx_user_code")]
        public string UserCode { get; set; }

        [DisplayName("処理ステータス"), Column("status")]
        public int Status { get; set; }

        [DisplayName("メッセージ"), Column("message")]
        public string Message { get; set; }

        [DisplayName("ユーザーＩＤ"), Column("user_id")]
        [Index("idx_user_id")]
        public int? UserModelId { get; set; }

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
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DefaultSqlValue("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP")]
        public DateTime? ModifiedDateTime { get; set; }
        #endregion

        #region サポート機能
        public void SetStatus(int status)
        {
            this.Status = status;
            try
            {
                // this.Message = HttpContext.GetGlobalResourceObject("AuthResources", "LogMessage" + status.ToString().Replace('-', '_')) as string;
                this.Message = App_GlobalResources.AuthResources.ResourceManager.GetString("LogMessage" + status.ToString().Replace('-', '_'));
            }
            catch (Exception)
            {
                this.Message = "未定義のエラー";
            }
        }
        #endregion

        #region データ連携
        [JsonIgnore]
        [ForeignKey("UserModelId")]
        public virtual UserModel UserModel { get; set; }
        #endregion
    }
}
