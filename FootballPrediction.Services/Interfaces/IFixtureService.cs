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
        public async Task<Fixture> GetFixtureById(Guid FixtureId, PredictionContext context)
        {
            return await context.Fixtures.FirstOrDefaultAsync(f => f.Id == FixtureId);
        }
    }
}
