namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderEntiersPositif : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.OrderLines", "OrderedQty");
            DropColumn("dbo.OrderLines", "BuyQty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderLines", "BuyQty", c => c.Int(nullable: false));
            AddColumn("dbo.OrderLines", "OrderedQty", c => c.Int(nullable: false));
        }
    }
}
