using System.Collections.Generic;

namespace SongListScraper.Helpers.SongWriter
{
    public interface IWrite
    {
        void WriteSongs(List<Song> songs);
    }
}
