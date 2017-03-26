namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FilesToShopPictures2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ShopPictures", "Id", "dbo.Files");
            DropPrimaryKey("dbo.Files");
            AddColumn("dbo.Files", "ShopPictureId", c => c.Guid());
            AlterColumn("dbo.Files", "Id", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.Files", "Id");
            AddForeignKey("dbo.ShopPictures", "Id", "dbo.Files", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShopPictures", "Id", "dbo.Files");
            DropPrimaryKey("dbo.Files");
            AlterColumn("dbo.Files", "Id", c => c.Guid(nullable: false));
            DropColumn("dbo.Files", "ShopPictureId");
            AddPrimaryKey("dbo.Files", "Id");
            AddForeignKey("dbo.ShopPictures", "Id", "dbo.Files", "Id");
        }
    }
}
