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
    /// 発注情報モデル
    /// </summary>
    [Table("orders")]
    public class OrderModel : BaseModel
    {
        /// <summary>
        /// 発注ＩＤ
        /// </summary>
        [Key, Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// 発注番号
        /// </summary>
        [DisplayName("発注番号"), Column("order_no")]
        [Required, MaxLength(128)]
        public string OrderNo { get; set; }

        /// <summary>
        /// 発注日
        /// </summary>
        [DisplayName("発注日"), Column("order_date", TypeName = "Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// 商品ＩＤ
        /// </summary>
        [DisplayName("商品ＩＤ"), Column("product_id")]
        [Required]
        [Index("idx_product_id")]
        public int ProductModelId { get; set; }

        /// <summary>
        /// 発注数量
        /// </summary>
        [DisplayName("発注数量"), Column("quantity")]
        [DecimalPrecision(12, 3), DefaultValue(0), DefaultSqlValue(0)]
        public decimal Order { get; set; }

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
        public virtual ProductModel ProductModel { get; set; }
        #endregion
    }
}
