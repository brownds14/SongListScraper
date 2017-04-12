﻿using System;
using System.Text;

namespace SongListScraper.Scraper
{
    public class Song
    {
        public String Title { get; set; }
        public String Artist { get; set; }
        public DateTime Played { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Played {0} by {1} at {2}.", Title, Artist, Played);
            return sb.ToString();
        }
    }
}
