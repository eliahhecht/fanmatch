namespace FanMatch.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddFandomsToPeople : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "FandomPersons",
            //    c => new
            //        {
            //            Fandom_Id = c.Int(nullable: false),
            //            Person_Id = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => new { t.Fandom_Id, t.Person_Id })
            //    .ForeignKey("Fandoms", t => t.Fandom_Id, cascadeDelete: true)
            //    .ForeignKey("People", t => t.Person_Id, cascadeDelete: true)
            //    .Index(t => t.Fandom_Id)
            //    .Index(t => t.Person_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("FandomPersons", new[] { "Person_Id" });
            DropIndex("FandomPersons", new[] { "Fandom_Id" });
            DropForeignKey("FandomPersons", "Person_Id", "People");
            DropForeignKey("FandomPersons", "Fandom_Id", "Fandoms");
            DropTable("FandomPersons");
        }
    }
}
