namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeparateFilesFromShopPictures : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ShopPictures", "Shop_Id", "dbo.Shops");
            DropIndex("dbo.ShopPictures", new[] { "Shop_Id" });
            RenameColumn(table: "dbo.ShopPictures", name: "Shop_Id", newName: "ShopId");
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ContentType = c.String(maxLength: 100),
                        Content = c.Binary(),
                        FileType = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            AlterColumn("dbo.ShopPictures", "ShopId", c => c.Guid(nullable: false));
            CreateIndex("dbo.ShopPictures", "Id");
            CreateIndex("dbo.ShopPictures", "ShopId");
            AddForeignKey("dbo.ShopPictures", "Id", "dbo.Files", "Id");
            AddForeignKey("dbo.ShopPictures", "ShopId", "dbo.Shops", "Id", cascadeDelete: true);
            DropColumn("dbo.ShopPictures", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ShopPictures", "Image", c => c.Binary());
            DropForeignKey("dbo.ShopPictures", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.Files", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ShopPictures", "Id", "dbo.Files");
            DropIndex("dbo.Files", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ShopPictures", new[] { "ShopId" });
            DropIndex("dbo.ShopPictures", new[] { "Id" });
            AlterColumn("dbo.ShopPictures", "ShopId", c => c.Guid());
            DropTable("dbo.Files");
            RenameColumn(table: "dbo.ShopPictures", name: "ShopId", newName: "Shop_Id");
            CreateIndex("dbo.ShopPictures", "Shop_Id");
            AddForeignKey("dbo.ShopPictures", "Shop_Id", "dbo.Shops", "Id");
        }
    }
}
