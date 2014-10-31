namespace BlindTestServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajoutScore : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Scores",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        Points = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Name);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Scores");
        }
    }
}
