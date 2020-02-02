using FootballPrediction.Data.Database;
using FootballPrediction.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballPrediction.Services.Interfaces
{
    public interface IPredictionService
    {
        Task<List<Prediction>> GetPredictionsForGameWeek(GameWeek gameWeek, string userName, PredictionContext context);
        Task<List<PredictionResult>> GetPredictionResultsForGameWeek(GameWeek gameWeek, string userName, PredictionContext context);
        Task SavePredictions(List<Prediction> predictions, string userName, PredictionContext context);
        Task<bool> HasSetPredictions(GameWeek gameWeek, string userName, PredictionContext context);
    }

    public class PredictionService : IPredictionService
    {
        private readonly IUserService userService;
        private readonly IGameWeekService gameWeekService;
        private readonly IFixtureService fixtureService;

        public PredictionService(
            IUserService userService, 
            IGameWeekService gameWeekService,
            IFixtureService fixtureService)
        {
            this.userService = userService;
            this.gameWeekService = gameWeekService;
            this.fixtureService = fixtureService;
        }

        /// <summary>
        /// Returns a list of Prediction Results for a set of gameweek fixtures.
        /// </summary>
        /// <param name="gameWeek"></param>
        /// <param name="userName"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<List<PredictionResult>> GetPredictionResultsForGameWeek(GameWeek gameWeek, string userName, PredictionContext context)
        {
            var predictionResults = new List<PredictionResult>();

            // No user, no results!
            if (string.IsNullOrEmpty(userName))
                return predictionResults;

            var predictions = await GetPredictionsForGameWeek(gameWeek, userName, context);
            predictionResults.AddRange(predictions.Select(p => new PredictionResult { Fixture = p.Fixture, Prediction = p }));

            return predictionResults;
        }

        /// <summary>
        /// Returns the predictions the provided user has made for the specified gameweek. For any fixtures where a 
        /// prediction has not been made, a plain Prediction object is returned ready to be filled in.
        /// </summary>
        /// <param name="gameWeek"></param>
        /// <param name="userName"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<List<Prediction>> GetPredictionsForGameWeek(GameWeek gameWeek, string userName, PredictionContext context)
        {
            // Not logged in.
            if (string.IsNullOrEmpty(userName))
                return new List<Prediction>();

            var maybePredictions = await context.Predictions.Where(p => p.User.UserName == userName && p.Fixture.GameWeek.Id == gameWeek.Id).ToListAsync();
            var fixtures = await gameWeekService.GetFixturesForGameWeek(gameWeek, context);

            // If the user has not made a prediction for one (or more) of the fixtures, add in a default prediction ready for
            // them to edit.
            foreach (var fixture in fixtures)
            {
                var fixturedPredicted = maybePredictions.FirstOrDefault(p => p.Fixture.Id == fixture.Id);

                if (fixturedPredicted == null)
                {
                    maybePredictions.Add(new Prediction { 
                        Fixture = fixture,
                        User = await userService.GetUserByName(userName, context),
                        HomeScore = -1,
                        AwayScore = -1,
                        Id = Guid.NewGuid()
                    });
                }
            }

            return maybePredictions.OrderBy(p => p.Fixture.KickoffDate).ToList();
        }

        /// <summary>
        /// Returns whether or not the provided user has made a full set of predictions for a specified gameweek.
        /// </summary>
        /// <param name="gameWeek"></param>
        /// <param name="userName"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<bool> HasSetPredictions(GameWeek gameWeek, string userName, PredictionContext context)
        {
            if (string.IsNullOrEmpty(userName))
                return false;

            var predictions = await this.GetPredictionsForGameWeek(gameWeek,userName, context);
            var missingPredictions = predictions.Any(p => p.HomeScore == -1 || p.AwayScore == -1);

            return !missingPredictions;
        }

        /// <summary>
        /// Handles saving a list of predictions made by the specified user. Will not save a prediction if it fails some basic validation
        /// checks, such as having a valid scoreline, valid fixture, deadline has not passed for the fixture's gameweek.
        /// </summary>
        /// <param name="predictions"></param>
        /// <param name="userName"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task SavePredictions(List<Prediction> predictions, string userName, PredictionContext context)
        {
            foreach (var prediction in predictions.Where(prediction => 
                prediction.HomeScore >= 0 && prediction.AwayScore >= 0 && 
                prediction.HomeScore <= 10 && prediction.AwayScore <= 10))
            {
                // Check that fixture actually exists
                var fixture = await fixtureService.GetFixtureById(prediction.Fixture.Id, context);
                if (fixture == null)
                    continue;

                // Deadline has passed don't save anything.
                if (DateTime.Now >= fixture.GameWeek.DeadlineDate)
                    continue;

                // Set user properly
                if (prediction.User == null)
                    prediction.User = await userService.GetUserByName(userName, context);

                // Set fixture properly
                prediction.Fixture = fixture;

                context.Predictions.AddOrUpdate(p => p.Id, prediction);
                await context.SaveChangesAsync();
            }
        }
    }
}
