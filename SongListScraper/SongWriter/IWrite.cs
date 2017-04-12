using SongListScraper.Scraper;
using System.Collections.Generic;

namespace SongListScraper.SongWriter
{
    public interface IWrite
    {
        void WriteSongs(List<Song> songs);
    }
}
