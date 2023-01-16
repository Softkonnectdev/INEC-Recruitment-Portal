using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Recruitment_Portal_System.Startup))]
namespace Recruitment_Portal_System
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
