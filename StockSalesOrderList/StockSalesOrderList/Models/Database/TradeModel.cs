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
    /// 貿易情報モデル
    /// </summary>
    [Table("trades")]
    public class TradeModel : BaseModel
    {
        /// <summary>
        /// 貿易ＩＤ
        /// </summary>
        [Key, Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// 商品ＩＤ
        /// </summary>
        [DisplayName("商品ＩＤ"), Column("product_id")]
        [Required]
        [Index("ui_product_id_detail_date", IsUnique = true, Order = 0)]
        public int ProductModelId { get; set; }

        /// <summary>
        /// 対象年月
        /// </summary>
        [DisplayName("対象年月"), Column("detail_date", TypeName = "Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [Required]
        [Index("ui_product_id_detail_date", IsUnique = true, Order = 1)]
        public DateTime TargetDate { get; set; }

        /// <summary>
        /// 発注予定
        /// </summary>
        [DisplayName("発注予定"), Column("orders_plan_qty")]
        [DecimalPrecision(12, 3), DefaultValue(0), DefaultSqlValue(0)]
        public decimal OrderPlan { get; set; }

        /// <summary>
        /// 発注実績
        /// </summary>
        [DisplayName("発注実績"), Column("orders_qty")]
        [DecimalPrecision(12, 3), DefaultValue(0), DefaultSqlValue(0)]
        public decimal Order { get; set; }

        /// <summary>
        /// 入荷予定
        /// </summary>
        [DisplayName("入荷予定"), Column("invoice_plan_qty")]
        [DecimalPrecision(12, 3), DefaultValue(0), DefaultSqlValue(0)]
        public decimal InvoicePlan { get; set; }

        /// <summary>
        /// 入荷実績
        /// </summary>
        [DisplayName("入荷実績"), Column("invoice_qty")]
        [DecimalPrecision(12, 3), DefaultValue(0), DefaultSqlValue(0)]
        public decimal Invoice { get; set; }

        /// <summary>
        /// 入荷残数
        /// </summary>
        [DisplayName("入荷残数"), Column("remaining_invoice_qty")]
        [DecimalPrecision(12, 3), DefaultValue(0), DefaultSqlValue(0)]
        public decimal RemainingInvoice { get; set; }

        /// <summary>
        /// 入荷残数調整 ADJUSTMENT
        /// </summary>
        [DisplayName("入荷残数調整"), Column("adjustment_invoice_qty")]
        [DecimalPrecision(12, 3), DefaultValue(0), DefaultSqlValue(0)]
        public decimal AdjustmentInvoice { get; set; }

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
        #endregion
    }
}
