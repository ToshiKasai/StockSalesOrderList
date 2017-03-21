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
    /// グループ商品情報モデル
    /// </summary>
    [Table("groups_products")]
    public class GroupProductModel : BaseModel
    {
        /// <summary>
        /// グループ商品ＩＤ
        /// </summary>
        [Key, Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// グループＩＤ
        /// </summary>
        [DisplayName("グループＩＤ"), Column("group_id")]
        [Required]
        [Index("idx_group_id")]
        public int GroupModelId { get; set; }

        /// <summary>
        /// 商品ＩＤ
        /// </summary>
        [DisplayName("商品ＩＤ"), Column("product_id")]
        [Required]
        [Index("idx_product_id")]
        public int ProductModelId { get; set; }

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
        [ForeignKey("GroupModelId")]
        public virtual GroupModel GroupModel { get; set; }
        [JsonIgnore]
        [ForeignKey("ProductModelId")]
        public virtual ProductModel ProductModel { get; set; }
        #endregion
    }
}
