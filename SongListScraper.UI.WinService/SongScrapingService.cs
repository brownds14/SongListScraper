using System.ServiceProcess;
using Microsoft.Practices.Unity;
using SongListScraper.Helpers.Download;
using SongListScraper.Helpers.Logging;
using SongListScraper.Helpers.SongWriter;
using SongListScraper.Scraper;
using SongListScraper.Settings;
using System.Collections.Generic;

namespace SongListScraper.UI.WinService
{
    public partial class SongScrapingService : ServiceBase
    {
        private ScrapingService _service;
        private ILogger _logger;
        private UnityContainer _container;

        public SongScrapingService()
        {
            InitializeComponent();

            _container = new UnityContainer();

            SettingsConfig _settings = new SettingsConfig();
            SettingsConfig.Load(ref _settings);
            _container.RegisterInstance<SettingsConfig>(_settings);

            _container.RegisterType<IDownload, HtmlDownloader>();
            _container.RegisterType<ILogger, Log4NetAdapter>();
            _container.RegisterType<IScrape, Alt1025Scraper>();

            switch (_settings.StorageType)
            {
                case SongStorage.CONSOLE:
                    _container.RegisterType<IWrite, WriteSongToConsole>();
                    break;
                case SongStorage.FILE:
                    _container.RegisterType<IWrite, WriteSongToFile>();
                    break;
            }

            _service = _container.Resolve<ScrapingService>();
            _service.NewSongsRetrieved += WriteSongs;

            _logger = _container.Resolve<ILogger>();
            _logger.Log(LogType.INFO, "Components initialized");
        }

        public void WriteSongs(List<Song> songs)
        {
            IWrite writer = _container.Resolve<IWrite>();
            writer.WriteSongs(songs);
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
