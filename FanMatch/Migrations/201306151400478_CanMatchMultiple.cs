namespace FanMatch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CanMatchMultiple : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "CanMatchMultiple", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.People", "CanMatchMultiple");
        }
    }
}
