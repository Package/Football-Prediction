using FootballPrediction.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballPrediction.Web.ViewModels
{
    public class HomepageViewModel
    {
        public GameWeek LatestGameweek { get; set; }

        public GameWeek UpcomingGameweek { get; set; }

        public bool Authenticated { get; set; }
        public bool SetPredictions { get; set; }
    }
}