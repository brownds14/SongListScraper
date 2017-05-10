using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using SongListScraper.Helpers.Logging;
using SongListScraper.Helpers.Download;
using SongListScraper.Helpers.TimeProvider;
using System.Threading.Tasks;

namespace SongListScraper.Scraper
{
    public class Station1033Scraper : IScrape
    {
        private HtmlDocument _doc;
        private static DateTime? _lastDownload;
        private IDownload _downloader;
        private ILogger _logger;

        private static readonly String _address = "http://alt1033.iheart.com/music/recently-played/";
        private static readonly Int32 _downloadRestrict = 10; //Number of minutes before next download can be made

        public Station1033Scraper(IDownload downloader, ILogger logger)
        {
            _doc = new HtmlDocument();
            _lastDownload = null;
            _downloader = downloader;
            _logger = logger;
        }

        public async Task<bool> DownloadPage()
        {
            //Restricts the number of download requests
            if (_lastDownload == null || _lastDownload.Value.AddMinutes(_downloadRestrict) <= TimeProvider.Current.UtcNow)
            {
                string html = await _downloader.DownloadHtml(_address);
                _doc.LoadHtml(html);
                _lastDownload = TimeProvider.Current.UtcNow;
                return true;
            }
            else
            {
                throw new ExcessiveDownloadException("Please wait " + _downloadRestrict + " minutes between downloads.");
            }
        }

        public List<Song> ScrapeSongList()
        {
            DateTime today = TimeProvider.Current.UtcNow.Date;
            DateTime date;
            bool isMorning = false;
            List<Song> songs = new List<Song>();

            _logger.Log(LogType.INFO, "Beginning parsing html");
            HtmlNodeCollection songTitles = _doc.DocumentNode.SelectNodes("//a[contains(@class,'track-title')]");
            HtmlNodeCollection songArtists = _doc.DocumentNode.SelectNodes("//a[contains(@class,'track-artist')]");
            HtmlNodeCollection playTimes = _doc.DocumentNode.SelectNodes("//div[contains(@class,'playlist-track-time')]");
            HtmlNode dateHtml = _doc.DocumentNode.SelectSingleNode("//li[contains(@class,'playlist-date-header')]")?.SelectSingleNode("./span");

            if (songTitles != null)
            {
                for (int i = 0; i < songTitles.Count; ++i)
                {
                    //Parse the play times html for the hour and minute
                    String timeHtml = playTimes[i].SelectSingleNode("./span").InnerHtml;
                    String fullTime = timeHtml.Substring(0, timeHtml.IndexOf('<'));
                    String[] time = fullTime.Substring(0, fullTime.Length - 2).Split(':');
                    Int32 hour = Convert.ToInt32(time[0]);
                    Int32 min = Convert.ToInt32(time[1]);

                    //Adjust if the playlist plays over multiple days
                    if (fullTime.Contains("am"))
                        isMorning = true;
                    else if (fullTime.Contains("pm") && isMorning)
                    {
                        isMorning = false;
                        today = today.AddDays(-1);
                    }

                    //Adjust hour for AM and PM
                    if (hour != 12 && fullTime.Contains("pm"))
                        hour += 12;
                    date = today.AddHours(hour).AddMinutes(min);

                    songs.Add(new Song() { Title = songTitles[i].InnerHtml, Artist = songArtists[i].InnerHtml, Played = date });
                }
            }

            return songs;
        }
    }
}
