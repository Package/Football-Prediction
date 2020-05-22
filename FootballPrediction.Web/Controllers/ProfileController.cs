using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using FootballPrediction.Data.Database;
using FootballPrediction.Data.Models;
using FootballPrediction.Services.Interfaces;
using FootballPrediction.Web.ViewModels;

namespace FootballPrediction.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;
        private readonly IGameWeekService _gameWeekService;
        private readonly IPredictionService _predictionService;
        
        public ProfileController(IUserService userService, 
            IGameWeekService gameWeekService,
            IPredictionService predictionService)
        {
            this._userService = userService;
            this._gameWeekService = gameWeekService;
            this._predictionService = predictionService;
        }
        
        [HttpGet]
        public async Task<ActionResult> Index(string id, int? gameweek)
        {
            // If no ID is provided, look at your own profile
            var user = await _userService.GetById(id) ?? await _userService.GetUserByName(User.Identity.Name);

            // Look at the current gameweek unless a specific one is provided
            GameWeek selectedGameweek = null;
            if (gameweek != null && gameweek.Value > 0)
                selectedGameweek = await _gameWeekService.GetGameWeekByInternalId(gameweek.Value);

            // Fall back to the current gameweek if no specific gameweek is provided.
            if (selectedGameweek == null)
                selectedGameweek = await _gameWeekService.GetCurrentGameweek();
            
            // Any predictions the user made for this gameweek.
            //var predictions = await _predictionService.GetPredictionsForGameWeek(selectedGameweek, user.UserName, _context);

            ViewBag.PredictionResults = await _predictionService.GetPredictionResultsForGameWeek(selectedGameweek, user.UserName);

            var viewModel = new ProfileViewModel
            {
                User = user,
                GameWeek = selectedGameweek,
                //Predictions = predictions,
                OwnProfile = user.UserName == User.Identity.Name
            };
            
            return View(viewModel);
        }
    }
}