namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GroupedOrderEvents : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CreationDate = c.DateTime(nullable: false),
                        CreateUserId = c.String(nullable: false, maxLength: 128),
                        EventType = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.CreateUserId)
                .Index(t => t.CreateUserId);
            
            CreateTable(
                "dbo.GroupedOrderEvents",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        GroupedOrder_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.Id)
                .ForeignKey("dbo.GroupedOrders", t => t.GroupedOrder_Id)
                .Index(t => t.Id)
                .Index(t => t.GroupedOrder_Id);
            
            CreateTable(
                "dbo.EventApplicationUsers",
                c => new
                    {
                        Event_Id = c.Guid(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Event_Id, t.ApplicationUser_Id })
                .ForeignKey("dbo.Events", t => t.Event_Id, cascadeDelete: true)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.Event_Id)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventApplicationUsers", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.EventApplicationUsers", "Event_Id", "dbo.Events");
            DropForeignKey("dbo.Events", "CreateUserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.GroupedOrderEvents", "GroupedOrder_Id", "dbo.GroupedOrders");
            DropForeignKey("dbo.GroupedOrderEvents", "Id", "dbo.Events");
            DropIndex("dbo.EventApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.EventApplicationUsers", new[] { "Event_Id" });
            DropIndex("dbo.GroupedOrderEvents", new[] { "GroupedOrder_Id" });
            DropIndex("dbo.GroupedOrderEvents", new[] { "Id" });
            DropIndex("dbo.Events", new[] { "CreateUserId" });
            DropTable("dbo.EventApplicationUsers");
            DropTable("dbo.GroupedOrderEvents");
            DropTable("dbo.Events");
        }
    }
}
