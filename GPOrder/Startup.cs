using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GPOrder.Startup))]
namespace GPOrder
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
