using System;
using System.Collections.Generic;
using System.ServiceProcess;
using SongListScraper;
using System.Timers;
using System.IO;
using System.Threading.Tasks;
using SongListScraper.Service;
using SongListScraper.Settings;
using SongListScraper.Logging;
using System.Configuration;
using SongListScraper.Scraper;

namespace WindowsService1
{
    public partial class SongScrapingService : ServiceBase
    {
        private ScrapingService _service;
        private ILogger _logger;
        private SettingsConfig _settings;

        public SongScrapingService()
        {
            InitializeComponent();

            _settings = new SettingsConfig();
            SettingsConfig.Load(ref _settings);

            _logger = LoggingManager.CreateLogger("MyBasicLogger");

            _service = new ScrapingService(new Station1033Scraper(_logger), _settings, _logger);
        }

        protected override void OnStart(string[] args)
        {
            _logger.Log(LogType.INFO, "Windows service starting");
        }

        protected override void OnStop()
        {
            _logger.Log(LogType.INFO, "Windows service stopping");
        }
    }
}
