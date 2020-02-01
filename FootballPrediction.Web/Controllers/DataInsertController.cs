using FootballPrediction.Data.Database;
using FootballPrediction.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FootballPrediction.Web.Controllers
{
    public class DataInsertController : Controller
    {
        const string BOOTSTRAP_STATIC_URL = "https://fantasy.premierleague.com/api/bootstrap-static/";
        const string BOOTSTRAP_FIXTURES_URL = "https://fantasy.premierleague.com/api/fixtures/";
        private readonly PredictionContext context = new PredictionContext();

        [HttpGet]
        public async Task<JsonResult> Index()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(new Uri(BOOTSTRAP_STATIC_URL));
                var result = await response.Content.ReadAsStringAsync();

                var jsonData = JsonConvert.DeserializeObject<BootstrapStatic>(result);
                await ManageGameweeks(jsonData.events);
                await ManageTeams(jsonData.teams);
                await ManageFixtures();

                return Json(new { message = "Done", data = jsonData }, JsonRequestBehavior.AllowGet);
            }
        }

        private async Task ManageFixtures()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(new Uri(BOOTSTRAP_FIXTURES_URL));
                var result = await response.Content.ReadAsStringAsync();

                var jsonData = JsonConvert.DeserializeObject<List<BootstrapFixture>>(result);

                var fixturesToInsert = new List<Fixture>();

                jsonData.ForEach(f =>
                {
                    // Skip any fixtures without a gameweek reference
                    if (string.IsNullOrEmpty(f.@event))
                        return;

                    var gameWeek = Convert.ToInt32(f.@event);
                    var homeTeam = Convert.ToInt32(f.team_h);
                    var awayTeam = Convert.ToInt32(f.team_a);
                    var homeScore = f.team_h_score != null ? Convert.ToInt32(f.team_h_score) : -1;
                    var awayScore = f.team_a_score != null ? Convert.ToInt32(f.team_a_score) : -1;

                    var fixture = new Fixture
                    {
                        Id = Guid.NewGuid(),
                        GameWeek = context.GameWeeks.FirstOrDefault(gw => gw.InternalId == gameWeek),
                        InternalId = Convert.ToInt32(f.code),
                        KickoffDate = DateTime.Parse(f.kickoff_time),
                        HomeTeam = context.Teams.FirstOrDefault(t => t.InternalId == homeTeam),
                        AwayTeam = context.Teams.FirstOrDefault(t => t.InternalId == awayTeam),
                        HomeScore = homeScore,
                        AwayScore = awayScore
                    };

                    fixturesToInsert.Add(fixture);
                });

                foreach (var fixture in fixturesToInsert)
                {
                    context.Fixtures.AddOrUpdate(f => f.InternalId, fixture);
                    await context.SaveChangesAsync();
                }
            }
        }

        private async Task ManageTeams(List<BootstrapStaticTeam> teams)
        {
            // Don't attempt to add more teams if they already exist.
            // Would need to look at this year-on-year as obviously 3 teams get relegated and 3 get promoted.
            //if (context.Teams.Count() > 0)
            //    return;

            var teamsToInsert = teams.Select(t => new Team
            {
                Id = Guid.NewGuid(),
                InternalCode = Convert.ToInt32(t.code),
                InternalId = Convert.ToInt32(t.id),
                Name = t.name,
                ShortName = t.short_name,
                Strength = Convert.ToInt32(t.strength)
            });

            foreach (var team in teamsToInsert)
            {
                context.Teams.AddOrUpdate(t => t.InternalId, team);
                await context.SaveChangesAsync();
            }
        }

        private async Task ManageGameweeks(List<BootstrapStaticEvent> events)
        {
            // Don't attempt to add more gameweeks if they already exist.
            // Would need to look at this year-on-year as obvious each season there is a new set of 38 gameweeks.
            //if (context.GameWeeks.Count() > 0)
            //    return;

            var gameweeksToInsert = events.Select(e => new GameWeek
            {
                Id = Guid.NewGuid(),
                InternalId = Convert.ToInt32(e.id),
                DeadlineDate = DateTime.Parse(e.deadline_time),
                Name = e.name
            });

            foreach (var gameweek in gameweeksToInsert)
            {
                context.GameWeeks.AddOrUpdate(g => g.InternalId, gameweek);
                await context.SaveChangesAsync();
            }
        }
    }
}