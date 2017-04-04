namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SpecializedEvents : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GroupedOrderEventsAskDeliveryBoy",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        LimitDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GroupedOrderEvents", t => t.Id)
                .Index(t => t.Id);
            
            DropColumn("dbo.GroupedOrderEvents", "LimitDateTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GroupedOrderEvents", "LimitDateTime", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.GroupedOrderEventsAskDeliveryBoy", "Id", "dbo.GroupedOrderEvents");
            DropIndex("dbo.GroupedOrderEventsAskDeliveryBoy", new[] { "Id" });
            DropTable("dbo.GroupedOrderEventsAskDeliveryBoy");
        }
    }
}
