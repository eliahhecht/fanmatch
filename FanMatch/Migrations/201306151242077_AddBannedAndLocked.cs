namespace FanMatch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBannedAndLocked : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Matches", "IsBanned", c => c.Boolean(nullable: false));
            AddColumn("dbo.Matches", "IsLocked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Matches", "IsLocked");
            DropColumn("dbo.Matches", "IsBanned");
        }
    }
}
