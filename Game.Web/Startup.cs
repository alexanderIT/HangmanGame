using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Game.Web.Startup))]
namespace Game.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
