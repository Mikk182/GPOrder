namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShopsRemoveRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Shops", "PhoneNumber", c => c.String(maxLength: 100));
            AlterColumn("dbo.Shops", "Mail", c => c.String(maxLength: 100));
            AlterColumn("dbo.Shops", "Description", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Shops", "Description", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Shops", "Mail", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Shops", "PhoneNumber", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
