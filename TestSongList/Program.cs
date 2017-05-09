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

            container.RegisterType<IDownload, HtmlDownloader>();
            container.RegisterType<ILogger, FalseLogger>();
            container.RegisterType<IWrite, WriteSongToConsole>();
            container.RegisterType<IScrape, Station1033Scraper>();
            container.RegisterType<SettingsConfig, SettingsConfig>();

            ScrapingService service = container.Resolve<ScrapingService>();

            System.Console.Read();
        }
    }
}
