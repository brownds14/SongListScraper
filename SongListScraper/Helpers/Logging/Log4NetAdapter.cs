using log4net;
using System;

namespace SongListScraper.Helpers.Logging
{
    public class Log4NetAdapter : ILogger
    {
        private static ILog _logger;

        public Log4NetAdapter()
        {
            _logger = LogManager.GetLogger("MyConsoleAppender");
        }

        public void Log(LogType type, string msg, Exception e = null)
        {
            _logger.Info(msg);
        }
    }
}
