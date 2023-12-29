namespace WebApplication7.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fourteen : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "DateTime", c => c.DateTime(nullable: false));
            //DropColumn("dbo.Messages", "Timestamp");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.Messages", "Timestamp", c => c.DateTime(nullable: false));
            //DropColumn("dbo.Messages", "DateTime");
        }
    }
}
