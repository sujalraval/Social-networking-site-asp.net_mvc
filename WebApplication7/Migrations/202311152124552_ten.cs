namespace WebApplication7.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ten : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Activities", new[] { "ViewedUser_Id" });
            DropColumn("dbo.Activities", "ViewedUserId");
            RenameColumn(table: "dbo.Activities", name: "ViewedUser_Id", newName: "ViewedUserId");
            AlterColumn("dbo.Activities", "ViewedUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Activities", "ViewedUserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Activities", new[] { "ViewedUserId" });
            AlterColumn("dbo.Activities", "ViewedUserId", c => c.Int());
            RenameColumn(table: "dbo.Activities", name: "ViewedUserId", newName: "ViewedUser_Id");
            AddColumn("dbo.Activities", "ViewedUserId", c => c.Int());
            CreateIndex("dbo.Activities", "ViewedUser_Id");
        }
    }
}
