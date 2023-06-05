using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SchoolAdminSite.Startup))]
namespace SchoolAdminSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
