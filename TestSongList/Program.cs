using System;
using SongListScraper.Settings;
using SongListScraper.Scraper;
using SongListScraper.Logging;
using SongListScraper.Service;

namespace TestSongList
{
    class Pogram
    {
        static void Main(string[] args)
        {
            SettingsConfig settings = new SettingsConfig();
            settings.StorageType = SongStorage.CONSOLE;
            ILogger _logger = LoggingManager.CreateLogger("MyBasicLogger");
            ScrapingService service = new ScrapingService(new Station1033Scraper(_logger), settings, _logger);
            Console.Read();
        }
    }
}
