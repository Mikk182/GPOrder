namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BillGroupedorder_FK2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Bills", new[] { "Id" });
            CreateIndex("dbo.GroupedOrders", "Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.GroupedOrders", new[] { "Id" });
            CreateIndex("dbo.Bills", "Id");
        }
    }
}
