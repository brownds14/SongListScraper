using System;
using System.Collections.ObjectModel;

namespace SongListScraper.UI.WPFApplication.Model
{
    public enum Scrapers
    {
        Station1033Scraper
    }

    public class SongScraperModel
    {
        public ObservableCollection<string> ScraperList { get; }
        public int SelectedScraper { get; set; }
        public bool SelectScraperEnabled { get; set; }
        public bool StartStopServiceEnabled { get; set; }
        public int SelectedSong { get; set; }

        public SongScraperModel()
        {
            SelectedScraper = -1;
            SelectScraperEnabled = true;
            StartStopServiceEnabled = false;
            SelectedSong = -1;

            ScraperList = new ObservableCollection<string>();
            foreach (var s in Enum.GetNames(typeof(Scrapers)))
                ScraperList.Add(s);
        }
    }
}
