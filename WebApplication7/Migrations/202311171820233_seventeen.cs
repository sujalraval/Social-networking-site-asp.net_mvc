namespace WebApplication7.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seventeen : DbMigration
    {
        public override void Up()
        {
            CreateTable(
       "dbo.Chats",
       c => new
       {
           ChatId = c.Int(nullable: false, identity: true),
           Content = c.String(nullable: false),
           ImagePath = c.String(),
           DateTime = c.DateTime(nullable: false),
           SenderID = c.String(nullable: false, maxLength: 128),
           ReceiverID = c.String(nullable: false, maxLength: 128),
       })
       .PrimaryKey(t => t.ChatId)
       .ForeignKey("dbo.AspNetUsers", t => t.ReceiverID, cascadeDelete: false)
       .ForeignKey("dbo.AspNetUsers", t => t.SenderID, cascadeDelete: false)
       .Index(t => t.SenderID)
       .Index(t => t.ReceiverID);

        }

        public override void Down()
        {
            RenameTable(name: "dbo.chats", newName: "Messages");
        }
    }
}
