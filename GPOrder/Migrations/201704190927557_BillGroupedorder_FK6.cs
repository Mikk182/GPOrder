namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BillGroupedorder_FK6 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Bills", new[] { "GroupedOrder_Id1" });
            DropColumn("dbo.Bills", "GroupedOrder_Id");
            RenameColumn(table: "dbo.Bills", name: "GroupedOrder_Id1", newName: "GroupedOrder_Id");
            AlterColumn("dbo.Bills", "GroupedOrder_Id", c => c.Guid());
            CreateIndex("dbo.Bills", "GroupedOrder_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Bills", new[] { "GroupedOrder_Id" });
            AlterColumn("dbo.Bills", "GroupedOrder_Id", c => c.Guid(nullable: false));
            RenameColumn(table: "dbo.Bills", name: "GroupedOrder_Id", newName: "GroupedOrder_Id1");
            AddColumn("dbo.Bills", "GroupedOrder_Id", c => c.Guid(nullable: false));
            CreateIndex("dbo.Bills", "GroupedOrder_Id1");
        }
    }
}
