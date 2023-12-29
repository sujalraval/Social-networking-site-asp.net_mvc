namespace WebApplication7.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nine : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Timestamp = c.DateTime(nullable: false),
                        Type = c.Int(nullable: false),
                        PostId = c.Int(),
                        CommentId = c.Int(),
                        LikedUserId = c.Int(),
                        UnlikedUserId = c.Int(),
                        UnfollowedUserId = c.Int(),
                        ViewedUserId = c.Int(),
                        LikedUser_Id = c.String(maxLength: 128),
                        UnfollowedUser_Id = c.String(maxLength: 128),
                        UnlikedUser_Id = c.String(maxLength: 128),
                        ViewedUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Comments", t => t.CommentId)
                .ForeignKey("dbo.AspNetUsers", t => t.LikedUser_Id)
                .ForeignKey("dbo.Posts", t => t.PostId)
                .ForeignKey("dbo.AspNetUsers", t => t.UnfollowedUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UnlikedUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ViewedUser_Id)
                .Index(t => t.UserId)
                .Index(t => t.PostId)
                .Index(t => t.CommentId)
                .Index(t => t.LikedUser_Id)
                .Index(t => t.UnfollowedUser_Id)
                .Index(t => t.UnlikedUser_Id)
                .Index(t => t.ViewedUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Activities", "ViewedUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Activities", "UnlikedUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Activities", "UnfollowedUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Activities", "PostId", "dbo.Posts");
            DropForeignKey("dbo.Activities", "LikedUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Activities", "CommentId", "dbo.Comments");
            DropForeignKey("dbo.Activities", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Activities", new[] { "ViewedUser_Id" });
            DropIndex("dbo.Activities", new[] { "UnlikedUser_Id" });
            DropIndex("dbo.Activities", new[] { "UnfollowedUser_Id" });
            DropIndex("dbo.Activities", new[] { "LikedUser_Id" });
            DropIndex("dbo.Activities", new[] { "CommentId" });
            DropIndex("dbo.Activities", new[] { "PostId" });
            DropIndex("dbo.Activities", new[] { "UserId" });
            DropTable("dbo.Activities");
        }
    }
}
