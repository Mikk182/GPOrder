namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GroupedOrderEventlimitDateNullable1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GroupedOrderEvents", "LimitDateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GroupedOrderEvents", "LimitDateTime", c => c.DateTime());
        }
    }
}
