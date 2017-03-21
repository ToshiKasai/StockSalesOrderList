namespace StockSalesOrderList.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<StockSalesOrderList.Models.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(StockSalesOrderList.Models.DataContext context)
        {
            context.UserModels.AddOrUpdate(
                new UserModel
                {
                    UserName = "administrator",
                    Name = "管理者",
                    Password = new Microsoft.AspNet.Identity.PasswordHasher().HashPassword("Administrator7"),
                    Expiration = System.DateTime.Now.AddMonths(1),
                    Email = "serverlog@j-fla.com",
                    EmailConfirmed = true,
                    LockoutEnabled = true,
                    Enabled = true
                },
                new UserModel
                {
                    UserName = "90990002",
                    Name = "高山稔",
                    Password = new Microsoft.AspNet.Identity.PasswordHasher().HashPassword("Minoru77!"),
                    Expiration = System.DateTime.Now.AddMonths(1),
                    Email = "m.takayama@j-fla.com",
                    EmailConfirmed = true,
                    LockoutEnabled = true,
                    Enabled = true
                },
                new UserModel
                {
                    UserName = "TESTUSER",
                    Name = "テスト検証ユーザー(権限あり)",
                    Password = new Microsoft.AspNet.Identity.PasswordHasher().HashPassword("Jfla0001"),
                    Expiration = System.DateTime.Now.AddMonths(1),
                    Email = "testuser@j-fla.test.com",
                    EmailConfirmed = false,
                    LockoutEnabled = true,
                    Enabled = true
                },
                new UserModel
                {
                    UserName = "PGUSER",
                    Name = "確認用ユーザー(権限なし)",
                    Password = new Microsoft.AspNet.Identity.PasswordHasher().HashPassword("Jfla0001"),
                    Expiration = System.DateTime.Now.AddMonths(1),
                    Email = "pguser@j-fla.test.com",
                    EmailConfirmed = false,
                    LockoutEnabled = true,
                    Enabled = true
                }
            );
            context.RoleModels.AddOrUpdate(
                new RoleModel { Name = "admin", DisplayName = "全体管理" },
                new RoleModel { Name = "user", DisplayName = "ユーザーメンテナンス" },
                new RoleModel { Name = "maker", DisplayName = "メーカーメンテナンス" },
                new RoleModel { Name = "group", DisplayName = "グループメンテナンス" },
                new RoleModel { Name = "product", DisplayName = "商品メンテナンス" }
            );
            context.UserRoleModels.AddOrUpdate(
                new UserRoleModel { UserModelId = 1, RoleModelId = 1 },
                new UserRoleModel { UserModelId = 2, RoleModelId = 1 },
                new UserRoleModel { UserModelId = 3, RoleModelId = 2 },
                new UserRoleModel { UserModelId = 3, RoleModelId = 3 },
                new UserRoleModel { UserModelId = 3, RoleModelId = 4 },
                new UserRoleModel { UserModelId = 3, RoleModelId = 5 }
            );
            context.ContainerModels.AddOrUpdate(
                new ContainerModel { Name = "２０ｆｔドライ" },
                new ContainerModel { Name = "４０ｆｔドライ" },
                new ContainerModel { Name = "２０ｆｔリーファ" },
                new ContainerModel { Name = "４０ｆｔリーファ" }
            );
            context.OfficeModels.AddOrUpdate(
                new OfficeModel { Code = "1", Name = "東京" },
                new OfficeModel { Code = "2", Name = "大阪" },
                new OfficeModel { Code = "3", Name = "福岡" },
                new OfficeModel { Code = "4", Name = "札幌" },
                new OfficeModel { Code = "5", Name = "名古屋" },
                new OfficeModel { Code = "6", Name = "長崎（ＩＭＥＸ）", Deleted = true },
                new OfficeModel { Code = "7", Name = "イメックス", Deleted = true },
                new OfficeModel { Code = "**", Name = "合計", Deleted = true }
            );
        }
    }
}
