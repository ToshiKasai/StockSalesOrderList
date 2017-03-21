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
    /// 販売情報モデル
    /// </summary>
    [Table("sales")]
    public class SalesModel
    {
        /// <summary>
        /// 販売ＩＤ
        /// </summary>
        [Key, Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// 商品ＩＤ
        /// </summary>
        [DisplayName("商品ＩＤ"), Column("product_id")]
        [Required]
        [Index("ui_sales", IsUnique = true, Order = 0)]
        public int ProductModelId { get; set; }

        /// <summary>
        /// 対象年月
        /// </summary>
        [DisplayName("対象年月"), Column("detail_date", TypeName = "Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy/MM}")]
        [Required]
        [Index("ui_sales", IsUnique = true, Order = 1)]
        public DateTime TargetDate { get; set; }

        /// <summary>
        /// 事務所ＩＤ
        /// </summary>
        [DisplayName("事務所ＩＤ"), Column("office_id")]
        [Required]
        [Index("ui_sales", IsUnique = true, Order = 2)]
        public int OfficeModelId { get; set; }

        /// <summary>
        /// 販売予算
        /// </summary>
        [DisplayName("販売予算"), Column("plan")]
        [DecimalPrecision(12, 3), DefaultValue(0), DefaultSqlValue(0)]
        public decimal Plan { get; set; }

        /// <summary>
        /// 販売実績
        /// </summary>
        [DisplayName("販売実績"), Column("actual")]
        [DecimalPrecision(12, 3), DefaultValue(0), DefaultSqlValue(0)]
        public decimal Sales { get; set; }

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
        [ForeignKey("OfficeModelId")]
        public virtual OfficeModel OfficeModel { get; set; }
        #endregion
    }
}
