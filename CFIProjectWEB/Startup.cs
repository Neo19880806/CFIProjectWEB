using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CFIProjectWEB.Startup))]
namespace CFIProjectWEB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
