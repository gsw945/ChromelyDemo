using Chromely;
using Chromely.Core.Configuration;
using Chromely.Core.Network;
using System.Collections.Generic;

namespace ChromelyDemo
{
    public class DemoController : ChromelyController
    {
        private readonly IChromelyConfiguration _config;

        public DemoController(IChromelyConfiguration config)
        {
            _config = config;
        }

        [RequestAction(RouteKey = "/democontroller/movies")]
        public IChromelyResponse GetMovies(IChromelyRequest request)
        {
            return new ChromelyResponse();
        }

        [CommandAction(RouteKey = "/democontroller/showdevtools")]
        public void ShowDevTools(IDictionary<string, string> queryParameters)
        {
            if (_config != null && !string.IsNullOrWhiteSpace(_config.DevToolsUrl))
            {
                BrowserLauncher.Open(_config.Platform, _config.DevToolsUrl);
            }
        }
    }
}
