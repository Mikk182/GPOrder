namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Bills : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BillPictures",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CreationDate = c.DateTime(nullable: false),
                        CreateUser_Id = c.String(nullable: false, maxLength: 128),
                        IsLocked = c.Boolean(nullable: false),
                        Name = c.String(maxLength: 100),
                        Bill_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Files", t => t.Id, cascadeDelete: true)
                .ForeignKey("dbo.Bills", t => t.Bill_Id, cascadeDelete: true)
                .ForeignKey("dbo.ApplicationUsers", t => t.CreateUser_Id)
                .Index(t => t.Id)
                .Index(t => t.CreateUser_Id)
                .Index(t => t.Bill_Id);
            
            CreateTable(
                "dbo.Bills",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CreateUser_Id = c.String(nullable: false, maxLength: 128),
                        GroupedOrder_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GroupedOrders", t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.CreateUser_Id)
                .Index(t => t.Id)
                .Index(t => t.CreateUser_Id);
            
            CreateTable(
                "dbo.BillEvents",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DebitUser_Id = c.String(nullable: false, maxLength: 128),
                        CreditUser_Id = c.String(nullable: false, maxLength: 128),
                        Order_Id = c.Guid(nullable: false),
                        Bill_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.DebitUser_Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.CreditUser_Id)
                .ForeignKey("dbo.Orders", t => t.Order_Id, cascadeDelete: true)
                .ForeignKey("dbo.Bills", t => t.Bill_Id, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.DebitUser_Id)
                .Index(t => t.CreditUser_Id)
                .Index(t => t.Order_Id)
                .Index(t => t.Bill_Id);
            
            AddColumn("dbo.GroupedOrders", "LinkedBill_Id", c => c.Guid());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BillEvents", "Bill_Id", "dbo.Bills");
            DropForeignKey("dbo.BillEvents", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.BillEvents", "CreditUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.BillEvents", "DebitUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.BillEvents", "Id", "dbo.Events");
            DropForeignKey("dbo.Bills", "CreateUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.BillPictures", "CreateUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.BillPictures", "Bill_Id", "dbo.Bills");
            DropForeignKey("dbo.BillPictures", "Id", "dbo.Files");
            DropForeignKey("dbo.Bills", "Id", "dbo.GroupedOrders");
            DropIndex("dbo.BillEvents", new[] { "Bill_Id" });
            DropIndex("dbo.BillEvents", new[] { "Order_Id" });
            DropIndex("dbo.BillEvents", new[] { "CreditUser_Id" });
            DropIndex("dbo.BillEvents", new[] { "DebitUser_Id" });
            DropIndex("dbo.BillEvents", new[] { "Id" });
            DropIndex("dbo.Bills", new[] { "CreateUser_Id" });
            DropIndex("dbo.Bills", new[] { "Id" });
            DropIndex("dbo.BillPictures", new[] { "Bill_Id" });
            DropIndex("dbo.BillPictures", new[] { "CreateUser_Id" });
            DropIndex("dbo.BillPictures", new[] { "Id" });
            DropColumn("dbo.GroupedOrders", "LinkedBill_Id");
            DropTable("dbo.BillEvents");
            DropTable("dbo.Bills");
            DropTable("dbo.BillPictures");
        }
    }
}
