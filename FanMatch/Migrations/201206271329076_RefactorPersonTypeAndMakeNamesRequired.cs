namespace FanMatch.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class RefactorPersonTypeAndMakeNamesRequired : DbMigration
    {
        public override void Up()
        {
            AddColumn("People", "IsReader", c => c.Boolean(nullable: false));
            AddColumn("People", "IsWriter", c => c.Boolean(nullable: false));
            AlterColumn("People", "Name", c => c.String(nullable: false));
            AlterColumn("Fandoms", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("Fandoms", "Name", c => c.String());
            AlterColumn("People", "Name", c => c.String());
            DropColumn("People", "IsWriter");
            DropColumn("People", "IsReader");
        }
    }
}
