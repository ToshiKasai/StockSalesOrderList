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
    /// 商品情報モデル
    /// </summary>
    [Table("products")]
    public class ProductModel : BaseModel
    {
        /// <summary>
        /// 商品ＩＤ
        /// </summary>
        [Key, Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// 商品コード
        /// </summary>
        [DisplayName("商品コード"), Column("code")]
        [Required, MaxLength(128), RegularExpression(@"[a-zA-Z0-9]+")]
        [Index("ui_code", IsUnique = true)]
        public string Code { get; set; }

        /// <summary>
        /// 商品名
        /// </summary>
        [DisplayName("商品名"), Column("name")]
        [Required, MaxLength(256)]
        public string Name { get; set; }

        /// <summary>
        /// 入数
        /// </summary>
        [DisplayName("入数"), Column("quantity")]
        [Required, Range(0.0, 100.0)]
        [DecimalPrecision(12, 3), DefaultValue(0), DefaultSqlValue(0)]
        public decimal Quantity { get; set; }

        /// <summary>
        /// メーカー
        /// </summary>
        [DisplayName("メーカー"), Column("maker_id")]
        [Required]
        [Index("idx_maker_id")]
        public int MakerModelId { get; set; }

        /// <summary>
        /// 計量品
        /// </summary>
        [DisplayName("計量品"), Column("is_sold_weight")]
        [Required]
        [DefaultValue(false), DefaultSqlValue(false)]
        public bool IsSoldWeight { get; set; }

        /// <summary>
        /// パレット入数
        /// </summary>
        [DisplayName("パレット入数"), Column("palette_quantity")]
        [DecimalPrecision(12, 3)]
        public decimal? PaletteQuantity { get; set; }

        /// <summary>
        /// カートン入数
        /// </summary>
        [DisplayName("カートン入数"), Column("carton_quantity")]
        [DecimalPrecision(12, 3)]
        public decimal? CartonQuantity { get; set; }

        /// <summary>
        /// ケースの高さ
        /// </summary>
        [DisplayName("ケース高さ"), Column("case_height")]
        [DecimalPrecision(12, 3)]
        public decimal? CaseHeight { get; set; }

        /// <summary>
        /// ケースの幅
        /// </summary>
        [DisplayName("ケース幅"), Column("case_width")]
        [DecimalPrecision(12, 3)]
        public decimal? CaseWidth { get; set; }

        /// <summary>
        /// ケースの奥行き
        /// </summary>
        [DisplayName("ケース奥行き"), Column("case_depth")]
        [DecimalPrecision(12, 3)]
        public decimal? CaseDepth { get; set; }

        /// <summary>
        /// ケースの容量
        /// </summary>
        [DisplayName("ケース容量"), Column("case_capacity")]
        [DecimalPrecision(12, 3)]
        public decimal? CaseCapacity { get; set; }

        /// <summary>
        /// リードタイム（調達時間）
        /// </summary>
        [DisplayName("リードタイム"), Column("lead_time")]
        public int? LeadTime { get; set; }

        /// <summary>
        /// 発注間隔
        /// </summary>
        [DisplayName("発注間隔"), Column("order_interval")]
        public int? OrderInterval { get; set; }

        /// <summary>
        /// 関連商品
        /// </summary>
        [DisplayName("関連商品"), Column("old_product_id")]
        [Index("idx_old_product_id")]
        public int? OldProductModelId { get; set; }

        /// <summary>
        /// 関連商品の計算倍率
        /// </summary>
        [DisplayName("倍率"), Column("magnification")]
        [DecimalPrecision(12, 3)]
        public decimal? Magnification { get; set; }

        /// <summary>
        /// 最低発注数量
        /// </summary>
        [DisplayName("最低発注数量"), Column("minimum_order_quantity")]
        public decimal? MinimumOrderQuantity { get; set; }

        /// <summary>
        /// 使用許可
        /// </summary>
        [DisplayName("使用許可"), Column("enabled")]
        [DefaultValue(true), DefaultSqlValue(true)]
        public bool Enabled { get; set; }

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
        [ForeignKey("MakerModelId")]
        public virtual MakerModel MakerModel { get; set; }
        [JsonIgnore]
        public virtual ICollection<GroupProductModel> GroupProductModels { get; set; }
        [JsonIgnore]
        public virtual ICollection<SalesModel> SalesModels { get; set; }
        [JsonIgnore]
        public virtual ICollection<StockModel> StockModels { get; set; }
        [JsonIgnore]
        public virtual ICollection<TradeModel> TradeModels { get; set; }
        [JsonIgnore]
        public virtual ICollection<SalesTrendModel> SalesTrendModels { get; set; }
        [JsonIgnore]
        [ForeignKey("OldProductModelId")]
        public virtual ProductModel OldProductModel { get; set; }

        [JsonIgnore]
        public virtual ICollection<CurrentStockModel> CurrentStockModels { get; set; }
        [JsonIgnore]
        public virtual ICollection<OrderModel> OrderModels { get; set; }
        [JsonIgnore]
        public virtual ICollection<InvoiceModel> InvoiceModels { get; set; }
        #endregion
    }
}
