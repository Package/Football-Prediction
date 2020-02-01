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
    [Authorize]
    public class PredictController : Controller
    {
        private readonly IGameWeekService gameWeekService;
        private readonly IPredictionService predictionService;
        private readonly ILeagueService leagueService;
        private readonly PredictionContext context;

        public PredictController(
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
        public async Task<ActionResult> Index(string gameweek)
        {
            if (string.IsNullOrEmpty(gameweek))
                return Redirect("~/");
                
            var gw = await gameWeekService.GetGameWeekById(Guid.Parse(gameweek), context);
            var predictions = await predictionService.GetPredictionsForGameWeek(gw, User.Identity.Name, context);
            var table = await leagueService.GetPremierLeagueTable(context);

            var viewModel = new PredictViewModel { 
                GameWeek = gw,
                Predictions = predictions,
                LeagueTable = table
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(PredictViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index", new { gameweek = viewModel.GameWeek.Id });

            await predictionService.SavePredictions(viewModel.Predictions, User.Identity.Name, context);

            return Redirect("~/");
        }
    }
}