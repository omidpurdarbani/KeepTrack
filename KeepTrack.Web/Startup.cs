using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KeepTrack.Web.Startup))]
namespace KeepTrack.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
