namespace StockSalesOrderList.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations.Model;
    using System.Data.Entity.ModelConfiguration.Configuration;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Web;

    public class DataContext : DbContext
    {
        private const string CacheKey = "__DataContext__";

        public DataContext()
            : base("name=DataContext")
        {
        }

        public static DataContext CreateContext()
        {
            var db = new DataContext();
            db.Database.Log = (log) => System.Diagnostics.Debug.WriteLine(log);
            return db;
        }

        public static bool HasContext
        {
            get { return HttpContext.Current.Items[CacheKey] != null; }
        }

        public static DataContext CurrentContext
        {
            get
            {
                DataContext context = (DataContext)HttpContext.Current.Items[CacheKey];
                if (context == null)
                {
                    context = new DataContext();
                    HttpContext.Current.Items[CacheKey] = context;
                }
                return context;
            }
        }

        public override int SaveChanges()
        {
            try
            {
                // DateTime now = DateTime.Now;
                // SetCreatedDateTime(now);
                // SetModifiedDateTime(now);
                RowVersion();
                return base.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                this.Database.Log = (log) => System.Diagnostics.Debug.WriteLine(log);
                // ex.Entries.Single().Reload();
                return 0;
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private void RowVersion()
        {
            var entities = this.ChangeTracker.Entries()
                .Where(e => (e.State == EntityState.Added || e.State == EntityState.Modified) && e.CurrentValues.PropertyNames.Contains("RowVersion"))
                .Select(e => e.Entity);
            foreach (dynamic entity in entities)
            {
                entity.RowVersion++;
            }
        }

        private void SetCreatedDateTime(DateTime now)
        {
            var entities = this.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added && e.CurrentValues.PropertyNames.Contains("CreatedDateTime"))
                .Select(e => e.Entity);
            foreach (dynamic entity in entities)
            {
                entity.CreatedDateTime = now;
            }
        }

        private void SetModifiedDateTime(DateTime now)
        {
            var entities = this.ChangeTracker.Entries()
                .Where(e => (e.State == EntityState.Added || e.State == EntityState.Modified) && e.CurrentValues.PropertyNames.Contains("ModifiedDateTime"))
                .Select(e => e.Entity);
            foreach (dynamic entity in entities)
            {
                entity.ModifiedDateTime = now;
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            foreach (Type classType in from t in Assembly.GetAssembly(typeof(DecimalPrecisionAttribute)).GetTypes()
                                       where t.IsClass && t.Namespace == "StockSalesOrderList.Models"
                                       select t)
            {
                foreach (var propAttr in classType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.GetCustomAttribute<DecimalPrecisionAttribute>() != null).Select(
                    p => new { prop = p, attr = p.GetCustomAttribute<DecimalPrecisionAttribute>(true) }))
                {
                    var entityConfig = modelBuilder.GetType().GetMethod("Entity").MakeGenericMethod(classType).Invoke(modelBuilder, null);
                    ParameterExpression param = ParameterExpression.Parameter(classType, "c");
                    Expression property = Expression.Property(param, propAttr.prop.Name);
                    LambdaExpression lambdaExpression = Expression.Lambda(property, true, new ParameterExpression[] { param });
                    DecimalPropertyConfiguration decimalConfig;
                    if (propAttr.prop.PropertyType.IsGenericType && propAttr.prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        MethodInfo methodInfo = entityConfig.GetType().GetMethods().Where(p => p.Name == "Property").ToList()[7];
                        decimalConfig = methodInfo.Invoke(entityConfig, new[] { lambdaExpression }) as DecimalPropertyConfiguration;
                    }
                    else
                    {
                        MethodInfo methodInfo = entityConfig.GetType().GetMethods().Where(p => p.Name == "Property").ToList()[6];
                        decimalConfig = methodInfo.Invoke(entityConfig, new[] { lambdaExpression }) as DecimalPropertyConfiguration;
                    }
                    decimalConfig.HasPrecision(propAttr.attr.Precision, propAttr.attr.Scale);
                }
            }

            modelBuilder.Conventions.Add(new AttributeToColumnAnnotationConvention<DefaultSqlValueAttribute, object>("DefaultValueSql", (p, attributes) => attributes.Single().Value));
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<UserModel> UserModels { get; set; }
        public DbSet<RoleModel> RoleModels { get; set; }
        public DbSet<UserRoleModel> UserRoleModels { get; set; }

        public DbSet<ContainerModel> ContainerModels { get; set; }
        public DbSet<GroupModel> GroupModels { get; set; }
        public DbSet<GroupProductModel> GroupProductModels { get; set; }
        public DbSet<MakerModel> MakerModels { get; set; }
        public DbSet<OfficeModel> OfficeModels { get; set; }
        public DbSet<ProductModel> ProductModels { get; set; }
        public DbSet<SalesModel> SalesModels { get; set; }
        public DbSet<SalesTrendModel> SalesTrendModels { get; set; }
        public DbSet<StockModel> StockModels { get; set; }
        public DbSet<TradeModel> TradeModels { get; set; }
        public DbSet<UserMakerModel> UserMakerModels { get; set; }

        public DbSet<SignInLogModel> SignInLogModels { get; set; }
        public DbSet<ApplicationLogModel> ApplicationLogModel { get; set; }

        // 表示用
        public DbSet<CurrentStockModel> CurrentStockModels { get; set; }
        public DbSet<InvoiceModel> InvoiceModels { get; set; }
        public DbSet<OrderModel> OrderModels { get; set; }
    }

    public class ExtendedSqlGenerator : MySql.Data.Entity.MySqlMigrationSqlGenerator
    {
        protected override string Generate(ColumnModel op)
        {
            AnnotationValues values;
            if (op.Annotations.TryGetValue("DefaultValueSql", out values))
            {
                op.DefaultValueSql = (string)values.NewValue;
            }
            return base.Generate(op);
        }
    }
}
