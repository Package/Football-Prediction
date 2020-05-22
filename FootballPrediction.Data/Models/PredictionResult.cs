using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballPrediction.Data.Models
{
    public class PredictionResult
    {
        public Prediction Prediction { get; set; }
        public Fixture Fixture { get; set; }

        public bool ShowPrediction { get { return DateTime.Now > Fixture.KickoffDate; } }

        public bool CorrectScore()
        {
            // Fixture has no final result yet.
            if (Fixture.HomeScore == -1 || Fixture.AwayScore == -1)
                return false;

            return Prediction.HomeScore == Fixture.HomeScore && Prediction.AwayScore == Fixture.AwayScore;
        }

        public bool CorrectResult()
        {
            // Fixture has no final result yet.
            if (Fixture.HomeScore == -1 || Fixture.AwayScore == -1)
                return false;

            return (
                (Prediction.HomeScore > Prediction.AwayScore && Fixture.HomeScore > Fixture.AwayScore) ||
                (Prediction.HomeScore < Prediction.AwayScore && Fixture.HomeScore < Fixture.AwayScore) ||
                (Prediction.HomeScore == Prediction.AwayScore && Fixture.HomeScore == Fixture.AwayScore)
             ) && !CorrectScore();
        }

        public int GetPoints()
        {
            if (CorrectScore())
                return 30;
            if (CorrectResult())
                return 10;

            return 0;
        }

    }
}
