using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(WebApplication7.Startup))]
namespace WebApplication7
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure SignalR
            app.MapSignalR();

            // Configure your authentication (if applicable)
            ConfigureAuth(app);
        }
    }
}
