namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BillGroupedorder_FK4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bills", "GroupedOrder_Id", "dbo.GroupedOrders");
            DropIndex("dbo.Bills", new[] { "GroupedOrder_Id" });
            AddColumn("dbo.Bills", "GroupedOrder_Id1", c => c.Guid());
            AlterColumn("dbo.Bills", "GroupedOrder_Id", c => c.Guid(nullable: false));
            CreateIndex("dbo.Bills", "GroupedOrder_Id1");
            AddForeignKey("dbo.Bills", "GroupedOrder_Id1", "dbo.GroupedOrders", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bills", "GroupedOrder_Id1", "dbo.GroupedOrders");
            DropIndex("dbo.Bills", new[] { "GroupedOrder_Id1" });
            AlterColumn("dbo.Bills", "GroupedOrder_Id", c => c.Guid());
            DropColumn("dbo.Bills", "GroupedOrder_Id1");
            CreateIndex("dbo.Bills", "GroupedOrder_Id");
            AddForeignKey("dbo.Bills", "GroupedOrder_Id", "dbo.GroupedOrders", "Id");
        }
    }
}
