namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FilesToShopPictures : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ShopPictures", "Id", "dbo.Files");
            DropPrimaryKey("dbo.Files");
            AddColumn("dbo.ShopPictures", "LinkedFileId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Files", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Files", "Id");
            AddForeignKey("dbo.ShopPictures", "Id", "dbo.Files", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShopPictures", "Id", "dbo.Files");
            DropPrimaryKey("dbo.Files");
            AlterColumn("dbo.Files", "Id", c => c.Guid(nullable: false, identity: true));
            DropColumn("dbo.ShopPictures", "LinkedFileId");
            AddPrimaryKey("dbo.Files", "Id");
            AddForeignKey("dbo.ShopPictures", "Id", "dbo.Files", "Id");
        }
    }
}
