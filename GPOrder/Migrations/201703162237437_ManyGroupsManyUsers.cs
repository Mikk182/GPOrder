namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManyGroupsManyUsers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationUsers", "Group_Id", "dbo.Groups");
            DropIndex("dbo.ApplicationUsers", new[] { "Group_Id" });
            CreateTable(
                "dbo.GroupApplicationUsers",
                c => new
                    {
                        Group_Id = c.Guid(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Group_Id, t.ApplicationUser_Id })
                .ForeignKey("dbo.Groups", t => t.Group_Id, cascadeDelete: true)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.Group_Id)
                .Index(t => t.ApplicationUser_Id);
            
            DropColumn("dbo.ApplicationUsers", "Group_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApplicationUsers", "Group_Id", c => c.Guid());
            DropForeignKey("dbo.GroupApplicationUsers", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.GroupApplicationUsers", "Group_Id", "dbo.Groups");
            DropIndex("dbo.GroupApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.GroupApplicationUsers", new[] { "Group_Id" });
            DropTable("dbo.GroupApplicationUsers");
            CreateIndex("dbo.ApplicationUsers", "Group_Id");
            AddForeignKey("dbo.ApplicationUsers", "Group_Id", "dbo.Groups", "Id");
        }
    }
}
