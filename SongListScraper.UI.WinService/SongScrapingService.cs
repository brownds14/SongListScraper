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

            SettingsConfig _settings = new SettingsConfig();
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

            _service = container.Resolve<ScrapingService>();

            _logger = container.Resolve<ILogger>();
            _logger.Log(LogType.INFO, "Components initialized");
        }

        protected override void OnStart(string[] args)
        {
            _logger.Log(LogType.INFO, "Windows service starting");
            _service.StartService();
        }

        protected override void OnStop()
        {
            _logger.Log(LogType.INFO, "Windows service stopping");
            _service.StopService();
        }
    }
}
