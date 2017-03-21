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
    /// グループ情報モデル
    /// </summary>
    [Table("groups")]
    public class GroupModel : BaseModel
    {
        /// <summary>
        /// グループＩＤ
        /// </summary>
        [Key, Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// グループコード
        /// </summary>
        [DisplayName("グループコード"), Column("code")]
        [Required, MaxLength(128), RegularExpression(@"[a-zA-Z0-9]+")]
        [Index("ui_code", IsUnique = true)]
        public string Code { get; set; }

        /// <summary>
        /// グループ名
        /// </summary>
        [DisplayName("グループ名"), Column("name")]
        [Required, MaxLength(256)]
        public string Name { get; set; }

        /// <summary>
        /// メーカーＩＤ
        /// </summary>
        [DisplayName("メーカーＩＤ"), Column("maker_id")]
        [Required]
        [Index("idx_maker_id")]
        public int MakerModelId { get; set; }

        /// <summary>
        /// コンテナＩＤ
        /// </summary>
        [DisplayName("コンテナＩＤ"), Column("container_id")]
        [Required]
        [Index("idx_container_id")]
        public int ContainerModelId { get; set; }

        /// <summary>
        /// 容量管理
        /// </summary>
        [DisplayName("容量管理"), Column("is_capacity")]
        [DefaultValue(true), DefaultSqlValue(true)]
        public bool IsCapacity { get; set; }

        /// <summary>
        /// コンテナ入数：２０ドライ
        /// </summary>
        [DisplayName("コンテナ入数：２０ドライ"), Column("capacity_20dry")]
        [DecimalPrecision(12, 3), DefaultSqlValue(0)]
        public decimal ContainerCapacityBt20Dry { get; set; }

        /// <summary>
        /// コンテナ入数：４０ドライ
        /// </summary>
        [DisplayName("コンテナ入数：４０ドライ"), Column("capacity_40dry")]
        [DecimalPrecision(12, 3), DefaultSqlValue(0)]
        public decimal ContainerCapacityBt40Dry { get; set; }

        /// <summary>
        /// コンテナ入数：２０リーファ
        /// </summary>
        [DisplayName("コンテナ入数：２０リーファ"), Column("capacity_20reefer")]
        [DecimalPrecision(12, 3), DefaultSqlValue(0)]
        public decimal ContainerCapacityBt20Reefer { get; set; }

        /// <summary>
        /// コンテナ入数：４０リーファ
        /// </summary>
        [DisplayName("コンテナ入数：４０リーファ"), Column("capacity_40reefer")]
        [DecimalPrecision(12, 3), DefaultSqlValue(0)]
        public decimal ContainerCapacityBt40Reefer { get; set; }

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
        [ForeignKey("ContainerModelId")]
        public virtual ContainerModel ContainerModel { get; set; }
        #endregion
    }
}
