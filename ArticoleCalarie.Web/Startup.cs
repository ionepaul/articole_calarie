using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ArticoleCalarie.Web.Startup))]
namespace ArticoleCalarie.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
