namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcultureandtimezoneinuseridentity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationUsers", "UiCulture", c => c.String());
            AddColumn("dbo.ApplicationUsers", "TimeZone", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationUsers", "TimeZone");
            DropColumn("dbo.ApplicationUsers", "UiCulture");
        }
    }
}
