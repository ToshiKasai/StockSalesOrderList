using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StockSalesOrderList.Models.WebApi
{
    public class GroupApiModel : BaseModel
    {
        public int Id { get; set; }

        [DisplayName("グループコード")]
        [Required, MaxLength(128), RegularExpression(@"[a-zA-Z0-9]+")]
        public string Code { get; set; }

        [DisplayName("グループ名")]
        [Required, MaxLength(256)]
        public string Name { get; set; }

        [DisplayName("メーカーＩＤ")]
        [Required]
        public int MakerModelId { get; set; }

        [DisplayName("メーカーコード")]
        public string MakerCode { get; set; }

        [DisplayName("メーカー名")]
        public string MakerName { get; set; }

        [DisplayName("コンテナＩＤ")]
        [Required]
        public int ContainerModelId { get; set; }

        [DisplayName("コンテナ名")]
        public string ContainerName { get; set; }

        [DisplayName("容量管理")]
        public bool IsCapacity { get; set; }

        [DisplayName("コンテナ入数：２０ドライ")]
        public decimal ContainerCapacityBt20Dry { get; set; }

        [DisplayName("コンテナ入数：４０ドライ")]
        public decimal ContainerCapacityBt40Dry { get; set; }

        [DisplayName("コンテナ入数：２０リーファ")]
        public decimal ContainerCapacityBt20Reefer { get; set; }

        [DisplayName("コンテナ入数：４０リーファ")]
        public decimal ContainerCapacityBt40Reefer { get; set; }

        [DisplayName("削除済")]
        public bool Deleted { get; set; }
    }
}
