namespace WebApplication7.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seven : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Follows",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TargetUserId = c.String(maxLength: 128),
                        FollowerUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.FollowerUserId)
                .ForeignKey("dbo.AspNetUsers", t => t.TargetUserId)
                .Index(t => t.TargetUserId)
                .Index(t => t.FollowerUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Follows", "TargetUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Follows", "FollowerUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Follows", new[] { "FollowerUserId" });
            DropIndex("dbo.Follows", new[] { "TargetUserId" });
            DropTable("dbo.Follows");
        }
    }
}
