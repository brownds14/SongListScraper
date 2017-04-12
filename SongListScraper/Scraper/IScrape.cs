using System.Collections.Generic;
using System.Threading.Tasks;

namespace SongListScraper.Scraper
{
    public interface IScrape
    {
        Task<bool> DownloadPage();
        List<Song> ScrapeSongList();
    }
}
