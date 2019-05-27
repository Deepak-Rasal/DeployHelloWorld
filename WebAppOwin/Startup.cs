using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebAppOwin.Startup))]
namespace WebAppOwin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
