namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderAddForeignKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GroupedOrders", "CreateUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Orders", "CreateUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.GroupedOrders", "DeliveryBoy_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.GroupedOrders", "LinkedShop_Id", "dbo.Shops");
            DropForeignKey("dbo.Orders", "GroupedOrder_Id", "dbo.GroupedOrders");
            DropForeignKey("dbo.OrderLines", "Order_Id", "dbo.Orders");
            DropIndex("dbo.GroupedOrders", new[] { "LinkedShop_Id" });
            DropIndex("dbo.GroupedOrders", new[] { "CreateUser_Id" });
            DropIndex("dbo.GroupedOrders", new[] { "DeliveryBoy_Id" });
            DropIndex("dbo.Orders", new[] { "CreateUser_Id" });
            DropIndex("dbo.Orders", new[] { "GroupedOrder_Id" });
            DropIndex("dbo.OrderLines", new[] { "Order_Id" });
            AddColumn("dbo.GroupedOrders", "CreateUser_Id1", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.GroupedOrders", "DeliveryBoy_Id1", c => c.String(maxLength: 128));
            AddColumn("dbo.GroupedOrders", "LinkedShop_Id1", c => c.Guid(nullable: false));
            AddColumn("dbo.Orders", "CreateUser_Id1", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Orders", "GroupedOrder_Id1", c => c.Guid(nullable: false));
            AddColumn("dbo.OrderLines", "Order_Id1", c => c.Guid());
            AlterColumn("dbo.GroupedOrders", "LinkedShop_Id", c => c.Guid());
            AlterColumn("dbo.GroupedOrders", "CreateUser_Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.GroupedOrders", "DeliveryBoy_Id", c => c.Guid());
            AlterColumn("dbo.Orders", "CreateUser_Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.OrderLines", "Order_Id", c => c.Guid(nullable: false));
            CreateIndex("dbo.GroupedOrders", "CreateUser_Id1");
            CreateIndex("dbo.GroupedOrders", "DeliveryBoy_Id1");
            CreateIndex("dbo.GroupedOrders", "LinkedShop_Id1");
            CreateIndex("dbo.Orders", "CreateUser_Id1");
            CreateIndex("dbo.Orders", "GroupedOrder_Id1");
            CreateIndex("dbo.OrderLines", "Order_Id1");
            AddForeignKey("dbo.GroupedOrders", "CreateUser_Id1", "dbo.ApplicationUsers", "Id");
            AddForeignKey("dbo.Orders", "CreateUser_Id1", "dbo.ApplicationUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.GroupedOrders", "DeliveryBoy_Id1", "dbo.ApplicationUsers", "Id");
            AddForeignKey("dbo.GroupedOrders", "LinkedShop_Id1", "dbo.Shops", "Id");
            AddForeignKey("dbo.Orders", "GroupedOrder_Id1", "dbo.GroupedOrders", "Id", cascadeDelete: true);
            AddForeignKey("dbo.OrderLines", "Order_Id1", "dbo.Orders", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderLines", "Order_Id1", "dbo.Orders");
            DropForeignKey("dbo.Orders", "GroupedOrder_Id1", "dbo.GroupedOrders");
            DropForeignKey("dbo.GroupedOrders", "LinkedShop_Id1", "dbo.Shops");
            DropForeignKey("dbo.GroupedOrders", "DeliveryBoy_Id1", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Orders", "CreateUser_Id1", "dbo.ApplicationUsers");
            DropForeignKey("dbo.GroupedOrders", "CreateUser_Id1", "dbo.ApplicationUsers");
            DropIndex("dbo.OrderLines", new[] { "Order_Id1" });
            DropIndex("dbo.Orders", new[] { "GroupedOrder_Id1" });
            DropIndex("dbo.Orders", new[] { "CreateUser_Id1" });
            DropIndex("dbo.GroupedOrders", new[] { "LinkedShop_Id1" });
            DropIndex("dbo.GroupedOrders", new[] { "DeliveryBoy_Id1" });
            DropIndex("dbo.GroupedOrders", new[] { "CreateUser_Id1" });
            AlterColumn("dbo.OrderLines", "Order_Id", c => c.Guid());
            AlterColumn("dbo.Orders", "CreateUser_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.GroupedOrders", "DeliveryBoy_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.GroupedOrders", "CreateUser_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.GroupedOrders", "LinkedShop_Id", c => c.Guid(nullable: false));
            DropColumn("dbo.OrderLines", "Order_Id1");
            DropColumn("dbo.Orders", "GroupedOrder_Id1");
            DropColumn("dbo.Orders", "CreateUser_Id1");
            DropColumn("dbo.GroupedOrders", "LinkedShop_Id1");
            DropColumn("dbo.GroupedOrders", "DeliveryBoy_Id1");
            DropColumn("dbo.GroupedOrders", "CreateUser_Id1");
            CreateIndex("dbo.OrderLines", "Order_Id");
            CreateIndex("dbo.Orders", "GroupedOrder_Id");
            CreateIndex("dbo.Orders", "CreateUser_Id");
            CreateIndex("dbo.GroupedOrders", "DeliveryBoy_Id");
            CreateIndex("dbo.GroupedOrders", "CreateUser_Id");
            CreateIndex("dbo.GroupedOrders", "LinkedShop_Id");
            AddForeignKey("dbo.OrderLines", "Order_Id", "dbo.Orders", "Id");
            AddForeignKey("dbo.Orders", "GroupedOrder_Id", "dbo.GroupedOrders", "Id", cascadeDelete: true);
            AddForeignKey("dbo.GroupedOrders", "LinkedShop_Id", "dbo.Shops", "Id");
            AddForeignKey("dbo.GroupedOrders", "DeliveryBoy_Id", "dbo.ApplicationUsers", "Id");
            AddForeignKey("dbo.Orders", "CreateUser_Id", "dbo.ApplicationUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.GroupedOrders", "CreateUser_Id", "dbo.ApplicationUsers", "Id");
        }
    }
}
