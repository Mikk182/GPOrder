namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BillPicture : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.BillPictures", new[] { "Id" });
            DropPrimaryKey("dbo.BillPictures");
            AlterColumn("dbo.BillPictures", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.BillPictures", "Id");
            CreateIndex("dbo.BillPictures", "Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.BillPictures", new[] { "Id" });
            DropPrimaryKey("dbo.BillPictures");
            AlterColumn("dbo.BillPictures", "Id", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.BillPictures", "Id");
            CreateIndex("dbo.BillPictures", "Id");
        }
    }
}
