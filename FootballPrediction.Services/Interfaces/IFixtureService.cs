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
    public interface IFixtureService
    {
        Task<Fixture> GetFixtureById(Guid FixtureId, PredictionContext context);
    }

    public class FixtureService : IFixtureService
    {
        /// <summary>
        /// Returns details for a specified fixture ID.
        /// </summary>
        /// <param name="FixtureId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<Fixture> GetFixtureById(Guid FixtureId, PredictionContext context)
        {
            return await context.Fixtures.FirstOrDefaultAsync(f => f.Id == FixtureId);
        }
    }
}
