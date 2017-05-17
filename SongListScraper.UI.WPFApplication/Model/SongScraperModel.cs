using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;

namespace SongListScraper.UI.WPFApplication.Model
{
    public enum Scrapers
    {
        Station1033Scraper
    }

    public class SongScraperModel : ObservableObject
    {
        public static readonly string StartString = "Start Service";
        public static readonly string StopString = "Stop Service";

        private ObservableCollection<string> _scraperList;
        public ObservableCollection<string> ScraperList
        {
            get { return _scraperList; }
            private set { Set<ObservableCollection<string>>(() => this.ScraperList, ref _scraperList, value); }
        }

        public int _selectScraper;
        public int SelectedScraper
        {
            get { return _selectScraper; }
            set { Set<int>(() => this.SelectedScraper, ref _selectScraper, value); }
        }

        private bool _selectScraperEnabled;
        public bool SelectScraperEnabled
        {
            get { return _selectScraperEnabled; }
            set { Set<bool>(() => this.SelectScraperEnabled, ref _selectScraperEnabled, value); }
        }

        private string _scraperDesc;
        public string ScraperDesc
        {
            get { return _scraperDesc; }
            set { Set<string>(() => this.ScraperDesc, ref _scraperDesc, value); }
        }

        private string _scraperAddr;
        public string ScraperAddr
        {
            get { return _scraperAddr; }
            set { Set<string>(() => this.ScraperAddr, ref _scraperAddr, value); }
        }

        private string _buttonText;
        public string ButtonText
        {
            get { return _buttonText; }
            set { Set<string>(() => this.ButtonText, ref _buttonText, value); }
        }

        private ObservableCollection<Song> _songList;
        public ObservableCollection<Song> SongList
        {
            get { return _songList; }
            set { Set<ObservableCollection<Song>>(() => this.SongList, ref _songList, value); }
        }

        private int _selectedSong;
        public int SelectedSong
        {
            get { return _selectedSong; }
            set { Set<int>(() => this.SelectedSong, ref _selectedSong, value); }
        }

        private string _songTitle;
        public string SongTitle
        {
            get { return _songTitle; }
            set { Set<string>(() => this.SongTitle, ref _songTitle, value); }
        }

        private string _songArtist;
        public string SongArtist
        {
            get { return _songArtist; }
            set { Set<string>(() => this.SongArtist, ref _songArtist, value); }
        }

        private string _songPlayed;
        public string SongPlayed
        {
            get { return _songPlayed; }
            set { Set<string>(() => this.SongPlayed, ref _songPlayed, value); }
        }

        public SongScraperModel()
        {
            ScraperList = new ObservableCollection<string>();
            foreach (var s in Enum.GetNames(typeof(Scrapers)))
                ScraperList.Add(s);
            ScraperList.Add("tmpScraper");
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
    }
}
