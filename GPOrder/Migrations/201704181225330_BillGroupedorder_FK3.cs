namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BillGroupedorder_FK3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GroupedOrders", "Id", "dbo.Bills");
            DropForeignKey("dbo.BillPictures", "Bill_Id", "dbo.Bills");
            DropForeignKey("dbo.BillEvents", "Bill_Id", "dbo.Bills");
            DropIndex("dbo.GroupedOrders", new[] { "Id" });
            DropPrimaryKey("dbo.Bills");
            AddColumn("dbo.Bills", "GroupedOrder_Id", c => c.Guid());
            AlterColumn("dbo.Bills", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Bills", "Id");
            CreateIndex("dbo.Bills", "GroupedOrder_Id");
            AddForeignKey("dbo.Bills", "GroupedOrder_Id", "dbo.GroupedOrders", "Id");
            AddForeignKey("dbo.BillPictures", "Bill_Id", "dbo.Bills", "Id", cascadeDelete: true);
            AddForeignKey("dbo.BillEvents", "Bill_Id", "dbo.Bills", "Id", cascadeDelete: true);
            DropColumn("dbo.Bills", "GroupedOrderId");
            DropColumn("dbo.GroupedOrders", "LinkedBillId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GroupedOrders", "LinkedBillId", c => c.Guid());
            AddColumn("dbo.Bills", "GroupedOrderId", c => c.Guid(nullable: false));
            DropForeignKey("dbo.BillEvents", "Bill_Id", "dbo.Bills");
            DropForeignKey("dbo.BillPictures", "Bill_Id", "dbo.Bills");
            DropForeignKey("dbo.Bills", "GroupedOrder_Id", "dbo.GroupedOrders");
            DropIndex("dbo.Bills", new[] { "GroupedOrder_Id" });
            DropPrimaryKey("dbo.Bills");
            AlterColumn("dbo.Bills", "Id", c => c.Guid(nullable: false, identity: true));
            DropColumn("dbo.Bills", "GroupedOrder_Id");
            AddPrimaryKey("dbo.Bills", "Id");
            CreateIndex("dbo.GroupedOrders", "Id");
            AddForeignKey("dbo.BillEvents", "Bill_Id", "dbo.Bills", "Id", cascadeDelete: true);
            AddForeignKey("dbo.BillPictures", "Bill_Id", "dbo.Bills", "Id", cascadeDelete: true);
            AddForeignKey("dbo.GroupedOrders", "Id", "dbo.Bills", "Id");
        }
    }
}
