﻿using Microsoft.Practices.Unity;
using SongListScraper.Helpers.Download;
using SongListScraper.Helpers.Logging;
using SongListScraper.Helpers.SongWriter;
using SongListScraper.Scraper;
using SongListScraper.Settings;
using System.Collections.Generic;

namespace SongListScraper.UI.Console
{
    public class Pogram
    {
        public static UnityContainer container;
        public static void Main(string[] args)
        {
            container = new UnityContainer();

            SettingsConfig _settings = new SettingsConfig();
            _settings.StorageType = SongStorage.CONSOLE;
            SettingsConfig.Load(ref _settings);
            container.RegisterInstance<SettingsConfig>(_settings);

            container.RegisterType<IDownload, HtmlDownloader>();
            container.RegisterType<ILogger, Log4NetAdapter>();
            container.RegisterType<IScrape, Alt1025Scraper>();

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
            service.NewSongsRetrieved += WriteSongs;
            service.StartService();

            System.Console.Read();
        }

        public static void WriteSongs(List<Song> songs)
        {
            IWrite writer = container.Resolve<IWrite>();
            writer.WriteSongs(songs);
        }
    }
}
