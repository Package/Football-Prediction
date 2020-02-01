using FootballPrediction.Data.Database;
using FootballPrediction.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballPrediction.Services.Interfaces
{
    public interface IGameWeekService
    {
        Task<List<GameWeek>> GetGameWeeks(PredictionContext context);
        Task<List<Fixture>> GetFixturesForGameWeek(GameWeek gameWeek, PredictionContext context);
        Task<GameWeek> GetGameWeekById(Guid guid, PredictionContext context);
        Task<GameWeek> GetUpcomingGameweek(PredictionContext context);
        Task<GameWeek> GetCurrentGameweek(PredictionContext context);
    }

    public class GameWeekService : IGameWeekService
    {
        /// <summary>
        /// Gets all the fixtures for a provided gameweek
        /// </summary>
        /// <param name="gameWeek"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<List<Fixture>> GetFixturesForGameWeek(GameWeek gameWeek, PredictionContext context)
        {
            return await context.Fixtures.Where(f => f.GameWeek.Id == gameWeek.Id).OrderBy(f => f.KickoffDate).ThenBy(f => f.HomeTeam.Name).ToListAsync();
        }

        /// <summary>
        /// Get details of a gameweek by GUID reference
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<GameWeek> GetGameWeekById(Guid guid, PredictionContext context)
        {
            return await context.GameWeeks.FirstOrDefaultAsync(gw => gw.Id == guid);
        }

        /// <summary>
        /// Returns all the gameweeks in the database
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<List<GameWeek>> GetGameWeeks(PredictionContext context)
        {
            return await context.GameWeeks.OrderBy(gw => gw.DeadlineDate).ToListAsync();
        }

        /// <summary>
        /// Returns the next gameweek upcoming
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<GameWeek> GetUpcomingGameweek(PredictionContext context)
        {
            return await context.GameWeeks.Where(gw => gw.DeadlineDate > DateTime.Now).OrderBy(gw => gw.DeadlineDate).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Returns the latest gameweek (either current or just gone)
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<GameWeek> GetCurrentGameweek(PredictionContext context)
        {
            return await context.GameWeeks.Where(gw => gw.DeadlineDate < DateTime.Now).OrderByDescending(gw => gw.DeadlineDate).FirstOrDefaultAsync();
        }
    }
}

