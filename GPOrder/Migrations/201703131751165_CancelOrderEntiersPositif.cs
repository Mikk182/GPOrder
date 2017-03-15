namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CancelOrderEntiersPositif : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderLines", "OrderedQty", c => c.Int(nullable: false));
            AddColumn("dbo.OrderLines", "BuyQty", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderLines", "BuyQty");
            DropColumn("dbo.OrderLines", "OrderedQty");
        }
    }
}
