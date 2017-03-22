namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImplementationShops : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Shops",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CreationDate = c.DateTime(nullable: false),
                        CreateUserId = c.String(nullable: false, maxLength: 128),
                        OwnerUserId = c.String(nullable: false, maxLength: 128),
                        IsLocked = c.Boolean(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Adress = c.String(nullable: false, maxLength: 100),
                        PhoneNumber = c.String(nullable: false, maxLength: 100),
                        Mail = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.CreateUserId)
                .ForeignKey("dbo.ApplicationUsers", t => t.OwnerUserId)
                .Index(t => t.CreateUserId)
                .Index(t => t.OwnerUserId);
            
            CreateTable(
                "dbo.ShopPictures",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CreationDate = c.DateTime(nullable: false),
                        CreateUserId = c.String(nullable: false, maxLength: 128),
                        IsLocked = c.Boolean(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Image = c.Binary(),
                        Shop_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.CreateUserId)
                .ForeignKey("dbo.Shops", t => t.Shop_Id)
                .Index(t => t.CreateUserId)
                .Index(t => t.Shop_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShopPictures", "Shop_Id", "dbo.Shops");
            DropForeignKey("dbo.ShopPictures", "CreateUserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Shops", "OwnerUserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Shops", "CreateUserId", "dbo.ApplicationUsers");
            DropIndex("dbo.ShopPictures", new[] { "Shop_Id" });
            DropIndex("dbo.ShopPictures", new[] { "CreateUserId" });
            DropIndex("dbo.Shops", new[] { "OwnerUserId" });
            DropIndex("dbo.Shops", new[] { "CreateUserId" });
            DropTable("dbo.ShopPictures");
            DropTable("dbo.Shops");
        }
    }
}
