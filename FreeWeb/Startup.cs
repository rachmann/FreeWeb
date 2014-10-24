using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FreeWeb.Startup))]
namespace FreeWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
