using Chromely.Core;
using Chromely.Core.Configuration;
using Chromely.Core.Infrastructure;
using Chromely.Core.Network;
using System.Collections.Generic;

namespace ChromelyDemo
{
    class DemoConfiguration : IChromelyConfiguration
    {
        private static IChromelyConfiguration defaultConfiguration = DefaultConfiguration.CreateForRuntimePlatform();

        public string AppName { get; set; } = "My Awesome Chromely App";
        public string StartUrl { get; set; } = defaultConfiguration.StartUrl;
        public string AppExeLocation { get; set; } = defaultConfiguration.AppExeLocation;
        public string ChromelyVersion { get; set; } = defaultConfiguration.ChromelyVersion;
        public ChromelyPlatform Platform { get; set; } = ChromelyRuntime.Platform;
        public bool DebuggingMode { get; set; } = defaultConfiguration.DebuggingMode;
        public string DevToolsUrl { get; set; } = defaultConfiguration.DevToolsUrl;
        public Dictionary<string, string> CommandLineArgs { get; set; } = defaultConfiguration.CommandLineArgs;
        public List<string> CommandLineOptions { get; set; } = defaultConfiguration.CommandLineOptions;
        public Dictionary<string, string> CustomSettings { get; set; } = defaultConfiguration.CustomSettings;
        public Dictionary<string, object> ExtensionData { get; set; } = defaultConfiguration.ExtensionData;
        public IChromelyJavaScriptExecutor JavaScriptExecutor { get; set; } = defaultConfiguration.JavaScriptExecutor;
        public List<UrlScheme> UrlSchemes { get; set; } = defaultConfiguration.UrlSchemes;
        public CefDownloadOptions CefDownloadOptions { get; set; } = defaultConfiguration.CefDownloadOptions;
        public IWindowOptions WindowOptions { get; set; } = defaultConfiguration.WindowOptions;
    }
}
