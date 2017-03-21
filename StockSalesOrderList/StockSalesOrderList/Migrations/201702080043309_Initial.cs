namespace StockSalesOrderList.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.application_logs",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        processing_date = c.DateTime(nullable: false, precision: 0),
                        process_name = c.String(maxLength: 256, storeType: "nvarchar"),
                        user_id = c.Int(nullable: false),
                        message = c.String(unicode: false),
                        deleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "False")
                                },
                            }),
                        created_at = c.DateTime(nullable: false, precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP")
                                },
                            }),
                        modified_at = c.DateTime(precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP")
                                },
                            }),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.users", t => t.user_id, cascadeDelete: true)
                .Index(t => t.user_id, name: "idx_user_id");
            
            CreateTable(
                "dbo.users",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        code = c.String(nullable: false, maxLength: 256, storeType: "nvarchar"),
                        password = c.String(nullable: false, maxLength: 256, storeType: "nvarchar"),
                        name = c.String(maxLength: 256, storeType: "nvarchar"),
                        expiration = c.DateTime(nullable: false, storeType: "date"),
                        password_skip_count = c.Int(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        security_timestamp = c.String(maxLength: 256, storeType: "nvarchar"),
                        email = c.String(maxLength: 128, storeType: "nvarchar"),
                        email_confirmed = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "False")
                                },
                            }),
                        lockout_end_data = c.DateTime(precision: 0),
                        lockout_enabled = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "True")
                                },
                            }),
                        access_failed_count = c.Int(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        enabled = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "False")
                                },
                            }),
                        deleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "False")
                                },
                            }),
                        created_at = c.DateTime(nullable: false, precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP")
                                },
                            }),
                        modified_at = c.DateTime(nullable: false, precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP")
                                },
                            }),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.code, unique: true, name: "ui_code");
            
            CreateTable(
                "dbo.sales_trends",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        product_id = c.Int(nullable: false),
                        detail_date = c.DateTime(nullable: false, storeType: "date"),
                        quantity = c.Decimal(nullable: false, precision: 12, scale: 3,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        comments = c.String(unicode: false),
                        user_id = c.Int(nullable: false),
                        deleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "False")
                                },
                            }),
                        created_at = c.DateTime(nullable: false, precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP")
                                },
                            }),
                        modified_at = c.DateTime(precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP")
                                },
                            }),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.products", t => t.product_id, cascadeDelete: true)
                .ForeignKey("dbo.users", t => t.user_id, cascadeDelete: true)
                .Index(t => new { t.product_id, t.detail_date }, name: "idx_product_id_detail_date")
                .Index(t => t.user_id, name: "idx_user_id");
            
            CreateTable(
                "dbo.products",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        code = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        name = c.String(nullable: false, maxLength: 256, storeType: "nvarchar"),
                        quantity = c.Decimal(nullable: false, precision: 12, scale: 3,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        maker_id = c.Int(nullable: false),
                        is_sold_weight = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "False")
                                },
                            }),
                        palette_quantity = c.Decimal(precision: 12, scale: 3),
                        carton_quantity = c.Decimal(precision: 12, scale: 3),
                        case_height = c.Decimal(precision: 12, scale: 3),
                        case_width = c.Decimal(precision: 12, scale: 3),
                        case_depth = c.Decimal(precision: 12, scale: 3),
                        case_capacity = c.Decimal(precision: 12, scale: 3),
                        lead_time = c.Int(),
                        order_interval = c.Int(),
                        old_product_id = c.Int(),
                        magnification = c.Decimal(precision: 12, scale: 3),
                        minimum_order_quantity = c.Decimal(precision: 18, scale: 2),
                        enabled = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "True")
                                },
                            }),
                        deleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "False")
                                },
                            }),
                        created_at = c.DateTime(nullable: false, precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP")
                                },
                            }),
                        modified_at = c.DateTime(precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP")
                                },
                            }),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.makers", t => t.maker_id, cascadeDelete: true)
                .ForeignKey("dbo.products", t => t.old_product_id)
                .Index(t => t.code, unique: true, name: "ui_code")
                .Index(t => t.maker_id, name: "idx_maker_id")
                .Index(t => t.old_product_id, name: "idx_old_product_id");
            
            CreateTable(
                "dbo.current_ctocks",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        product_id = c.Int(nullable: false),
                        warehouse_code = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        warehouse_name = c.String(nullable: false, maxLength: 256, storeType: "nvarchar"),
                        state_name = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        logical_qty = c.Decimal(nullable: false, precision: 12, scale: 3,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        actual_qty = c.Decimal(nullable: false, precision: 12, scale: 3,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        expiration_date = c.DateTime(storeType: "date"),
                        deleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "False")
                                },
                            }),
                        created_at = c.DateTime(nullable: false, precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP")
                                },
                            }),
                        modified_at = c.DateTime(precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP")
                                },
                            }),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.products", t => t.product_id, cascadeDelete: true)
                .Index(t => t.product_id, name: "idx_product_id");
            
            CreateTable(
                "dbo.groups_products",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        group_id = c.Int(nullable: false),
                        product_id = c.Int(nullable: false),
                        deleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "False")
                                },
                            }),
                        created_at = c.DateTime(nullable: false, precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP")
                                },
                            }),
                        modified_at = c.DateTime(precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP")
                                },
                            }),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.groups", t => t.group_id, cascadeDelete: true)
                .ForeignKey("dbo.products", t => t.product_id, cascadeDelete: true)
                .Index(t => t.group_id, name: "idx_group_id")
                .Index(t => t.product_id, name: "idx_product_id");
            
            CreateTable(
                "dbo.groups",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        code = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        name = c.String(nullable: false, maxLength: 256, storeType: "nvarchar"),
                        maker_id = c.Int(nullable: false),
                        container_id = c.Int(nullable: false),
                        is_capacity = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "True")
                                },
                            }),
                        capacity_20dry = c.Decimal(nullable: false, precision: 12, scale: 3,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        capacity_40dry = c.Decimal(nullable: false, precision: 12, scale: 3,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        capacity_20reefer = c.Decimal(nullable: false, precision: 12, scale: 3,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        capacity_40reefer = c.Decimal(nullable: false, precision: 12, scale: 3,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        deleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "False")
                                },
                            }),
                        created_at = c.DateTime(nullable: false, precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP")
                                },
                            }),
                        modified_at = c.DateTime(precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP")
                                },
                            }),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.containers", t => t.container_id, cascadeDelete: true)
                .ForeignKey("dbo.makers", t => t.maker_id, cascadeDelete: true)
                .Index(t => t.code, unique: true, name: "ui_code")
                .Index(t => t.maker_id, name: "idx_maker_id")
                .Index(t => t.container_id, name: "idx_container_id");
            
            CreateTable(
                "dbo.containers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 256, storeType: "nvarchar"),
                        deleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "False")
                                },
                            }),
                        created_at = c.DateTime(nullable: false, precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP")
                                },
                            }),
                        modified_at = c.DateTime(precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP")
                                },
                            }),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.makers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        code = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        name = c.String(nullable: false, maxLength: 256, storeType: "nvarchar"),
                        enabled = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "True")
                                },
                            }),
                        deleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "False")
                                },
                            }),
                        created_at = c.DateTime(nullable: false, precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP")
                                },
                            }),
                        modified_at = c.DateTime(precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP")
                                },
                            }),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.code, unique: true, name: "ui_code");
            
            CreateTable(
                "dbo.users_makers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        user_id = c.Int(nullable: false),
                        maker_id = c.Int(nullable: false),
                        deleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "False")
                                },
                            }),
                        created_at = c.DateTime(nullable: false, precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP")
                                },
                            }),
                        modified_at = c.DateTime(precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP")
                                },
                            }),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.makers", t => t.maker_id, cascadeDelete: true)
                .ForeignKey("dbo.users", t => t.user_id, cascadeDelete: true)
                .Index(t => t.user_id, name: "idx_user_id")
                .Index(t => t.maker_id, name: "idx_maker_id");
            
            CreateTable(
                "dbo.invoices",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        invoice_no = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        warehouse_code = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        eta = c.DateTime(storeType: "date"),
                        customs_clearance = c.DateTime(storeType: "date"),
                        purchase_date = c.DateTime(storeType: "date"),
                        product_id = c.Int(nullable: false),
                        quantity = c.Decimal(nullable: false, precision: 12, scale: 3,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        deleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "False")
                                },
                            }),
                        created_at = c.DateTime(nullable: false, precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP")
                                },
                            }),
                        modified_at = c.DateTime(precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP")
                                },
                            }),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.products", t => t.product_id, cascadeDelete: true)
                .Index(t => t.product_id, name: "idx_product_id");
            
            CreateTable(
                "dbo.orders",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        order_no = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        order_date = c.DateTime(nullable: false, storeType: "date"),
                        product_id = c.Int(nullable: false),
                        quantity = c.Decimal(nullable: false, precision: 12, scale: 3,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        deleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "False")
                                },
                            }),
                        created_at = c.DateTime(nullable: false, precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP")
                                },
                            }),
                        modified_at = c.DateTime(precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP")
                                },
                            }),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.products", t => t.product_id, cascadeDelete: true)
                .Index(t => t.product_id, name: "idx_product_id");
            
            CreateTable(
                "dbo.sales",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        product_id = c.Int(nullable: false),
                        detail_date = c.DateTime(nullable: false, storeType: "date"),
                        office_id = c.Int(nullable: false),
                        plan = c.Decimal(nullable: false, precision: 12, scale: 3,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        actual = c.Decimal(nullable: false, precision: 12, scale: 3,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        deleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "False")
                                },
                            }),
                        created_at = c.DateTime(nullable: false, precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP")
                                },
                            }),
                        modified_at = c.DateTime(precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP")
                                },
                            }),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.offices", t => t.office_id, cascadeDelete: true)
                .ForeignKey("dbo.products", t => t.product_id, cascadeDelete: true)
                .Index(t => new { t.product_id, t.detail_date, t.office_id }, unique: true, name: "ui_sales");
            
            CreateTable(
                "dbo.offices",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        code = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        name = c.String(nullable: false, maxLength: 256, storeType: "nvarchar"),
                        deleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "False")
                                },
                            }),
                        created_at = c.DateTime(nullable: false, precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP")
                                },
                            }),
                        modified_at = c.DateTime(precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP")
                                },
                            }),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.code, unique: true, name: "ui_code");
            
            CreateTable(
                "dbo.stocks",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        product_id = c.Int(nullable: false),
                        detail_date = c.DateTime(nullable: false, storeType: "date"),
                        stocks = c.Decimal(nullable: false, precision: 12, scale: 3,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        deleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "False")
                                },
                            }),
                        created_at = c.DateTime(nullable: false, precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP")
                                },
                            }),
                        modified_at = c.DateTime(precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP")
                                },
                            }),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.products", t => t.product_id, cascadeDelete: true)
                .Index(t => new { t.product_id, t.detail_date }, unique: true, name: "ui_product_id_detail_date");
            
            CreateTable(
                "dbo.trades",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        product_id = c.Int(nullable: false),
                        detail_date = c.DateTime(nullable: false, storeType: "date"),
                        orders_plan_qty = c.Decimal(nullable: false, precision: 12, scale: 3,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        orders_qty = c.Decimal(nullable: false, precision: 12, scale: 3,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        invoice_plan_qty = c.Decimal(nullable: false, precision: 12, scale: 3,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        invoice_qty = c.Decimal(nullable: false, precision: 12, scale: 3,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        remaining_invoice_qty = c.Decimal(nullable: false, precision: 12, scale: 3,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        adjustment_invoice_qty = c.Decimal(nullable: false, precision: 12, scale: 3,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        deleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "False")
                                },
                            }),
                        created_at = c.DateTime(nullable: false, precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP")
                                },
                            }),
                        modified_at = c.DateTime(precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP")
                                },
                            }),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.products", t => t.product_id, cascadeDelete: true)
                .Index(t => new { t.product_id, t.detail_date }, unique: true, name: "ui_product_id_detail_date");
            
            CreateTable(
                "dbo.signin_logs",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        processing_date = c.DateTime(nullable: false, precision: 0),
                        client_ip = c.String(maxLength: 50, storeType: "nvarchar"),
                        user_code = c.String(maxLength: 256, storeType: "nvarchar"),
                        status = c.Int(nullable: false),
                        message = c.String(unicode: false),
                        user_id = c.Int(),
                        deleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "False")
                                },
                            }),
                        created_at = c.DateTime(nullable: false, precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP")
                                },
                            }),
                        modified_at = c.DateTime(precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP")
                                },
                            }),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.users", t => t.user_id)
                .Index(t => t.user_code, name: "idx_user_code")
                .Index(t => t.user_id, name: "idx_user_id");
            
            CreateTable(
                "dbo.users_roles",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        user_id = c.Int(nullable: false),
                        role_id = c.Int(nullable: false),
                        deleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "False")
                                },
                            }),
                        created_at = c.DateTime(nullable: false, precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP")
                                },
                            }),
                        modified_at = c.DateTime(precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP")
                                },
                            }),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.roles", t => t.role_id, cascadeDelete: true)
                .ForeignKey("dbo.users", t => t.user_id, cascadeDelete: true)
                .Index(t => t.user_id, name: "idx_user_id")
                .Index(t => t.role_id, name: "idx_role_id");
            
            CreateTable(
                "dbo.roles",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        comment = c.String(nullable: false, maxLength: 256, storeType: "nvarchar"),
                        deleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "False")
                                },
                            }),
                        created_at = c.DateTime(nullable: false, precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP")
                                },
                            }),
                        modified_at = c.DateTime(precision: 0,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValueSql",
                                    new AnnotationValues(oldValue: null, newValue: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP")
                                },
                            }),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.name, unique: true, name: "ui_name");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.application_logs", "user_id", "dbo.users");
            DropForeignKey("dbo.users_roles", "user_id", "dbo.users");
            DropForeignKey("dbo.users_roles", "role_id", "dbo.roles");
            DropForeignKey("dbo.signin_logs", "user_id", "dbo.users");
            DropForeignKey("dbo.sales_trends", "user_id", "dbo.users");
            DropForeignKey("dbo.trades", "product_id", "dbo.products");
            DropForeignKey("dbo.stocks", "product_id", "dbo.products");
            DropForeignKey("dbo.sales_trends", "product_id", "dbo.products");
            DropForeignKey("dbo.sales", "product_id", "dbo.products");
            DropForeignKey("dbo.sales", "office_id", "dbo.offices");
            DropForeignKey("dbo.orders", "product_id", "dbo.products");
            DropForeignKey("dbo.products", "old_product_id", "dbo.products");
            DropForeignKey("dbo.invoices", "product_id", "dbo.products");
            DropForeignKey("dbo.groups_products", "product_id", "dbo.products");
            DropForeignKey("dbo.users_makers", "user_id", "dbo.users");
            DropForeignKey("dbo.users_makers", "maker_id", "dbo.makers");
            DropForeignKey("dbo.products", "maker_id", "dbo.makers");
            DropForeignKey("dbo.groups", "maker_id", "dbo.makers");
            DropForeignKey("dbo.groups_products", "group_id", "dbo.groups");
            DropForeignKey("dbo.groups", "container_id", "dbo.containers");
            DropForeignKey("dbo.current_ctocks", "product_id", "dbo.products");
            DropIndex("dbo.roles", "ui_name");
            DropIndex("dbo.users_roles", "idx_role_id");
            DropIndex("dbo.users_roles", "idx_user_id");
            DropIndex("dbo.signin_logs", "idx_user_id");
            DropIndex("dbo.signin_logs", "idx_user_code");
            DropIndex("dbo.trades", "ui_product_id_detail_date");
            DropIndex("dbo.stocks", "ui_product_id_detail_date");
            DropIndex("dbo.offices", "ui_code");
            DropIndex("dbo.sales", "ui_sales");
            DropIndex("dbo.orders", "idx_product_id");
            DropIndex("dbo.invoices", "idx_product_id");
            DropIndex("dbo.users_makers", "idx_maker_id");
            DropIndex("dbo.users_makers", "idx_user_id");
            DropIndex("dbo.makers", "ui_code");
            DropIndex("dbo.groups", "idx_container_id");
            DropIndex("dbo.groups", "idx_maker_id");
            DropIndex("dbo.groups", "ui_code");
            DropIndex("dbo.groups_products", "idx_product_id");
            DropIndex("dbo.groups_products", "idx_group_id");
            DropIndex("dbo.current_ctocks", "idx_product_id");
            DropIndex("dbo.products", "idx_old_product_id");
            DropIndex("dbo.products", "idx_maker_id");
            DropIndex("dbo.products", "ui_code");
            DropIndex("dbo.sales_trends", "idx_user_id");
            DropIndex("dbo.sales_trends", "idx_product_id_detail_date");
            DropIndex("dbo.users", "ui_code");
            DropIndex("dbo.application_logs", "idx_user_id");
            DropTable("dbo.roles",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "created_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP" },
                        }
                    },
                    {
                        "deleted",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "False" },
                        }
                    },
                    {
                        "modified_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP" },
                        }
                    },
                });
            DropTable("dbo.users_roles",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "created_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP" },
                        }
                    },
                    {
                        "deleted",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "False" },
                        }
                    },
                    {
                        "modified_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP" },
                        }
                    },
                });
            DropTable("dbo.signin_logs",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "created_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP" },
                        }
                    },
                    {
                        "deleted",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "False" },
                        }
                    },
                    {
                        "modified_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP" },
                        }
                    },
                });
            DropTable("dbo.trades",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "adjustment_invoice_qty",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "0" },
                        }
                    },
                    {
                        "created_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP" },
                        }
                    },
                    {
                        "deleted",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "False" },
                        }
                    },
                    {
                        "invoice_plan_qty",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "0" },
                        }
                    },
                    {
                        "invoice_qty",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "0" },
                        }
                    },
                    {
                        "modified_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP" },
                        }
                    },
                    {
                        "orders_plan_qty",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "0" },
                        }
                    },
                    {
                        "orders_qty",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "0" },
                        }
                    },
                    {
                        "remaining_invoice_qty",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "0" },
                        }
                    },
                });
            DropTable("dbo.stocks",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "created_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP" },
                        }
                    },
                    {
                        "deleted",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "False" },
                        }
                    },
                    {
                        "modified_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP" },
                        }
                    },
                    {
                        "stocks",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "0" },
                        }
                    },
                });
            DropTable("dbo.offices",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "created_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP" },
                        }
                    },
                    {
                        "deleted",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "False" },
                        }
                    },
                    {
                        "modified_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP" },
                        }
                    },
                });
            DropTable("dbo.sales",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "actual",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "0" },
                        }
                    },
                    {
                        "created_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP" },
                        }
                    },
                    {
                        "deleted",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "False" },
                        }
                    },
                    {
                        "modified_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP" },
                        }
                    },
                    {
                        "plan",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "0" },
                        }
                    },
                });
            DropTable("dbo.orders",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "created_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP" },
                        }
                    },
                    {
                        "deleted",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "False" },
                        }
                    },
                    {
                        "modified_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP" },
                        }
                    },
                    {
                        "quantity",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "0" },
                        }
                    },
                });
            DropTable("dbo.invoices",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "created_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP" },
                        }
                    },
                    {
                        "deleted",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "False" },
                        }
                    },
                    {
                        "modified_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP" },
                        }
                    },
                    {
                        "quantity",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "0" },
                        }
                    },
                });
            DropTable("dbo.users_makers",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "created_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP" },
                        }
                    },
                    {
                        "deleted",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "False" },
                        }
                    },
                    {
                        "modified_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP" },
                        }
                    },
                });
            DropTable("dbo.makers",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "created_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP" },
                        }
                    },
                    {
                        "deleted",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "False" },
                        }
                    },
                    {
                        "enabled",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "True" },
                        }
                    },
                    {
                        "modified_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP" },
                        }
                    },
                });
            DropTable("dbo.containers",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "created_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP" },
                        }
                    },
                    {
                        "deleted",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "False" },
                        }
                    },
                    {
                        "modified_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP" },
                        }
                    },
                });
            DropTable("dbo.groups",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "capacity_20dry",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "0" },
                        }
                    },
                    {
                        "capacity_20reefer",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "0" },
                        }
                    },
                    {
                        "capacity_40dry",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "0" },
                        }
                    },
                    {
                        "capacity_40reefer",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "0" },
                        }
                    },
                    {
                        "created_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP" },
                        }
                    },
                    {
                        "deleted",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "False" },
                        }
                    },
                    {
                        "is_capacity",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "True" },
                        }
                    },
                    {
                        "modified_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP" },
                        }
                    },
                });
            DropTable("dbo.groups_products",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "created_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP" },
                        }
                    },
                    {
                        "deleted",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "False" },
                        }
                    },
                    {
                        "modified_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP" },
                        }
                    },
                });
            DropTable("dbo.current_ctocks",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "actual_qty",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "0" },
                        }
                    },
                    {
                        "created_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP" },
                        }
                    },
                    {
                        "deleted",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "False" },
                        }
                    },
                    {
                        "logical_qty",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "0" },
                        }
                    },
                    {
                        "modified_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP" },
                        }
                    },
                });
            DropTable("dbo.products",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "created_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP" },
                        }
                    },
                    {
                        "deleted",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "False" },
                        }
                    },
                    {
                        "enabled",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "True" },
                        }
                    },
                    {
                        "is_sold_weight",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "False" },
                        }
                    },
                    {
                        "modified_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP" },
                        }
                    },
                    {
                        "quantity",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "0" },
                        }
                    },
                });
            DropTable("dbo.sales_trends",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "created_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP" },
                        }
                    },
                    {
                        "deleted",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "False" },
                        }
                    },
                    {
                        "modified_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP" },
                        }
                    },
                    {
                        "quantity",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "0" },
                        }
                    },
                });
            DropTable("dbo.users",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "access_failed_count",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "0" },
                        }
                    },
                    {
                        "created_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP" },
                        }
                    },
                    {
                        "deleted",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "False" },
                        }
                    },
                    {
                        "email_confirmed",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "False" },
                        }
                    },
                    {
                        "enabled",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "False" },
                        }
                    },
                    {
                        "lockout_enabled",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "True" },
                        }
                    },
                    {
                        "modified_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP" },
                        }
                    },
                    {
                        "password_skip_count",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "0" },
                        }
                    },
                });
            DropTable("dbo.application_logs",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "created_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP" },
                        }
                    },
                    {
                        "deleted",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "False" },
                        }
                    },
                    {
                        "modified_at",
                        new Dictionary<string, object>
                        {
                            { "DefaultValueSql", "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP" },
                        }
                    },
                });
        }
    }
}
