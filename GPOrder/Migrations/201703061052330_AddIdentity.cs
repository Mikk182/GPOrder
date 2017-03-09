namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIdentity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderLines", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.OrderLines", "Order_Id", "dbo.Orders");
            DropPrimaryKey("dbo.Products");
            DropPrimaryKey("dbo.Orders");
            DropPrimaryKey("dbo.OrderLines");
            AddColumn("dbo.OrderLines", "Order_Id", c => c.Guid());
            AlterColumn("dbo.Products", "Id", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"));
            AlterColumn("dbo.Orders", "Id", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"));
            AlterColumn("dbo.OrderLines", "Id", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"));
            AddPrimaryKey("dbo.Products", "Id");
            AddPrimaryKey("dbo.Orders", "Id");
            AddPrimaryKey("dbo.OrderLines", "Id");
            CreateIndex("dbo.OrderLines", "Order_Id");
            AddForeignKey("dbo.OrderLines", "Order_Id", "dbo.Orders", "Id");
            AddForeignKey("dbo.OrderLines", "Product_Id", "dbo.Products", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderLines", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.OrderLines", "Order_Id", "dbo.Orders");
            DropIndex("dbo.OrderLines", new[] { "Order_Id" });
            DropPrimaryKey("dbo.OrderLines");
            DropPrimaryKey("dbo.Orders");
            DropPrimaryKey("dbo.Products");
            AlterColumn("dbo.OrderLines", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.Orders", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.Products", "Id", c => c.Guid(nullable: false));
            DropColumn("dbo.OrderLines", "Order_Id");
            AddPrimaryKey("dbo.OrderLines", "Id");
            AddPrimaryKey("dbo.Orders", "Id");
            AddPrimaryKey("dbo.Products", "Id");
            AddForeignKey("dbo.OrderLines", "Product_Id", "dbo.Products", "Id");
        }
    }
}
