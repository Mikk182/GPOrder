namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FilesToShopPictures3 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ShopPictures", new[] { "Id" });
            DropPrimaryKey("dbo.ShopPictures");
            AlterColumn("dbo.ShopPictures", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.ShopPictures", "Id");
            CreateIndex("dbo.ShopPictures", "Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ShopPictures", new[] { "Id" });
            DropPrimaryKey("dbo.ShopPictures");
            AlterColumn("dbo.ShopPictures", "Id", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.ShopPictures", "Id");
            CreateIndex("dbo.ShopPictures", "Id");
        }
    }
}
