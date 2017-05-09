using System.ServiceProcess;
using Microsoft.Practices.Unity;
using SongListScraper.Helpers.Download;
using SongListScraper.Helpers.Logging;
using SongListScraper.Helpers.SongWriter;
using SongListScraper.Scraper;
using SongListScraper.Settings;

namespace SongListScraper.UI.WinService
{
    public partial class SongScrapingService : ServiceBase
    {
        private ScrapingService _service;
        private ILogger _logger;

        public SongScrapingService()
        {
            InitializeComponent();

            var container = new UnityContainer();

            container.RegisterType<IDownload, HtmlDownloader>();
            container.RegisterType<ILogger, FalseLogger>();
            container.RegisterType<IWrite, WriteSongToConsole>();
            container.RegisterType<IScrape, Station1033Scraper>();
            container.RegisterType<SettingsConfig, SettingsConfig>();

            _service = container.Resolve<ScrapingService>();
            _logger = container.Resolve<ILogger>();
        }

        protected override void OnStart(string[] args)
        {
            _service.StartService();
            _logger.Log(LogType.INFO, "Windows service starting");
        }

        protected override void OnStop()
        {
            _service.StopService();
            _logger.Log(LogType.INFO, "Windows service stopping");
        }
    }
}
