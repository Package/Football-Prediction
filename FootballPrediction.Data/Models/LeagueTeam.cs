using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballPrediction.Data.Models
{
    public class LeagueTeam
    {
        public int Position { get; set; }
        public int CorrectScores { get; set; }
        public int CorrectResults { get; set; }
        public int WeeksPredicted { get; set; }
        public int Points { get; set; }
        public string TeamName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Id { get; set; }
    }
}