using System.Collections.Generic;
using System.Text;
using SongListScraper.Settings;
using System.IO;

namespace SongListScraper.Helpers.SongWriter
{
    public class WriteSongToFile : IWrite
    {
        private string _delimiter;
        private string _filePath;

        public WriteSongToFile(SettingsConfig settings)
        {
            _delimiter = settings.Delimiter;
            _filePath = settings.SavedSongsFile;
        }

        public void WriteSongs(List<Song> songs)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var song in songs)
            {
                sb.Append(song.Title).Append(_delimiter)
                    .Append(song.Artist).Append(_delimiter)
                    .AppendLine(song.Played.ToString());
            }

            byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());

            using (FileStream fs = new FileStream(_filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
        }
    }
}
