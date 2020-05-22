using Autofac;
using Autofac.Integration.Mvc;
using FootballPrediction.Data.Database;
using FootballPrediction.Data.Models;
using FootballPrediction.Services.Interfaces;
using FootballPrediction.Web.Controllers;
using System.Web.Mvc;

namespace FootballPrediction.Web
{
    public static class AutofacConfig
    {
        public static void ConfigureAutofac()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterType<GameWeekService>().As<IGameWeekService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<PredictionService>().As<IPredictionService>();
            builder.RegisterType<FixtureService>().As<IFixtureService>();
            builder.RegisterType<LeagueService>().As<ILeagueService>();
            builder.RegisterType<PredictionService>().As<IPredictionService>();

            builder.RegisterType<PredictionContext>().InstancePerRequest();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}