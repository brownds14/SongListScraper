using System.Collections.Generic;

namespace SongListScraper.Scraper
{
    public interface IScrape
    {
        List<Song> ScrapeSongList();
        void DownloadPage();
    }
}
