namespace FootballPrediction.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FootballModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Fixtures",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        HomeScore = c.Int(),
                        AwayScore = c.Int(),
                        KickoffDate = c.DateTime(nullable: false),
                        InternalId = c.Int(nullable: false),
                        AwayTeam_Id = c.Guid(),
                        GameWeek_Id = c.Guid(),
                        HomeTeam_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.AwayTeam_Id)
                .ForeignKey("dbo.GameWeeks", t => t.GameWeek_Id)
                .ForeignKey("dbo.Teams", t => t.HomeTeam_Id)
                .Index(t => t.AwayTeam_Id)
                .Index(t => t.GameWeek_Id)
                .Index(t => t.HomeTeam_Id);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        ShortName = c.String(),
                        InternalId = c.Int(nullable: false),
                        InternalCode = c.Int(nullable: false),
                        Strength = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GameWeeks",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        InternalId = c.Int(nullable: false),
                        Name = c.String(),
                        DeadlineDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Predictions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        HomeScore = c.Int(nullable: false),
                        AwayScore = c.Int(nullable: false),
                        Fixture_Id = c.Guid(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Fixtures", t => t.Fixture_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Fixture_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Predictions", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Predictions", "Fixture_Id", "dbo.Fixtures");
            DropForeignKey("dbo.Fixtures", "HomeTeam_Id", "dbo.Teams");
            DropForeignKey("dbo.Fixtures", "GameWeek_Id", "dbo.GameWeeks");
            DropForeignKey("dbo.Fixtures", "AwayTeam_Id", "dbo.Teams");
            DropIndex("dbo.Predictions", new[] { "User_Id" });
            DropIndex("dbo.Predictions", new[] { "Fixture_Id" });
            DropIndex("dbo.Fixtures", new[] { "HomeTeam_Id" });
            DropIndex("dbo.Fixtures", new[] { "GameWeek_Id" });
            DropIndex("dbo.Fixtures", new[] { "AwayTeam_Id" });
            DropTable("dbo.Predictions");
            DropTable("dbo.GameWeeks");
            DropTable("dbo.Teams");
            DropTable("dbo.Fixtures");
        }
    }
}
