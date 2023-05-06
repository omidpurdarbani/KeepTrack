namespace KeepTrack.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateProjectTaskIdType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "RoleId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "RoleId", c => c.String());
        }
    }
}
