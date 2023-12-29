namespace WebApplication7.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tweleve : DbMigration
    {
        public override void Up()
        {
            // Drop the existing Messages table
            DropTable("dbo.Messages");
            // Recreate the Messages table with the desired changes
            CreateTable(
                "dbo.Messages",
                c => new
                {
                    MessageID = c.Int(nullable: false, identity: true),
                    ImagePath = c.String(),
                    SenderID = c.String(nullable: false, maxLength: 128),
                    ReceiverID = c.String(nullable: false, maxLength: 128),
                    Content = c.String(nullable: false),
                })
                .PrimaryKey(t => t.MessageID)
                .Index(t => t.SenderID)
                .Index(t => t.ReceiverID)
                .ForeignKey("dbo.AspNetUsers", t => t.ReceiverID, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.SenderID, cascadeDelete: false);

        }

        public override void Down()
        {
            // Drop the existing Messages table
            DropTable("dbo.Messages");

            // Recreate the Messages table with the old structure
            CreateTable(
                "dbo.Messages",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ImageUrl = c.String(),
                    VideoUrl = c.String(),
                })
                .PrimaryKey(t => t.Id);

            // Other modifications...
        }

    }
}
