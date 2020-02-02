using System.Collections.Generic;
using FootballPrediction.Data.Models;

namespace FootballPrediction.Web.ViewModels
{
    public class ProfileViewModel
    {
        public ApplicationUser User { get; set; }
        public GameWeek GameWeek { get; set; }
        public List<Prediction> Predictions { get; set; }
        public bool OwnProfile { get; set; }
    }
}