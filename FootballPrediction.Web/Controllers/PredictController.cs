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

        public PredictController(
            IGameWeekService gameWeekService,
            IPredictionService predictionService,
            ILeagueService leagueService)
        {
            this.gameWeekService = gameWeekService;
            this.predictionService = predictionService;
            this.leagueService = leagueService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(string gameweek)
        {
            if (string.IsNullOrEmpty(gameweek))
                return Redirect("~/");
                
            var gw = await gameWeekService.GetGameWeekById(Guid.Parse(gameweek));
            var predictions = await predictionService.GetPredictionsForGameWeek(gw, User.Identity.Name);
            var table = await leagueService.GetPremierLeagueTable();

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

            await predictionService.SavePredictions(viewModel.Predictions, User.Identity.Name);

            return Redirect("~/");
        }
    }
}