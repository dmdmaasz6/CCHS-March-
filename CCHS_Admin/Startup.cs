using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CCHS_Admin.Startup))]
namespace CCHS_Admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
