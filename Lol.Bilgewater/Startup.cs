using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Lol.Itemsets.Startup))]
namespace Lol.Itemsets
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
