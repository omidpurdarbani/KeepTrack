namespace KeepTrack.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateProjectTypeDataName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectTypes", "DataName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectTypes", "DataName");
        }
    }
}
