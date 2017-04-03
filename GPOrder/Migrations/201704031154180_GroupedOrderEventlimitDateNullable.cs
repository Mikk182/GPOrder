namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GroupedOrderEventlimitDateNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GroupedOrderEvents", "LimitDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GroupedOrderEvents", "LimitDateTime", c => c.DateTime(nullable: false));
        }
    }
}
