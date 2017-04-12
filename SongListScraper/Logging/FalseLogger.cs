using System;

namespace SongListScraper.Logging
{
    internal class FalseLogger : ILogger
    {
        public void Log(LogType type, string msg, Exception e)
        {
            //Log nothing
        }
    }
}
