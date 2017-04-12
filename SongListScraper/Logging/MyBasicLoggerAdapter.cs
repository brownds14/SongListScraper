using System;
using MyBasicLogger;
using MyBasicLogger.Loggers;

namespace SongListScraper.Logging
{
    internal class MyBasicLoggerAdapter : ILogger
    {
        private ILog _logger;

        public MyBasicLoggerAdapter()
        {
            _logger = BasicLoggerManager.CreateLogger();
        }

        public void Log(LogType type, string msg, Exception e)
        {
            _logger.Log(ConvertLogSeverity(type), msg, e);
        }

        private MyBasicLogger.Loggers.LogType ConvertLogSeverity(LogType type)
        {
            MyBasicLogger.Loggers.LogType ret = MyBasicLogger.Loggers.LogType.INFO;
            switch (type)
            {
                case LogType.INFO:
                    ret = MyBasicLogger.Loggers.LogType.INFO;
                    break;
                case LogType.WARN:
                    ret = MyBasicLogger.Loggers.LogType.WARN;
                    break;
                case LogType.ERROR:
                    ret = MyBasicLogger.Loggers.LogType.ERROR;
                    break;
                case LogType.FATAL:
                    ret = MyBasicLogger.Loggers.LogType.FATAL;
                    break;
            }
            return ret;
        }
    }
}
