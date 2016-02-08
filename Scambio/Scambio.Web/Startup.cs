using Microsoft.Owin;
using Owin;
using Scambio.Web;

[assembly: OwinStartup(typeof(Startup))]
namespace Scambio.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
