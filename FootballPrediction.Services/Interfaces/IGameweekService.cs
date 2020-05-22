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
        Task<List<GameWeek>> GetGameWeeks();
        Task<List<Fixture>> GetFixturesForGameWeek(GameWeek gameWeek);
        Task<GameWeek> GetGameWeekById(Guid guid);
        Task<GameWeek> GetGameWeekByInternalId(int internalId);
        Task<GameWeek> GetUpcomingGameweek();
        Task<GameWeek> GetCurrentGameweek();
    }

    public class GameWeekService : IGameWeekService
    {
        private PredictionContext context;

        public GameWeekService(PredictionContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets all the fixtures for a provided gameweek
        /// </summary>
        /// <param name="gameWeek"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<List<Fixture>> GetFixturesForGameWeek(GameWeek gameWeek)
        {
            return await context.Fixtures.Where(f => f.GameWeek.Id == gameWeek.Id).OrderBy(f => f.KickoffDate).ThenBy(f => f.HomeTeam.Name).ToListAsync();
        }

        /// <summary>
        /// Get details of a gameweek by GUID reference
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<GameWeek> GetGameWeekById(Guid guid)
        {
            return await context.GameWeeks.FirstOrDefaultAsync(gw => gw.Id == guid);
        }

        /// <summary>
        /// Get details of a gameweek by internal ID reference.
        /// </summary>
        /// <param name="internalId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<GameWeek> GetGameWeekByInternalId(int internalId)
        {
            return await context.GameWeeks.FirstOrDefaultAsync(gw => gw.InternalId == internalId);
        }

        /// <summary>
        /// Returns all the gameweeks in the database
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<List<GameWeek>> GetGameWeeks()
        {
            return await context.GameWeeks.OrderBy(gw => gw.DeadlineDate).ToListAsync();
        }

        /// <summary>
        /// Returns the next gameweek upcoming
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<GameWeek> GetUpcomingGameweek()
        {
            return await context.GameWeeks.Where(gw => gw.DeadlineDate > DateTime.Now).OrderBy(gw => gw.DeadlineDate).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Returns the latest gameweek (either current or just gone)
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<GameWeek> GetCurrentGameweek()
        {
            return await context.GameWeeks.Where(gw => gw.DeadlineDate < DateTime.Now).OrderByDescending(gw => gw.DeadlineDate).FirstOrDefaultAsync();
        }
    }
}

