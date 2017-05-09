using System.Collections.Generic;
using System.Threading.Tasks;

namespace SongListScraper.Scraper
{
    public interface IScrape
    {
        List<Song> ScrapeSongList();
        Task<bool> DownloadPage();
    }
}
