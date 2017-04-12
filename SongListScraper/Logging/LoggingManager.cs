namespace SongListScraper.Logging
{
    public class LoggingManager
    {
        private static ILogger _logger = null;

        public static ILogger CreateLogger(string type)
        {
            if (_logger == null)
            {
                switch (type)
                {
                    case "MyBasicLogger":
                        _logger = new MyBasicLoggerAdapter();
                        break;
                    default:
                        _logger = new FalseLogger();
                        break;
                }
            }

            return _logger;
        }

        public static ILogger GetInstance()
        {
            return _logger;
        }
    }
}
