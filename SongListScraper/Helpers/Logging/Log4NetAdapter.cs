using log4net;
using System;

namespace SongListScraper.Helpers.Logging
{
    public class Log4NetAdapter : ILogger
    {
        private static ILog _logger;

        public Log4NetAdapter()
        {
            _logger = LogManager.GetLogger("SongListScraper");
        }

        public void Log(LogType type, string msg, Exception e = null)
        {
            switch (type)
            {
                case LogType.DEBUG:
                    _logger.Debug(msg, e);
                    break;
                case LogType.INFO:
                    _logger.Info(msg, e);
                    break;
                case LogType.WARN:
                    _logger.Warn(msg, e);
                    break;
                case LogType.ERROR:
                    _logger.Error(msg, e);
                    break;
                case LogType.FATAL:
                    _logger.Fatal(msg, e);
                    break;
            }
        }
    }
}
