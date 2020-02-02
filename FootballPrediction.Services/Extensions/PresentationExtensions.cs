using FootballPrediction.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FootballPrediction.Services.Extensions
{
    public static class PresentationExtensions
    {
        /// <summary>
        /// Formats the result of a fixture.
        /// If the fixture has final scores, then it returns a string with the scores concatenated for example "2 - 1",
        /// however if the match has no final score, it will return "vs."
        /// </summary>
        /// <param name="fixture"></param>
        /// <returns></returns>
        public static string FormatResult(this Fixture fixture)
        {
            if (fixture.HomeScore != -1 && fixture.AwayScore != -1)
            {
                return $"{fixture.HomeScore}-{fixture.AwayScore}";
            }

            return string.Format("vs.");
        }

        /// <summary>
        /// Formats the score display of a prediction.
        /// </summary>
        /// <param name="prediction"></param>
        /// <returns></returns>
        public static string FormatPrediction(this Prediction prediction)
        {
            if (prediction.HomeScore != -1 && prediction.AwayScore != -1)
            {
                return $"{prediction.HomeScore}-{prediction.AwayScore}";
            }

            return string.Empty;
        }

        /// <summary>
        /// Generates a list of SelectListItem to be used to power the DropDownList when predicting a team's score.
        /// </summary>
        /// <param name="prediction"></param>
        /// <param name="home"></param>
        /// <returns></returns>
        public static List<SelectListItem> ScoreSelectList(this Prediction prediction, bool home)
        {
            return Enumerable.Range(0, 11).Select(score =>
                new SelectListItem
                {
                    Value = score.ToString(),
                    Text = score.ToString(),
                    Selected = score == (home ? prediction.HomeScore : prediction.AwayScore)
                }).ToList();
        }
    }
}
