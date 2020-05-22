using FootballPrediction.Data.Database;
using FootballPrediction.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballPrediction.Services.Interfaces
{
    public interface ILeagueService
    {
        Task<List<LeagueTeam>> GlobalLeague();
        Task<List<PremierLeagueTeam>> GetPremierLeagueTable();
    }

    public class LeagueService : ILeagueService
    {
        private PredictionContext context;

        public LeagueService(PredictionContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Generates a table representing the positions of the teams in the premier league based
        /// on the results to the fixtures in the database.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<List<PremierLeagueTeam>> GetPremierLeagueTable()
        {
            return await context.Database.SqlQuery<PremierLeagueTeam>("dbo.FootballPrediction_SP_PremierLeagueTable").ToListAsync();
        }

        /// <summary>
        /// Generates a global prediction league representing the positions of the players in the database based on
        /// the results of their predictions.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<List<LeagueTeam>> GlobalLeague()
        {
            return await context.Database.SqlQuery<LeagueTeam>("dbo.FootballPrediction_SP_PredictionLeague").ToListAsync();
        }
    }
}
