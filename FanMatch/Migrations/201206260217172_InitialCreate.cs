namespace FanMatch.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "People",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "Matches",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Writer_Id = c.Int(),
            //            Reader_Id = c.Int(),
            //            Project_Id = c.Int(),
            //            Fandom_Id = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("People", t => t.Writer_Id)
            //    .ForeignKey("People", t => t.Reader_Id)
            //    .ForeignKey("Projects", t => t.Project_Id)
            //    .ForeignKey("Fandoms", t => t.Fandom_Id)
            //    .Index(t => t.Writer_Id)
            //    .Index(t => t.Reader_Id)
            //    .Index(t => t.Project_Id)
            //    .Index(t => t.Fandom_Id);
            
            //CreateTable(
            //    "Projects",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "Fandoms",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropIndex("Matches", new[] { "Fandom_Id" });
            DropIndex("Matches", new[] { "Project_Id" });
            DropIndex("Matches", new[] { "Reader_Id" });
            DropIndex("Matches", new[] { "Writer_Id" });
            DropForeignKey("Matches", "Fandom_Id", "Fandoms");
            DropForeignKey("Matches", "Project_Id", "Projects");
            DropForeignKey("Matches", "Reader_Id", "People");
            DropForeignKey("Matches", "Writer_Id", "People");
            DropTable("Fandoms");
            DropTable("Projects");
            DropTable("Matches");
            DropTable("People");
        }
    }
}
