using System;

namespace SongListScraper.Logging
{
    public enum LogType
    {
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
