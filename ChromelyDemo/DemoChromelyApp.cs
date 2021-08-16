using Chromely;
using Chromely.Core.Network;
using Microsoft.Extensions.DependencyInjection;
using Xilium.CefGlue;

namespace ChromelyDemo
{
    public class DemoChromelyApp : ChromelyBasicApp
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            // other service configuration can be placed here
            services.AddSingleton<ChromelyController, DemoController>();
            services.AddSingleton<CefContextMenuHandler, DemoContextMenuHandler>();
        }
    }
}
