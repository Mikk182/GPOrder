namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGroups : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CreationDate = c.DateTime(nullable: false),
                        CreateUserId = c.String(nullable: false, maxLength: 128),
                        OwnerUserId = c.String(nullable: false, maxLength: 128),
                        IsLocked = c.Boolean(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.CreateUserId)
                .ForeignKey("dbo.ApplicationUsers", t => t.OwnerUserId)
                .Index(t => t.CreateUserId)
                .Index(t => t.OwnerUserId);
            
            AddColumn("dbo.ApplicationUsers", "Group_Id", c => c.Guid());
            CreateIndex("dbo.ApplicationUsers", "Group_Id");
            AddForeignKey("dbo.ApplicationUsers", "Group_Id", "dbo.Groups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Groups", "OwnerUserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Groups", "CreateUserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUsers", "Group_Id", "dbo.Groups");
            DropIndex("dbo.ApplicationUsers", new[] { "Group_Id" });
            DropIndex("dbo.Groups", new[] { "OwnerUserId" });
            DropIndex("dbo.Groups", new[] { "CreateUserId" });
            DropColumn("dbo.ApplicationUsers", "Group_Id");
            DropTable("dbo.Groups");
        }
    }
}
