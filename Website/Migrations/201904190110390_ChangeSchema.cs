namespace Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeSchema : DbMigration
    {
        public override void Up()
        {
            MoveTable(name: "dbo.AspNetRoles", newSchema: "yz");
            MoveTable(name: "dbo.AspNetUserRoles", newSchema: "yz");
            MoveTable(name: "dbo.AspNetUsers", newSchema: "yz");
            MoveTable(name: "dbo.AspNetUserClaims", newSchema: "yz");
            MoveTable(name: "dbo.GameStatistic", newSchema: "yz");
            MoveTable(name: "dbo.AspNetUserLogins", newSchema: "yz");
        }
        
        public override void Down()
        {
            MoveTable(name: "yz.AspNetUserLogins", newSchema: "dbo");
            MoveTable(name: "yz.GameStatistic", newSchema: "dbo");
            MoveTable(name: "yz.AspNetUserClaims", newSchema: "dbo");
            MoveTable(name: "yz.AspNetUsers", newSchema: "dbo");
            MoveTable(name: "yz.AspNetUserRoles", newSchema: "dbo");
            MoveTable(name: "yz.AspNetRoles", newSchema: "dbo");
        }
    }
}
