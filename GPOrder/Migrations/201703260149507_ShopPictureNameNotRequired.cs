namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShopPictureNameNotRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ShopPictures", "Name", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ShopPictures", "Name", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
