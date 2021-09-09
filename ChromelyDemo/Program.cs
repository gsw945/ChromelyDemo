using Chromely.Core;
using Chromely.Core.Infrastructure;
using Chromely.Core.Network;
using Chromely.Loader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using Xilium.CefGlue;

namespace ChromelyDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomainOnProcessExit;
            AssemblyLoadContext.Default.Unloading += DefaultOnUnloading;
            Console.CancelKeyPress += ConsoleOnCancelKeyPress;

            string logDir = Path.Combine(GetExecuteDir(), "logs");
            Directory.CreateDirectory(logDir);
            LimitLogsSize(logDir, 500);

            RedirectStandardError(logDir);
            RedirectStandardOutput(logDir);

            ShowInfo();
            ShowSeparator();

            // create a configuration with OS-specific defaults
            var config = new DemoConfiguration();

            // your configuration
            // config.StartUrl = "https://gsw945.com/";
            config.CefDownloadOptions.AutoDownloadWhenMissing = false;
            config.StartUrl = "local://app/index.html";
            config.WindowOptions.Title = "My Awesome Chromely App!";
            config.UrlSchemes.AddRange(new List<UrlScheme>()
            {
                new UrlScheme("default-command-http", "http", "command.chromely", string.Empty, UrlSchemeType.Command, false),
            });
            config.DebuggingMode = true;
            config.CustomSettings = new Dictionary<string, string>()
            {
                ["cefLogFile"] = Path.Combine(logDir, "chromely.cef.log"),
                ["logSeverity"] = "verbose",
                ["locale"] = "zh-CN"
            };
            DemoLogger.SetDefaultLogFile(Path.Combine(logDir, $"app-{GetDateTimeString()}.log"));

            // application builder
            AppBuilder
                .Create()
                .UseConfig<DemoConfiguration>(config)
                .UseApp<DemoChromelyApp>()
                .Build()
                .Run(args);
        }

        static void ShowSeparator(char separator = '-', int width = 70)
        {
            Console.WriteLine(string.Empty.PadLeft(width, separator));
        }

        static void ShowInfo()
        {
            // var _platform = ChromelyRuntime.Platform;
            // var _architecture = RuntimeInformation.ProcessArchitecture;
            // var _build = ChromelyRuntime.GetExpectedCefBuild();

            Console.WriteLine($"CefVersion: {CefRuntime.CefVersion}");
            Console.WriteLine($"ChromeVersion: {CefRuntime.ChromeVersion}");
        }

        static string GetDateTimeString()
        {
            // return DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff");
            return DateTime.Now.ToString("yyyy-MM-dd-HH");
        }

        static string GetStartupFolder()
        {
            return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }

        static string GetExecuteDir()
        {
            return Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
        }

        static void LimitLogsSize(string logDir, int sizeMB)
        {
            var dirInfo = new DirectoryInfo(logDir);
            var dirInfoFiles = dirInfo.EnumerateFiles("*.log", SearchOption.AllDirectories);
            long sizeInBytes = sizeMB * 1024 * 1024;
            long size = 0;
            bool sizeReach = false;
            // 按照时间倒序, 当日志累计大小达到后，删除后续的(旧的)日志文件
            foreach (var fi in dirInfoFiles.OrderByDescending((fi) => fi.LastWriteTime))
            {
                size += fi.Length;
                if (!sizeReach)
                {
                    if (size > sizeInBytes)
                    {
                        sizeReach = true;
                    }
                }
                if (sizeReach)
                {
                    File.Delete(fi.FullName);
                }
            }
        }

        #region StandardOutput
        static FileStream fsOut;
        static StreamWriter swOut;
        static readonly TextWriter defaultOut = Console.Out;

        static void RedirectStandardOutput(string logDir)
        {
            var fileName = Path.Combine(logDir, $"output-{GetDateTimeString()}.log");
            // 
            try
            {
                fsOut = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
                swOut = new StreamWriter(fsOut)
                {
                    AutoFlush = true
                };
                Console.SetOut(TextWriter.Synchronized(swOut));
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Cannot open {fileName} for writing");
                Console.Error.WriteLine(e.Message);
            }
        }

        static void ResetStandardOutput()
        {
            if (fsOut != null)
            {
                if (swOut != null)
                {
                    swOut.Close();
                }
                fsOut.Close();
            }
            Console.SetOut(defaultOut);
        }
        #endregion

        #region StandardError
        static FileStream fsErr;
        static StreamWriter swErr;
        static readonly TextWriter defaultErr = Console.Error;

        static void RedirectStandardError(string logDir)
        {
            var fileName = Path.Combine(logDir, $"error-{GetDateTimeString()}.log");
            // 
            try
            {
                fsErr = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
                swErr = new StreamWriter(fsErr)
                {
                    AutoFlush = true
                };
                Console.SetOut(TextWriter.Synchronized(swErr));
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Cannot open {fileName} for writing");
                Console.Error.WriteLine(e.Message);
            }
        }

        static void ResetStandardError()
        {
            if (fsErr != null)
            {
                if (swErr != null)
                {
                    swErr.Close();
                }
                fsErr.Close();
            }
            Console.SetOut(defaultErr);
        }
        #endregion

        private static void CurrentDomainOnProcessExit(object sender, EventArgs eventArgs)
        {
            Console.WriteLine("Exiting process");
            ResetStandardError();
            ResetStandardOutput();
        }

        private static void DefaultOnUnloading(AssemblyLoadContext assemblyLoadContext)
        {
            Console.WriteLine("Unloading process");
            ResetStandardError();
            ResetStandardOutput();
        }

        private static void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("Canceling process");
            e.Cancel = true;
            ResetStandardError();
            ResetStandardOutput();
        }
    }
}
