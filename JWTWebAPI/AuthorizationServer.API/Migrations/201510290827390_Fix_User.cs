namespace AuthorizationServer.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fix_User : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Users", name: "Audience_ClientId", newName: "ClientId");
            RenameIndex(table: "dbo.Users", name: "IX_Audience_ClientId", newName: "IX_ClientId");
            DropColumn("dbo.Users", "AudienceId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "AudienceId", c => c.String());
            RenameIndex(table: "dbo.Users", name: "IX_ClientId", newName: "IX_Audience_ClientId");
            RenameColumn(table: "dbo.Users", name: "ClientId", newName: "Audience_ClientId");
        }
    }
}
