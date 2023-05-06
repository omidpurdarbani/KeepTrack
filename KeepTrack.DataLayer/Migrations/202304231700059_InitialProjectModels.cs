namespace KeepTrack.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialProjectModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        StateTypeId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        ProjectTypeId = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.StateTypes", t => t.StateTypeId, cascadeDelete: true)
                .ForeignKey("dbo.ProjectTypes", t => t.ProjectTypeId, cascadeDelete: true)
                .Index(t => t.StateTypeId)
                .Index(t => t.UserId)
                .Index(t => t.ProjectTypeId);
            
            CreateTable(
                "dbo.ProjectTasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ParentId = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                        StateTypeId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.StateTypes", t => t.StateTypeId)
                .Index(t => t.ProjectId)
                .Index(t => t.StateTypeId);
            
            CreateTable(
                "dbo.StateTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProjectTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "ProjectTypeId", "dbo.ProjectTypes");
            DropForeignKey("dbo.ProjectTasks", "StateTypeId", "dbo.StateTypes");
            DropForeignKey("dbo.Projects", "StateTypeId", "dbo.StateTypes");
            DropForeignKey("dbo.ProjectTasks", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Projects", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.ProjectTasks", new[] { "StateTypeId" });
            DropIndex("dbo.ProjectTasks", new[] { "ProjectId" });
            DropIndex("dbo.Projects", new[] { "ProjectTypeId" });
            DropIndex("dbo.Projects", new[] { "UserId" });
            DropIndex("dbo.Projects", new[] { "StateTypeId" });
            DropTable("dbo.ProjectTypes");
            DropTable("dbo.StateTypes");
            DropTable("dbo.ProjectTasks");
            DropTable("dbo.Projects");
        }
    }
}
