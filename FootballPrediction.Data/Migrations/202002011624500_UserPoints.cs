namespace FootballPrediction.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserPoints : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "TotalPoints", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "CorrectResult", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "CorrectScoreline", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "CorrectScoreline");
            DropColumn("dbo.AspNetUsers", "CorrectResult");
            DropColumn("dbo.AspNetUsers", "TotalPoints");
        }
    }
}
