using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SongListScraper.UI.WPFApplication.Model
{
    public enum Scrapers
    {
        Station1033Scraper
    }

    public class SongScraperModel : INotifyPropertyChanged
    {
        public static readonly string StartString = "Start Service";
        public static readonly string StopString = "Stop Service";

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> ScraperList { get; }
        public int SelectedScraper { get; set; }
        public bool SelectScraperEnabled { get; set; }

        public string ScraperDesc { get; set; }
        public string ScraperAddr { get; set; }

        public string ButtonText { get; set; }

        public ObservableCollection<Song> SongList { get; set; }
        public int SelectedSong { get; set; }

        public string SongTitle { get; set; }
        public string SongArtist { get; set; }
        public string SongPlayed { get; set; }

        public SongScraperModel()
        {
            ScraperList = new ObservableCollection<string>();
            foreach (var s in Enum.GetNames(typeof(Scrapers)))
                ScraperList.Add(s);
            SelectedScraper = 0;
            SelectScraperEnabled = true;

            ScraperDesc = string.Empty;
            ScraperAddr = string.Empty;

            ButtonText = StartString;

            SongList = new ObservableCollection<Song>();
            SelectedSong = -1;

            SongTitle = string.Empty;
            SongArtist = string.Empty;
            SongPlayed = string.Empty;
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            var tmp = PropertyChanged;
            if (tmp != null)
            {
                tmp(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
