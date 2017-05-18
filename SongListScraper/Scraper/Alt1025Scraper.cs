using HtmlAgilityPack;
using SongListScraper.Helpers.Download;
using SongListScraper.Helpers.Logging;
using SongListScraper.Helpers.TimeProvider;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SongListScraper.Scraper
{
    public class Alt1025Scraper : IScrape
    {
        private HtmlDocument _doc;
        private static DateTime? _lastDownload;
        private IDownload _downloader;
        private ILogger _logger;

        public static readonly string _description = "Get recently played songs from The Alternative Station 102.5";
        public static readonly string _address = "http://cd1025.com/about/playlists/now-playing";
        public static readonly int _downloadRestrict = 10;

        public Alt1025Scraper(IDownload downloader, ILogger logger)
        {
            if (downloader == null)
                throw new ArgumentNullException("downloader");
            if (logger == null)
                throw new ArgumentNullException("logger");

            _doc = new HtmlDocument();
            _downloader = downloader;
            _logger = logger;
            _lastDownload = null;
        }

        public string Address
        {
            get { return _address; }
        }

        public string Description
        {
            get { return _description; }
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
            List<Song> songs = new List<Song>();

            _logger.Log(LogType.DEBUG, "Attempting to parse html");
            var songList = from body in _doc.DocumentNode.SelectSingleNode("//table[contains(@id, 'now-playing-full')]")?.SelectNodes(".//tbody")
                           from row in body.SelectNodes(".//tr").Cast<HtmlNode>()
                           from artist in row.SelectNodes("(.//td)[2]").Cast<HtmlNode>()
                           from title in row.SelectNodes("(.//td)[3]").Cast<HtmlNode>()
                           from played in row.SelectNodes("(.//td)[4]").Cast<HtmlNode>()
                           select new
                           {
                               Title = title,
                               Artist = artist,
                               Played = played
                           };

            foreach (var song in songList)
            {
                string title, artist;
                DateTime? played;

                //Trim up title
                title = song.Title.InnerHtml;
                int index = title.IndexOf('<');
                title = title.Substring(0, index);

                artist = song.Artist.SelectSingleNode(".//a")?.InnerHtml;
                //Edge case where some artists don't have links
                if (artist == null)
                    artist = song.Artist.InnerHtml;

                //Parse time played into DateTime
                string time = song.Played.InnerHtml.Replace("<br>", string.Empty);
                DateTime parsedDate;
                if (DateTime.TryParseExact(time, "h:mmttMM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out parsedDate))
                    played = parsedDate;
                else //Parsing failed
                {
                    played = null;
                    _logger.Log(LogType.WARN, $"Was unable to parse datetime from string '{time}'. Setting Played to null.");
                }

                songs.Add(new Song() { Title = title, Artist = artist, Played = played });
            }
                        
            return songs;
        }
    }
}