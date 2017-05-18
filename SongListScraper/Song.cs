using System;
using System.Text;

namespace SongListScraper
{
    public class Song
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public DateTime? Played { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Played {0} by {1} at {2}.", Title, Artist, Played);
            return sb.ToString();
        }
    }
}
