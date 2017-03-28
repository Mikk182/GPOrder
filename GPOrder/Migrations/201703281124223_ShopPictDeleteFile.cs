namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShopPictDeleteFile : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ShopPictures", "Id", "dbo.Files");
            AddForeignKey("dbo.ShopPictures", "Id", "dbo.Files", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShopPictures", "Id", "dbo.Files");
            AddForeignKey("dbo.ShopPictures", "Id", "dbo.Files", "Id");
        }
    }
}
