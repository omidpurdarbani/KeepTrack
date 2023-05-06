namespace KeepTrack.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userroleid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "RoleId", c => c.String());
            DropColumn("dbo.AspNetUsers", "RoleName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "RoleName", c => c.String());
            DropColumn("dbo.AspNetUsers", "RoleId");
        }
    }
}
