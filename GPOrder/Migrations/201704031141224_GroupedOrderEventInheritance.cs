namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GroupedOrderEventInheritance : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.GroupedOrderEvents", name: "GroupedOrder_Id", newName: "GroupedOrderId");
            RenameIndex(table: "dbo.GroupedOrderEvents", name: "IX_GroupedOrder_Id", newName: "IX_GroupedOrderId");
            AddColumn("dbo.GroupedOrderEvents", "EventStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GroupedOrderEvents", "EventStatus");
            RenameIndex(table: "dbo.GroupedOrderEvents", name: "IX_GroupedOrderId", newName: "IX_GroupedOrder_Id");
            RenameColumn(table: "dbo.GroupedOrderEvents", name: "GroupedOrderId", newName: "GroupedOrder_Id");
        }
    }
}
