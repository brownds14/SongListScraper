using System;
using System.Collections.Generic;
using System.Timers;
using SongListScraper.Scraper;
using SongListScraper.Settings;
using SongListScraper.Helpers.Logging;
using SongListScraper.Helpers.SongWriter;

namespace SongListScraper
{
    public delegate void SongCallback(List<Song> songs);

    public class ScrapingService
    {


        private Timer _timer;
        private double _interval;
        private DateTime? lastAdded = null;
        private IScrape _scraper;
        private ILogger _logger;
        private bool _canDownload = true;
        private SongCallback _callback;


        public ScrapingService(IScrape scraper, SongCallback callback, SettingsConfig settings, ILogger logger)
        {
            //Guards
            if (scraper == null)
                throw new ArgumentNullException("scraper");
            if (logger == null)
                throw new ArgumentNullException("logger");

            _callback = callback;
            _interval = new TimeSpan(0, settings.UpdateInterval, 0).TotalMilliseconds;
            _logger = logger;
            _scraper = scraper;
            _timer = new Timer(_interval);
        }

        public void StartService()
        {
            if (_canDownload)
            {
                GetNewSongs();
                _canDownload = false;
            }

            _timer.Elapsed -= timerStoppedElapsed;
            _timer.Elapsed += timerElapsed;
            _timer.Start();
        }

        public void StopService()
        {
            _timer.Elapsed -= timerElapsed;
            _timer.Elapsed += timerStoppedElapsed;
        }

        private void timerElapsed(object sender, ElapsedEventArgs e)
        {
            GetNewSongs();
        }

        private void timerStoppedElapsed(object sender, ElapsedEventArgs e)
        {
            _canDownload = true;
            _timer.Elapsed -= timerStoppedElapsed;
            _timer.Stop();
        }

        private async void GetNewSongs()
        {
            DateTime? oldest = new DateTime();

            try
            {
                if (await _scraper.DownloadPage())
                {
                    _logger.Log(LogType.INFO, "Download Complete");
                    List<Song> songs = _scraper.ScrapeSongList();
                    List<Song> newSongs = new List<Song>();

                    foreach (Song s in songs)
                    {
                        if (s.Played > oldest)
                            oldest = s.Played;

                        if (lastAdded == null || s.Played > lastAdded)
                        {
                            newSongs.Add(s);
                        }
                    }

                    _callback(newSongs);
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
