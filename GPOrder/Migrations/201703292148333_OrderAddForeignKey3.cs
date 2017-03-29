namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderAddForeignKey3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderLines", "Order_Id1", "dbo.Orders");
            DropIndex("dbo.OrderLines", new[] { "Order_Id1" });
            DropColumn("dbo.OrderLines", "Order_Id");
            RenameColumn(table: "dbo.OrderLines", name: "Order_Id1", newName: "Order_Id");
            AlterColumn("dbo.OrderLines", "Order_Id", c => c.Guid(nullable: false));
            CreateIndex("dbo.OrderLines", "Order_Id");
            AddForeignKey("dbo.OrderLines", "Order_Id", "dbo.Orders", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderLines", "Order_Id", "dbo.Orders");
            DropIndex("dbo.OrderLines", new[] { "Order_Id" });
            AlterColumn("dbo.OrderLines", "Order_Id", c => c.Guid());
            RenameColumn(table: "dbo.OrderLines", name: "Order_Id", newName: "Order_Id1");
            AddColumn("dbo.OrderLines", "Order_Id", c => c.Guid(nullable: false));
            CreateIndex("dbo.OrderLines", "Order_Id1");
            AddForeignKey("dbo.OrderLines", "Order_Id1", "dbo.Orders", "Id");
        }
    }
}
