using System;

namespace SongListScraper.Helpers.Logging
{
    public enum LogType
    {
        DEBUG,
        INFO,
        WARN,
        ERROR,
        FATAL
    }

    public interface ILogger
    {
        void Log(LogType type, string msg, Exception e = null);
    }
}
