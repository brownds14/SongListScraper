using System;
using System.Collections.Generic;
using System.Timers;
using SongListScraper.Scraper;
using SongListScraper.Settings;
using SongListScraper.Helpers.Logging;
using SongListScraper.Helpers.SongWriter;

namespace SongListScraper
{
    public class ScrapingService
    {
        private Timer _timer;
        private double _interval;
        private DateTime? lastAdded = null;
        private IScrape _scraper;
        private ILogger _logger;
        private IWrite _writer;

        public ScrapingService(IScrape scraper, IWrite writer, SettingsConfig settings, ILogger logger)
        {
            //Guards
            if (scraper == null)
                throw new ArgumentNullException("scraper");
            if (writer == null)
                throw new ArgumentNullException("writer");
            if (logger == null)
                throw new ArgumentNullException("logger");

            _interval = new TimeSpan(0, settings.UpdateInterval, 0).TotalMilliseconds;
            _logger = logger;
            _scraper = scraper;
            _writer = writer;
            _timer = new Timer(_interval);
            _timer.Elapsed += _timer_Elapsed;
            GetNewSongs();
            StartService();
        }

        public void StartService()
        {
            _timer.Start();
        }

        public void StopService()
        {
            _timer.Stop();
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            GetNewSongs();
        }

        private void GetNewSongs()
        {
            DateTime oldest = new DateTime();

            try
            {
                _scraper.DownloadPage();
                _logger.Log(LogType.INFO, "Download Complete");
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
            catch (ExcessiveDownloadException exception)
            {
                _logger.Log(LogType.WARN, "Failed to download", exception);
            }
        }
    }
}
