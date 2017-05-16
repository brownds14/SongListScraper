using Microsoft.Practices.Unity;
using SongListScraper.Helpers.Download;
using SongListScraper.Helpers.Logging;
using SongListScraper.Helpers.SongWriter;
using SongListScraper.Scraper;
using SongListScraper.Settings;

namespace SongListScraper.UI.Console
{
    class Pogram
    {
        static void Main(string[] args)
        {
            var container = new UnityContainer();

            SettingsConfig _settings = new SettingsConfig();
            _settings.StorageType = SongStorage.CONSOLE;
            SettingsConfig.Load(ref _settings);
            container.RegisterInstance<SettingsConfig>(_settings);

            container.RegisterType<IDownload, HtmlDownloader>();
            container.RegisterType<ILogger, Log4NetAdapter>();
            container.RegisterType<IScrape, Station1033Scraper>();

            switch (_settings.StorageType)
            {
                case SongStorage.CONSOLE:
                    container.RegisterType<IWrite, WriteSongToConsole>();
                    break;
                case SongStorage.FILE:
                    container.RegisterType<IWrite, WriteSongToFile>();
                    break;
            }

            ScrapingService service = container.Resolve<ScrapingService>();
            service.StartService();

            System.Console.Read();
        }
    }
}
