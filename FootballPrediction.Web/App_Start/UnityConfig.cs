using FootballPrediction.Services.Interfaces;
using FootballPrediction.Web.Controllers;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Mvc5;

namespace FootballPrediction.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<AccountController>(new InjectionConstructor());
            container.RegisterType<ManageController>(new InjectionConstructor());

            container.RegisterType<IGameWeekService, GameWeekService>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IPredictionService, PredictionService>();
            container.RegisterType<IFixtureService, FixtureService>();
            container.RegisterType<ILeagueService, LeagueService>();
            container.RegisterType<IPredictionService, PredictionService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}