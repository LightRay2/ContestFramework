namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class one : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.ServerGameServerPlayers", "ServerGameId");
            CreateIndex("dbo.ServerGameServerPlayers", "ServerPlayerId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ServerGameServerPlayers", new[] { "ServerPlayerId" });
            DropIndex("dbo.ServerGameServerPlayers", new[] { "ServerGameId" });
        }
    }
}
