using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StockSalesOrderList.Models.WebApi
{
    public class ProductApiModel : BaseModel
    {
        public int Id { get; set; }

        [DisplayName("商品コード")]
        [Required]
        public string Code { get; set; }

        [DisplayName("商品名")]
        [Required, MaxLength(256)]
        public string Name { get; set; }

        [DisplayName("入数")]
        [Required, Range(0.0, 100.0)]
        public decimal Quantity { get; set; }

        [DisplayName("メーカー")]
        [Required]
        public int MakerModelId { get; set; }

        [DisplayName("メーカーコード")]
        [Required, MaxLength(128)]
        public string MakerCode { get; set; }

        [DisplayName("メーカー名")]
        [Required, MaxLength(256)]
        public string MakerName { get; set; }

        [DisplayName("計量品")]
        [Required]
        public bool IsSoldWeight { get; set; }

        [DisplayName("パレット入数")]
        public decimal? PaletteQuantity { get; set; }

        [DisplayName("カートン入数")]
        public decimal? CartonQuantity { get; set; }

        [DisplayName("ケース高さ")]
        public decimal? CaseHeight { get; set; }

        [DisplayName("ケース幅")]
        public decimal? CaseWidth { get; set; }

        [DisplayName("ケース奥行き")]
        public decimal? CaseDepth { get; set; }

        [DisplayName("ケース容量")]
        public decimal? CaseCapacity { get; set; }

        [DisplayName("リードタイム")]
        public int? LeadTime { get; set; }

        [DisplayName("発注間隔")]
        public int? OrderInterval { get; set; }

        [DisplayName("関連商品")]
        public int? OldProductModelId { get; set; }

        [DisplayName("倍率")]
        public decimal? Magnification { get; set; }

        [DisplayName("最低発注数量")]
        public decimal? MinimumOrderQuantity { get; set; }

        [DisplayName("使用許可")]
        public bool Enabled { get; set; }

        [DisplayName("削除済")]
        public bool Deleted { get; set; }
    }
}
