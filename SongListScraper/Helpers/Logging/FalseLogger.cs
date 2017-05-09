using System;

namespace SongListScraper.Helpers.Logging
{
    public class FalseLogger : ILogger
    {
        public void Log(LogType type, string msg, Exception e)
        {
            //Log nothing
        }
    }
}
