using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Lol.Bilgewater.Startup))]
namespace Lol.Bilgewater
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
