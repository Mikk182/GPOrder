namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GroupedOrderEvents_LimitDateTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GroupedOrderEvents", "LimitDateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GroupedOrderEvents", "LimitDateTime");
        }
    }
}
