namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyOrders : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderLines", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.Orders", "User_Id", "dbo.ApplicationUsers");
            DropIndex("dbo.OrderLines", new[] { "Product_Id" });
            DropIndex("dbo.Orders", new[] { "User_Id" });
            RenameColumn(table: "dbo.Orders", name: "User_Id", newName: "CreateUser_Id");
            CreateTable(
                "dbo.GroupedOrders",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CreationDate = c.DateTime(nullable: false),
                        LimitDate = c.DateTime(),
                        IsLocked = c.Boolean(nullable: false),
                        LinkedShop_Id = c.Guid(nullable: false),
                        CreateUser_Id = c.String(nullable: false, maxLength: 128),
                        DeliveryBoy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shops", t => t.LinkedShop_Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.CreateUser_Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.DeliveryBoy_Id)
                .Index(t => t.LinkedShop_Id)
                .Index(t => t.CreateUser_Id)
                .Index(t => t.DeliveryBoy_Id);
            
            AddColumn("dbo.OrderLines", "Description", c => c.String(maxLength: 100));
            AddColumn("dbo.Orders", "CreationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "EstimatedPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Orders", "RealPrice", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Orders", "GroupedOrder_Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.Orders", "CreateUser_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Orders", "CreateUser_Id");
            CreateIndex("dbo.Orders", "GroupedOrder_Id");
            AddForeignKey("dbo.Orders", "GroupedOrder_Id", "dbo.GroupedOrders", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Orders", "CreateUser_Id", "dbo.ApplicationUsers", "Id", cascadeDelete: true);
            DropColumn("dbo.OrderLines", "OrderedQty");
            DropColumn("dbo.OrderLines", "BuyQty");
            DropColumn("dbo.OrderLines", "Product_Id");
            DropColumn("dbo.Orders", "Date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.OrderLines", "Product_Id", c => c.Guid());
            AddColumn("dbo.OrderLines", "BuyQty", c => c.Int(nullable: false));
            AddColumn("dbo.OrderLines", "OrderedQty", c => c.Int(nullable: false));
            DropForeignKey("dbo.Orders", "CreateUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.GroupedOrders", "DeliveryBoy_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.GroupedOrders", "CreateUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.GroupedOrders", "LinkedShop_Id", "dbo.Shops");
            DropForeignKey("dbo.Orders", "GroupedOrder_Id", "dbo.GroupedOrders");
            DropIndex("dbo.Orders", new[] { "GroupedOrder_Id" });
            DropIndex("dbo.Orders", new[] { "CreateUser_Id" });
            DropIndex("dbo.GroupedOrders", new[] { "DeliveryBoy_Id" });
            DropIndex("dbo.GroupedOrders", new[] { "CreateUser_Id" });
            DropIndex("dbo.GroupedOrders", new[] { "LinkedShop_Id" });
            AlterColumn("dbo.Orders", "CreateUser_Id", c => c.String(maxLength: 128));
            DropColumn("dbo.Orders", "GroupedOrder_Id");
            DropColumn("dbo.Orders", "RealPrice");
            DropColumn("dbo.Orders", "EstimatedPrice");
            DropColumn("dbo.Orders", "CreationDate");
            DropColumn("dbo.OrderLines", "Description");
            DropTable("dbo.GroupedOrders");
            RenameColumn(table: "dbo.Orders", name: "CreateUser_Id", newName: "User_Id");
            CreateIndex("dbo.Orders", "User_Id");
            CreateIndex("dbo.OrderLines", "Product_Id");
            AddForeignKey("dbo.Orders", "User_Id", "dbo.ApplicationUsers", "Id");
            AddForeignKey("dbo.OrderLines", "Product_Id", "dbo.Products", "Id");
        }
    }
}
