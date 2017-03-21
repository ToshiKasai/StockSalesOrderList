using MySql.Data.Entity;
using MySql.Data.MySqlClient;
using StockSalesOrderList.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Migrations.History;
using System.Linq;
using System.Web;

namespace StockSalesOrderList.Migrations
{
    public class MysqlConfiguration : DbConfiguration
    {
        public MysqlConfiguration()
        {
            AddDependencyResolver(new MySqlDependencyResolver());
            SetProviderFactory(MySqlProviderInvariantName.ProviderName, new MySqlClientFactory());
            SetDefaultConnectionFactory(new MySqlConnectionFactory());
            // SetMigrationSqlGenerator(MySqlProviderInvariantName.ProviderName, () => new MySqlMigrationSqlGenerator());
            SetMigrationSqlGenerator(MySqlProviderInvariantName.ProviderName, () => new ExtendedSqlGenerator());
            SetProviderServices(MySqlProviderInvariantName.ProviderName, new MySqlProviderServices());
            SetProviderFactoryResolver(new MySqlProviderFactoryResolver());
            SetManifestTokenResolver(new MySqlManifestTokenResolver());

            // __migrationHistory テーブルのデフォルト設定の変更
            SetHistoryContext("MySql.Data.MySqlClient", (connection, defaultSchema) => new MysqlHistoryContext(connection, defaultSchema));
        }

        public class MysqlHistoryContext : HistoryContext
        {
            public MysqlHistoryContext(DbConnection dbConnection, string defaultSchema)
                : base(dbConnection, defaultSchema)
            {
            }

            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                modelBuilder.Entity<HistoryRow>().Property(h => h.MigrationId).HasMaxLength(255).IsRequired();
                modelBuilder.Entity<HistoryRow>().Property(h => h.ContextKey).HasMaxLength(255).IsRequired();
            }
        }
    }
}
