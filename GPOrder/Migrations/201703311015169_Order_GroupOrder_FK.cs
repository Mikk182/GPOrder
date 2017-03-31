namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Order_GroupOrder_FK : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Orders", "GroupedOrder_Id");
            RenameColumn(table: "dbo.Orders", name: "GroupedOrder_Id1", newName: "GroupedOrder_Id");
            RenameIndex(table: "dbo.Orders", name: "IX_GroupedOrder_Id1", newName: "IX_GroupedOrder_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Orders", name: "IX_GroupedOrder_Id", newName: "IX_GroupedOrder_Id1");
            RenameColumn(table: "dbo.Orders", name: "GroupedOrder_Id", newName: "GroupedOrder_Id1");
            AddColumn("dbo.Orders", "GroupedOrder_Id", c => c.Guid(nullable: false));
        }
    }
}
