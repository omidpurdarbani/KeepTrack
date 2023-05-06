namespace KeepTrack.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateProjectTypeimage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectTypes", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectTypes", "Image");
        }
    }
}
