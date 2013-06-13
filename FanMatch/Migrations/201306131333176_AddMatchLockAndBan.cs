namespace FanMatch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMatchLockAndBan : DbMigration
    {
        public override void Up()
        {
            AddColumn("Matches", "IsLocked", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("Matches", "IsBanned", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("Matches", "IsLocked");
            DropColumn("Matches", "IsBanned");
        }
    }
}
