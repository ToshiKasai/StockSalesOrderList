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
    /// 特需情報モデル
    /// </summary>
    [Table("sales_trends")]
    public class SalesTrendModel : BaseModel
    {
        /// <summary>
        /// 特需ＩＤ
        /// </summary>
        [Key, Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// 商品ＩＤ
        /// </summary>
        [DisplayName("商品ＩＤ"), Column("product_id")]
        [Required]
        [Index("idx_product_id_detail_date", IsUnique = false, Order = 0)]
        public int ProductModelId { get; set; }

        /// <summary>
        /// 対象年月
        /// </summary>
        [DisplayName("対象年月"), Column("detail_date", TypeName = "Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy/MM}")]
        [Required]
        [Index("idx_product_id_detail_date", IsUnique = false, Order = 1)]
        public DateTime TargetDate { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [DisplayName("数量"), Column("quantity")]
        [DecimalPrecision(12, 3), DefaultValue(0), DefaultSqlValue(0)]
        public decimal Sales { get; set; }

        /// <summary>
        /// コメント
        /// </summary>
        [DisplayName("コメント"), Column("comments")]
        public string Comments { get; set; }

        /// <summary>
        /// ユーザーＩＤ
        /// </summary>
        [DisplayName("ユーザーＩＤ"), Column("user_id")]
        [Required]
        [Index("idx_user_id")]
        public int UserModelId { get; set; }

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

        #region データ連携
        [JsonIgnore]
        [ForeignKey("ProductModelId")]
        public virtual ProductModel ProductModel { get; set; }

        [JsonIgnore]
        [ForeignKey("UserModelId")]
        public virtual UserModel UserModel { get; set; }
        #endregion
    }
}
