using System;
using System.Collections.Generic;
using System.Timers;
using SongListScraper.Scraper;
using SongListScraper.Logging;
using SongListScraper.Settings;
using SongListScraper.SongWriter;

namespace SongListScraper.Service
{
    public class ScrapingService
    {
        private Timer _timer;
        private double _interval;
        private DateTime? lastAdded = null;
        private IScrape _scraper;
        private ILogger _logger;
        private IWrite _writer;

        public ScrapingService(IScrape scraper) : this(scraper, new SettingsConfig(), new FalseLogger()) { }

        public ScrapingService(IScrape scraper, SettingsConfig settings) : this(scraper, settings, new FalseLogger()) { }

        public ScrapingService(IScrape scraper, SettingsConfig settings, ILogger logger)
        {
            _interval = new TimeSpan(0, settings.UpdateInterval, 0).TotalMilliseconds;
            _logger = logger;
            _scraper = scraper;
            _writer = SongWriterManager.CreateWriter(settings);
            _timer = new Timer(_interval);
            _timer.Elapsed += _timer_Elapsed;
            GetNewSongs();
            _timer.Start();
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            GetNewSongs();
        }

        private async void GetNewSongs()
        {
            DateTime oldest = new DateTime();

            try
            {
                if (await _scraper.DownloadPage())
                {
                    List<Song> songs = _scraper.ScrapeSongList();
                    List<Song> songsToWrite = new List<Song>();

                    foreach (Song s in songs)
                    {
                        if (s.Played > oldest)
                            oldest = s.Played;

                        if (lastAdded == null || s.Played > lastAdded)
                        {
                            songsToWrite.Add(s);
                        }
                    }

                    _writer.WriteSongs(songsToWrite);
                    lastAdded = oldest;
                }
            }
            catch (ExcessiveDownloadException exception)
            {
                _logger.Log(LogType.WARN, "Failed to download", exception);
            }
        }
    }
}
