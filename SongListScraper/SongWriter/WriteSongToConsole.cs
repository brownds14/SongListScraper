﻿using System;
using System.Collections.Generic;
using System.Text;
using SongListScraper.Scraper;

namespace SongListScraper.SongWriter
{
    internal class WriteSongToConsole : IWrite
    {
        public void WriteSongs(List<Song> songs)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var song in songs)
            {
                sb.Append(song.Title).Append(" ")
                    .Append(song.Artist).Append(" ")
                    .AppendLine(song.Played.ToString());
            }

            Console.Write(sb.ToString());
        }
    }
}
