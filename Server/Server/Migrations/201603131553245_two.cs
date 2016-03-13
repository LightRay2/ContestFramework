namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class two : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServerGames", "state", c => c.Int(nullable: false));
            AddColumn("dbo.ServerGames", "StartSettingsJson", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServerGames", "StartSettingsJson");
            DropColumn("dbo.ServerGames", "state");
        }
    }
}
