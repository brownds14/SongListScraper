using System;

namespace SongListScraper.Scraper
{
    public class ExcessiveDownloadException : Exception
    {
        public ExcessiveDownloadException() { }

        public ExcessiveDownloadException(string msg) : base(msg) { }

        public ExcessiveDownloadException(string msg, Exception inner) : base(msg, inner) { }
    }
}
