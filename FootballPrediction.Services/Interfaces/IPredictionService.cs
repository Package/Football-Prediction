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

        public async Task<List<Prediction>> GetPredictionsForGameWeek(GameWeek gameWeek, string userName, PredictionContext context)
        {
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

        public async Task<bool> HasSetPredictions(GameWeek gameWeek, string userName, PredictionContext context)
        {
            if (string.IsNullOrEmpty(userName))
                return false;

            var predictions = await this.GetPredictionsForGameWeek(gameWeek,userName, context);
            var missingPredictions = predictions.Where(p => p.HomeScore == -1 || p.AwayScore == -1).Any();

            return !missingPredictions;
        }

        public async Task SavePredictions(List<Prediction> predictions, string userName, PredictionContext context)
        {
            foreach (var prediction in predictions)
            {
                // Score has been set incorrectly.
                if (prediction.HomeScore < 0 || prediction.AwayScore < 0 || prediction.HomeScore > 10 || prediction.AwayScore > 10)
                    continue;

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
