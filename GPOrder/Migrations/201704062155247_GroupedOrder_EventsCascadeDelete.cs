namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GroupedOrder_EventsCascadeDelete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GroupedOrderEvents", "GroupedOrderId", "dbo.GroupedOrders");
            AddForeignKey("dbo.GroupedOrderEvents", "GroupedOrderId", "dbo.GroupedOrders", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GroupedOrderEvents", "GroupedOrderId", "dbo.GroupedOrders");
            AddForeignKey("dbo.GroupedOrderEvents", "GroupedOrderId", "dbo.GroupedOrders", "Id");
        }
    }
}
