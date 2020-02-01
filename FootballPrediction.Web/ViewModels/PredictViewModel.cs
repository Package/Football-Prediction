using FootballPrediction.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballPrediction.Web.ViewModels
{
    public class PredictViewModel
    {
        public GameWeek GameWeek { get; set; }
        public List<Prediction> Predictions { get; set; }
        public List<PremierLeagueTeam> LeagueTable { get; set; }
    }
}