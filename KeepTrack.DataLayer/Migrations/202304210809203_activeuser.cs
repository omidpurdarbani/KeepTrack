﻿namespace KeepTrack.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class activeuser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Active");
        }
    }
}
