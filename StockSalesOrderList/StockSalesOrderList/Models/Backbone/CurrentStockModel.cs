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
    /// 現在在庫情報モデル
    /// </summary>
    [Table("current_ctocks")]
    public class CurrentStockModel : BaseModel
    {
        /// <summary>
        /// 現在在庫ＩＤ
        /// </summary>
        [Key, Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// 商品ＩＤ
        /// </summary>
        [DisplayName("商品ＩＤ"), Column("product_id")]
        [Required]
        [Index("idx_product_id")]
        public int ProductModelId { get; set; }

        /// <summary>
        /// 倉庫コード
        /// </summary>
        [DisplayName("倉庫コード"), Column("warehouse_code")]
        [Required, MaxLength(128)]
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 倉庫名
        /// </summary>
        [DisplayName("倉庫名"), Column("warehouse_name")]
        [Required, MaxLength(256)]
        public string WarehouseName { get; set; }

        /// <summary>
        /// 打検名
        /// </summary>
        [DisplayName("打検名"), Column("state_name")]
        [Required, MaxLength(128)]
        public string StateName { get; set; }

        /// <summary>
        /// 論理在庫数
        /// </summary>
        [DisplayName("論理在庫数"), Column("logical_qty")]
        [DecimalPrecision(12, 3), DefaultValue(0), DefaultSqlValue(0)]
        public decimal LogicalQuantity { get; set; }

        /// <summary>
        /// 実在庫数量
        /// </summary>
        [DisplayName("実在庫数量"), Column("actual_qty")]
        [DecimalPrecision(12, 3), DefaultValue(0), DefaultSqlValue(0)]
        public decimal ActualQuantity { get; set; }

        /// <summary>
        /// 賞味期限
        /// </summary>
        [DisplayName("賞味期限"), Column("expiration_date", TypeName = "Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? ExpirationDate { get; set; }

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
