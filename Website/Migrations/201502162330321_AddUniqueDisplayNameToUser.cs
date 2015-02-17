namespace Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUniqueDisplayNameToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "DisplayName", c => c.String(nullable: false, maxLength: 20));
            CreateIndex("dbo.AspNetUsers", "DisplayName", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.AspNetUsers", new[] { "DisplayName" });
            DropColumn("dbo.AspNetUsers", "DisplayName");
        }
    }
}
