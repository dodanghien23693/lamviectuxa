using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebSoftSeo.Startup))]
namespace WebSoftSeo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
