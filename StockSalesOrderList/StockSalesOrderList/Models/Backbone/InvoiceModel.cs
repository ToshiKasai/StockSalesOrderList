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
    /// インボイス情報モデル
    /// </summary>
    [Table("invoices")]
    public class InvoiceModel : BaseModel
    {
        /// <summary>
        /// インボイスＩＤ
        /// </summary>
        [Key, Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// インボイス番号
        /// </summary>
        [DisplayName("インボイス番号"), Column("invoice_no")]
        [Required, MaxLength(128)]
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 倉庫コード
        /// </summary>
        [DisplayName("倉庫コード"), Column("warehouse_code")]
        [Required, MaxLength(128)]
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 入港予定日/Estimated Time of Arrival
        /// </summary>
        [DisplayName("入港予定日"), Column("eta", TypeName = "Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? ETA { get; set; }

        /// <summary>
        /// 通関日
        /// </summary>
        [DisplayName("通関日"), Column("customs_clearance", TypeName = "Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? CustomsClearanceDate { get; set; }

        /// <summary>
        /// 仕入日
        /// </summary>
        [DisplayName("仕入日"), Column("purchase_date", TypeName = "Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? PurchaseDate { get; set; }

        /// <summary>
        /// 商品ＩＤ
        /// </summary>
        [DisplayName("商品ＩＤ"), Column("product_id")]
        [Required]
        [Index("idx_product_id")]
        public int ProductModelId { get; set; }

        /// <summary>
        /// 仕入数量
        /// </summary>
        [DisplayName("仕入数量"), Column("quantity")]
        [DecimalPrecision(12, 3), DefaultValue(0), DefaultSqlValue(0)]
        public decimal Quantity { get; set; }

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
