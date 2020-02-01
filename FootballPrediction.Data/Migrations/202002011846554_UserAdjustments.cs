namespace FootballPrediction.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAdjustments : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String(maxLength: 25));
            AlterColumn("dbo.AspNetUsers", "LastName", c => c.String(maxLength: 25));
            AlterColumn("dbo.AspNetUsers", "TeamName", c => c.String(maxLength: 25));
            DropColumn("dbo.AspNetUsers", "TotalPoints");
            DropColumn("dbo.AspNetUsers", "CorrectResult");
            DropColumn("dbo.AspNetUsers", "CorrectScoreline");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "CorrectScoreline", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "CorrectResult", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "TotalPoints", c => c.Int(nullable: false));
            AlterColumn("dbo.AspNetUsers", "TeamName", c => c.String());
            AlterColumn("dbo.AspNetUsers", "LastName", c => c.String());
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String());
        }
    }
}
