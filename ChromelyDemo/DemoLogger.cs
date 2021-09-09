using Chromely.Core.Logging;

namespace ChromelyDemo
{
    public class DemoLogger : SimpleLogger
    {
        private static string defaultLogFile = null;

        public static void SetDefaultLogFile(string logFile)
        {
            defaultLogFile = logFile;
        }

        public DemoLogger(
            string fullFilePath = null,
            bool logToConsole = false,
            int maxFileSizeBeforeLogRotation = 20
        ) : base(fullFilePath ?? defaultLogFile, logToConsole, maxFileSizeBeforeLogRotation)
        { 
        }
    }
}