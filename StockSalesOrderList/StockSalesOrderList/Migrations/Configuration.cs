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
                    Name = "�Ǘ���",
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
                    Name = "���R��",
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
                    Name = "�e�X�g���؃��[�U�[(��������)",
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
                    Name = "�m�F�p���[�U�[(�����Ȃ�)",
                    Password = new Microsoft.AspNet.Identity.PasswordHasher().HashPassword("Jfla0001"),
                    Expiration = System.DateTime.Now.AddMonths(1),
                    Email = "pguser@j-fla.test.com",
                    EmailConfirmed = false,
                    LockoutEnabled = true,
                    Enabled = true
                }
            );
            context.RoleModels.AddOrUpdate(
                new RoleModel { Name = "admin", DisplayName = "�S�̊Ǘ�" },
                new RoleModel { Name = "user", DisplayName = "���[�U�[�����e�i���X" },
                new RoleModel { Name = "maker", DisplayName = "���[�J�[�����e�i���X" },
                new RoleModel { Name = "group", DisplayName = "�O���[�v�����e�i���X" },
                new RoleModel { Name = "product", DisplayName = "���i�����e�i���X" }
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
                new ContainerModel { Name = "�Q�O�����h���C" },
                new ContainerModel { Name = "�S�O�����h���C" },
                new ContainerModel { Name = "�Q�O�������[�t�@" },
                new ContainerModel { Name = "�S�O�������[�t�@" }
            );
            context.OfficeModels.AddOrUpdate(
                new OfficeModel { Code = "1", Name = "����" },
                new OfficeModel { Code = "2", Name = "���" },
                new OfficeModel { Code = "3", Name = "����" },
                new OfficeModel { Code = "4", Name = "�D�y" },
                new OfficeModel { Code = "5", Name = "���É�" },
                new OfficeModel { Code = "6", Name = "����i�h�l�d�w�j", Deleted = true },
                new OfficeModel { Code = "7", Name = "�C���b�N�X", Deleted = true },
                new OfficeModel { Code = "**", Name = "���v", Deleted = true }
            );
        }
    }
}
