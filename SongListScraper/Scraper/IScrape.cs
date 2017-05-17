using System.Collections.Generic;
using System.Threading.Tasks;

namespace SongListScraper.Scraper
{
    public interface IScrape
    {
        string Description { get; }
        string Address { get; }

        List<Song> ScrapeSongList();
        Task<bool> DownloadPage();
    }
}
