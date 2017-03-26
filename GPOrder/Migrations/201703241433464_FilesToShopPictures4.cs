namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FilesToShopPictures4 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Files", "ShopPictureId");
            DropColumn("dbo.ShopPictures", "LinkedFileId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ShopPictures", "LinkedFileId", c => c.Guid(nullable: false));
            AddColumn("dbo.Files", "ShopPictureId", c => c.Guid());
        }
    }
}
