using Chromely.Core;
using Chromely.Core.Network;
using System.Collections.Generic;

namespace ChromelyDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // create a configuration with OS-specific defaults
            var config = new DemoConfiguration();

            // your configuration
            // config.StartUrl = "https://gsw945.com/";
            config.StartUrl = "local://app/index.html";
            config.WindowOptions.Title = "My Awesome Chromely App!";
            config.UrlSchemes.AddRange(new List<UrlScheme>()
            {
                new UrlScheme("default-command-http", "http", "command.chromely", string.Empty, UrlSchemeType.Command, false),
            });
            config.DebuggingMode = true;

            // application builder
            AppBuilder
                .Create()
                .UseConfig<DemoConfiguration>(config)
                .UseApp<DemoChromelyApp>()
                .Build()
                .Run(args);
        }
    }
}
