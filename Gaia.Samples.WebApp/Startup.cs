using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Gaia.Samples.WebApp.Startup))]
namespace Gaia.Samples.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
