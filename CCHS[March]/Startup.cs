using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CCHS_March_.Startup))]
namespace CCHS_March_
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
