using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FootballPrediction.Web.Startup))]
namespace FootballPrediction.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
