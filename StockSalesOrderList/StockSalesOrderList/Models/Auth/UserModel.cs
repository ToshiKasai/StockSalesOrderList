using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace StockSalesOrderList.Models
{
    /// <summary>
    /// ユーザー情報モデル
    /// </summary>
    [Table("users")]
    public class UserModel : BaseModel, IUser<int>
    {
        #region IUser<int>
        /// <summary>
        /// ユーザーＩＤ
        /// </summary>
        [Key, Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// ログインＩＤ
        /// </summary>
        [DisplayName("ログインＩＤ"), Column("code")]
        [Required, MaxLength(256)]
        [Index("ui_code", IsUnique = true)]
        public string UserName { get; set; }
        #endregion

        #region カスタム項目
        /// <summary>
        /// ハッシュパスワード
        /// </summary>
        [DisplayName("ハッシュパスワード"), Column("password")]
        [DataType(DataType.Password)]
        [Required, MaxLength(256)]
        public string Password { get; set; }

        /// <summary>
        /// ユーザー名
        /// </summary>
        [DisplayName("ユーザー名"), Column("name")]
        [MaxLength(256)]
        public string Name { get; set; }

        /// <summary>
        /// 有効期限
        /// </summary>
        [DisplayName("有効期限"), Column("expiration", TypeName = "Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime Expiration { get; set; }

        /// <summary>
        /// パスワード変更スキップ回数
        /// </summary>
        [DisplayName("パスワード変更スキップ回数"), Column("password_skip_count")]
        [DefaultValue(0), DefaultSqlValue(0)]
        public int PasswordSkipCnt { get; set; }

        /// <summary>
        /// 資格情報
        /// </summary>
        [DisplayName("資格情報"), Column("security_timestamp")]
        [MaxLength(256)]
        public string SecurityTimestamp { get; set; }

        /// <summary>
        /// メールアドレス
        /// </summary>
        [DisplayName("メールアドレス"), Column("email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress, MaxLength(128)]
        public string Email { get; set; }

        /// <summary>
        /// メールアドレス確認済
        /// </summary>
        [DisplayName("メールアドレス確認済"), Column("email_confirmed")]
        [DefaultValue(false), DefaultSqlValue(false)]
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// ロックアウト終了日時
        /// </summary>
        [DisplayName("ロックアウト終了日時"), Column("lockout_end_data")]
        public DateTime? LockoutEndData { get; set; }

        /// <summary>
        /// ロックアウト許可
        /// </summary>
        [DisplayName("ロックアウト許可"), Column("lockout_enabled")]
        [DefaultValue(true), DefaultSqlValue(true)]
        public bool LockoutEnabled { get; set; }

        /// <summary>
        /// アクセス失敗回数
        /// </summary>
        [DisplayName("アクセス失敗回数"), Column("access_failed_count")]
        [DefaultValue(0), DefaultSqlValue(0)]
        public int AccessFailedCount { get; set; }

        /// <summary>
        /// 使用許可
        /// </summary>
        [DisplayName("使用許可"), Column("enabled")]
        [DefaultValue(false), DefaultSqlValue(false)]
        public bool Enabled { get; set; }
        #endregion

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
        public DateTime ModifiedDateTime { get; set; }
        #endregion

        #region サポート機能
        /// <summary>
        /// 
        /// </summary>
        public class ApplicationClaimTypes
        {
            public const string ClaimUserName = "http://m.takayama/claims/logincd";
        }

        /// <summary>
        /// ロックアウト時間の取得/設定用(UTCで操作)
        /// </summary>
        [NotMapped]
        public DateTimeOffset LockoutEndDataUtc
        {
            get
            {
                if (this.LockoutEndData != null)
                {
                    return DateTime.SpecifyKind((DateTime)this.LockoutEndData, DateTimeKind.Utc);
                }
                else
                {
                    return DateTimeOffset.MinValue;
                }
            }
            set
            {
                this.LockoutEndData = value.UtcDateTime;
            }
        }

        [NotMapped]
        public bool IsLockout
        {
            get
            {
                return LockoutEndDataUtc > DateTime.UtcNow;
            }
        }

        public Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager)
        {
            return Task.FromResult(GenerateUserIdentity(manager));
        }

        public ClaimsIdentity GenerateUserIdentity(ApplicationUserManager manager)
        {
            manager.UpdateSecurityStamp(this.Id);

            ClaimsIdentity userIdentity = manager.CreateIdentity(this, DefaultAuthenticationTypes.ApplicationCookie);
            if (string.IsNullOrEmpty(this.Name))
            {
                userIdentity.AddClaim(new Claim(ApplicationClaimTypes.ClaimUserName, "名前未登録"));
            }
            else
            {
                userIdentity.AddClaim(new Claim(ApplicationClaimTypes.ClaimUserName, this.Name.ToString()));
            }
            return userIdentity;
        }
        #endregion

        #region データ連携
        [JsonIgnore]
        public virtual ICollection<UserRoleModel> UserRoleModels { get; set; }

        [JsonIgnore]
        public virtual ICollection<UserMakerModel> UserMakerModels { get; set; }

        [JsonIgnore]
        public virtual ICollection<SalesTrendModel> SalesTrendModels { get; set; }

        [JsonIgnore]
        public  virtual ICollection<SignInLogModel> SignInLogModels { get; set; }
        #endregion
    }
}
