namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixOrder : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OrderLines", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OrderLines", "Description", c => c.String(maxLength: 100));
        }
    }
}
