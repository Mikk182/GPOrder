namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderAddForeignKey2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.GroupedOrders", new[] { "CreateUser_Id1" });
            DropIndex("dbo.GroupedOrders", new[] { "DeliveryBoy_Id1" });
            DropIndex("dbo.GroupedOrders", new[] { "LinkedShop_Id1" });
            DropIndex("dbo.Orders", new[] { "CreateUser_Id1" });
            DropColumn("dbo.GroupedOrders", "CreateUser_Id");
            DropColumn("dbo.GroupedOrders", "DeliveryBoy_Id");
            DropColumn("dbo.GroupedOrders", "LinkedShop_Id");
            DropColumn("dbo.Orders", "CreateUser_Id");
            RenameColumn(table: "dbo.GroupedOrders", name: "CreateUser_Id1", newName: "CreateUser_Id");
            RenameColumn(table: "dbo.Orders", name: "CreateUser_Id1", newName: "CreateUser_Id");
            RenameColumn(table: "dbo.GroupedOrders", name: "DeliveryBoy_Id1", newName: "DeliveryBoy_Id");
            RenameColumn(table: "dbo.GroupedOrders", name: "LinkedShop_Id1", newName: "LinkedShop_Id");
            AlterColumn("dbo.GroupedOrders", "CreateUser_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.GroupedOrders", "DeliveryBoy_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.GroupedOrders", "LinkedShop_Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.Orders", "CreateUser_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.GroupedOrders", "CreateUser_Id");
            CreateIndex("dbo.GroupedOrders", "DeliveryBoy_Id");
            CreateIndex("dbo.GroupedOrders", "LinkedShop_Id");
            CreateIndex("dbo.Orders", "CreateUser_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Orders", new[] { "CreateUser_Id" });
            DropIndex("dbo.GroupedOrders", new[] { "LinkedShop_Id" });
            DropIndex("dbo.GroupedOrders", new[] { "DeliveryBoy_Id" });
            DropIndex("dbo.GroupedOrders", new[] { "CreateUser_Id" });
            AlterColumn("dbo.Orders", "CreateUser_Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.GroupedOrders", "LinkedShop_Id", c => c.Guid());
            AlterColumn("dbo.GroupedOrders", "DeliveryBoy_Id", c => c.Guid());
            AlterColumn("dbo.GroupedOrders", "CreateUser_Id", c => c.Guid(nullable: false));
            RenameColumn(table: "dbo.GroupedOrders", name: "LinkedShop_Id", newName: "LinkedShop_Id1");
            RenameColumn(table: "dbo.GroupedOrders", name: "DeliveryBoy_Id", newName: "DeliveryBoy_Id1");
            RenameColumn(table: "dbo.Orders", name: "CreateUser_Id", newName: "CreateUser_Id1");
            RenameColumn(table: "dbo.GroupedOrders", name: "CreateUser_Id", newName: "CreateUser_Id1");
            AddColumn("dbo.Orders", "CreateUser_Id", c => c.Guid(nullable: false));
            AddColumn("dbo.GroupedOrders", "LinkedShop_Id", c => c.Guid());
            AddColumn("dbo.GroupedOrders", "DeliveryBoy_Id", c => c.Guid());
            AddColumn("dbo.GroupedOrders", "CreateUser_Id", c => c.Guid(nullable: false));
            CreateIndex("dbo.Orders", "CreateUser_Id1");
            CreateIndex("dbo.GroupedOrders", "LinkedShop_Id1");
            CreateIndex("dbo.GroupedOrders", "DeliveryBoy_Id1");
            CreateIndex("dbo.GroupedOrders", "CreateUser_Id1");
        }
    }
}
