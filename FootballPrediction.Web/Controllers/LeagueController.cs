using FootballPrediction.Data.Database;
using FootballPrediction.Data.Models;
using FootballPrediction.Services.Interfaces;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FootballPrediction.Web.Controllers
{
    public class LeagueController : Controller
    {
        private readonly IGameWeekService gameWeekService;
        private readonly IPredictionService predictionService;
        private readonly ILeagueService leagueService;

        public LeagueController(
            IGameWeekService gameWeekService,
            IPredictionService predictionService,
            ILeagueService leagueService)
        {
            this.gameWeekService = gameWeekService;
            this.predictionService = predictionService;
            this.leagueService = leagueService;
        }


        [HttpGet]
        public async Task<ActionResult> Index(string league, int? page)
        {
            var leagueTeams = new List<LeagueTeam>();

            if (string.IsNullOrEmpty(league))
            {
                leagueTeams = await leagueService.GlobalLeague();
            }
            else
            {
                // Load the league for the provided code...
            }

            var pageNumber = page ?? 1;
            var pagedItems = leagueTeams.ToPagedList(pageNumber, 15);
            
            return View(pagedItems);
        }

        [HttpGet]
        public async Task<ActionResult> Premier()
        {
            var table = await leagueService.GetPremierLeagueTable();
            return View(table);
        }
    }
}