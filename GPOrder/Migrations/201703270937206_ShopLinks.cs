namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShopLinks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShopLinks",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Url = c.String(maxLength: 1024),
                        ShopId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shops", t => t.ShopId, cascadeDelete: true)
                .Index(t => t.ShopId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShopLinks", "ShopId", "dbo.Shops");
            DropIndex("dbo.ShopLinks", new[] { "ShopId" });
            DropTable("dbo.ShopLinks");
        }
    }
}
