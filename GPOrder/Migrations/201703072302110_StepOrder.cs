namespace GPOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StepOrder : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RoleUsers", "Role_RoleId", "dbo.Roles");
            DropForeignKey("dbo.RoleUsers", "User_UserId", "dbo.Users");

            // Added manually
            DropForeignKey("dbo.Orders", "User_UserId", "dbo.Users");

            DropIndex("dbo.Orders", new[] { "User_UserId" });
            DropIndex("dbo.RoleUsers", new[] { "Role_RoleId" });
            DropIndex("dbo.RoleUsers", new[] { "User_UserId" });
            RenameColumn(table: "dbo.Orders", name: "User_UserId", newName: "User_Id");
            AlterColumn("dbo.Orders", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Orders", "User_Id");
            DropColumn("dbo.OrderLines", "Weight");
            DropTable("dbo.Users");
            DropTable("dbo.Roles");
            DropTable("dbo.RoleUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RoleUsers",
                c => new
                    {
                        Role_RoleId = c.Guid(nullable: false),
                        User_UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_RoleId, t.User_UserId });
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleId = c.Guid(nullable: false),
                        RoleName = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        Username = c.String(nullable: false),
                        Email = c.String(),
                        Password = c.String(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Comment = c.String(),
                        IsApproved = c.Boolean(nullable: false),
                        PasswordFailuresSinceLastSuccess = c.Int(nullable: false),
                        LastPasswordFailureDate = c.DateTime(),
                        LastActivityDate = c.DateTime(),
                        LastLockoutDate = c.DateTime(),
                        LastLoginDate = c.DateTime(),
                        ConfirmationToken = c.String(),
                        CreateDate = c.DateTime(),
                        IsLockedOut = c.Boolean(nullable: false),
                        LastPasswordChangedDate = c.DateTime(),
                        PasswordVerificationToken = c.String(),
                        PasswordVerificationTokenExpirationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserId);
            
            AddColumn("dbo.OrderLines", "Weight", c => c.Decimal(precision: 18, scale: 2));
            DropIndex("dbo.Orders", new[] { "User_Id" });
            AlterColumn("dbo.Orders", "User_Id", c => c.Guid());
            RenameColumn(table: "dbo.Orders", name: "User_Id", newName: "User_UserId");
            CreateIndex("dbo.RoleUsers", "User_UserId");
            CreateIndex("dbo.RoleUsers", "Role_RoleId");
            CreateIndex("dbo.Orders", "User_UserId");
            AddForeignKey("dbo.RoleUsers", "User_UserId", "dbo.Users", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.RoleUsers", "Role_RoleId", "dbo.Roles", "RoleId", cascadeDelete: true);
        }
    }
}
