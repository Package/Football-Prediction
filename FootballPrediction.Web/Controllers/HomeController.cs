using FootballPrediction.Data.Database;
using FootballPrediction.Data.Models;
using FootballPrediction.Services.Interfaces;
using FootballPrediction.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FootballPrediction.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGameWeekService gameWeekService;
        private readonly IPredictionService predictionService;
        private readonly ILeagueService leagueService;

        public HomeController(
            IGameWeekService gameWeekService, 
            IPredictionService predictionService,
            ILeagueService leagueService)
        {
            this.gameWeekService = gameWeekService;
            this.predictionService = predictionService;
            this.leagueService = leagueService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var upcoming = await gameWeekService.GetUpcomingGameweek();
            var latest = await gameWeekService.GetCurrentGameweek();
            var setPredictions = await predictionService.HasSetPredictions(upcoming, User.Identity.Name);

            ViewBag.PredictionResults = await predictionService.GetPredictionResultsForGameWeek(latest, User.Identity.Name);

            var viewModel = new HomepageViewModel
            {
                LatestGameweek = latest,
                UpcomingGameweek = upcoming,
                SetPredictions = setPredictions,
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> All()
        {
            var gameweeks = await gameWeekService.GetGameWeeks();
            return View(gameweeks);
        }
    }
}