namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BillGroupedorder_FK : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bills", "GroupedOrderId", c => c.Guid(nullable: false));
            AddColumn("dbo.GroupedOrders", "LinkedBillId", c => c.Guid());
            DropColumn("dbo.Bills", "GroupedOrder_Id");
            DropColumn("dbo.GroupedOrders", "LinkedBill_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GroupedOrders", "LinkedBill_Id", c => c.Guid());
            AddColumn("dbo.Bills", "GroupedOrder_Id", c => c.Guid(nullable: false));
            DropColumn("dbo.GroupedOrders", "LinkedBillId");
            DropColumn("dbo.Bills", "GroupedOrderId");
        }
    }
}
