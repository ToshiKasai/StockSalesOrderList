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
    /// メーカー情報モデル
    /// </summary>
    [Table("makers")]
    public class MakerModel : BaseModel
    {
        /// <summary>
        /// メーカーＩＤ
        /// </summary>
        [Key, Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// メーカーコード
        /// </summary>
        [DisplayName("メーカーコード"), Column("code")]
        [Required, MaxLength(128), RegularExpression(@"[a-zA-Z0-9]+")]
        [Index("ui_code", IsUnique = true)]
        public string Code { get; set; }

        /// <summary>
        /// メーカー名
        /// </summary>
        [DisplayName("メーカー名"), Column("name")]
        [Required, MaxLength(256)]
        public string Name { get; set; }

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
        public virtual ICollection<ProductModel> ProductModels { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserMakerModel> UserMakerModels { get; set; }
        [JsonIgnore]
        public virtual ICollection<GroupModel> GroupModels { get; set; }
        #endregion
    }
}
