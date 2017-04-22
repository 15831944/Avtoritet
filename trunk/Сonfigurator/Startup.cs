using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Сonfigurator.Startup))]
namespace Сonfigurator
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
