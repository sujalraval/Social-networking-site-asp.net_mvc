namespace WebApplication7.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nineteen : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "VideoPath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "VideoPath");
        }
    }
}
