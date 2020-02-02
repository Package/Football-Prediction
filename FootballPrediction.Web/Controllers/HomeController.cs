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
        private readonly PredictionContext context;

        public HomeController(
            IGameWeekService gameWeekService, 
            IPredictionService predictionService,
            ILeagueService leagueService)
        {
            this.context = new PredictionContext();
            this.gameWeekService = gameWeekService;
            this.predictionService = predictionService;
            this.leagueService = leagueService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var upcoming = await gameWeekService.GetUpcomingGameweek(context);
            var latest = await gameWeekService.GetCurrentGameweek(context);
            var setPredictions = await predictionService.HasSetPredictions(upcoming, User.Identity.Name, context);

            ViewBag.PredictionResults = await predictionService.GetPredictionResultsForGameWeek(latest, User.Identity.Name, context);

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
            var gameweeks = await gameWeekService.GetGameWeeks(context);
            return View(gameweeks);
        }
    }
}