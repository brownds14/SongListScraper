using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.Unity;
using SongListScraper.Helpers.Download;
using SongListScraper.Helpers.Logging;
using SongListScraper.Scraper;
using SongListScraper.Settings;
using SongListScraper.UI.WPFApplication.Model;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using static SongListScraper.UI.WPFApplication.Model.Enums;

namespace SongListScraper.UI.WPFApplication.ViewModel
{
    public class SongScraperViewModel : ViewModelBase
    {
        private UnityContainer _container;
        private SettingsConfig _settings;
        private ScrapingService _service = null;
        public ICommand CmdServiceButton { get; private set; }

        #region BindingProperties
        private ObservableCollection<string> _scraperList;
        public ObservableCollection<string> ScraperList
        {
            get { return _scraperList; }
            private set { Set(() => ScraperList, ref _scraperList, value); }
        }

        public int _selectScraper;
        public int SelectedScraper
        {
            get { return _selectScraper; }
            set
            {
                Set<int>(() => this.SelectedScraper, ref _selectScraper, value);
                SetScraperDetail();
            }
        }

        private bool _selectScraperEnabled;
        public bool SelectScraperEnabled
        {
            get { return _selectScraperEnabled; }
            set { Set<bool>(() => this.SelectScraperEnabled, ref _selectScraperEnabled, value); }
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
            set
            {
                Set<int>(() => this.SelectedSong, ref _selectedSong, value);
                if (_selectedSong >= 0)
                    CurrSong = SongList[_selectedSong];
            }
        }

        private Song _currSong;
        public Song CurrSong
        {
            get { return _currSong; }
            set { Set<Song>(() => this.CurrSong, ref _currSong, value); }
        }

        private ScraperDetails _scraperDetails;
        public ScraperDetails ScraperDetails
        {
            get { return _scraperDetails; }
            set { Set<ScraperDetails>(() => this.ScraperDetails, ref _scraperDetails, value); }
        }

        public ServiceButton Button { get; set; }
        #endregion

        public SongScraperViewModel()
        {
            //Setup Unity Container
            _container = new UnityContainer();

            _settings = new SettingsConfig();
            SettingsConfig.Load(ref _settings);
            _container.RegisterInstance<SettingsConfig>(_settings);

            _container.RegisterInstance<SongCallback>(AddSongs);
            _container.RegisterType<IDownload, HtmlDownloader>();
            _container.RegisterType<ILogger, Log4NetAdapter>();

            //Setup commands
            CmdServiceButton = new RelayCommand(ButtonPressed);

            //Setup start values
            ScraperList = new ObservableCollection<string>();
            foreach (var s in Enum.GetNames(typeof(Scrapers)))
                ScraperList.Add(s);
            SelectedScraper = 0;
            SelectScraperEnabled = true;
            SongList = new ObservableCollection<Song>();
            SelectedSong = -1;

            Button = new ServiceButton() { ButtonText = ServiceButton.StartString };
        }

        public void ButtonPressed()
        {
            if (string.Compare(Button.ButtonText, ServiceButton.StartString) == 0)
            {
                if (_service == null)
                {
                    RegisterScraperToContainer();
                    _service = _container.Resolve<ScrapingService>();
                }
                _service.StartService();
                Button.ButtonText = ServiceButton.StopString;
                SelectScraperEnabled = false;
            }
            else //_model.ButtonText == SongScraperModel.StopString
            {
                _service.StopService();
                Button.ButtonText = ServiceButton.StartString;
                SelectScraperEnabled = true;
            }
        }

        public void AddSongs(List<Song> songs)
        {
            foreach (var s in songs)
            {
                SongList.Add(s);
            }
        }

        public void SetScraperDetail()
        {
            RegisterScraperToContainer();
            IScrape scraper = _container.Resolve<IScrape>();
            var details = new ScraperDetails() { Description = scraper.Description, Address = scraper.Address };
            ScraperDetails = details;
        }

        public void RegisterScraperToContainer()
        {
            string scraperName = ScraperList[SelectedScraper];
            string[] scraperNames = Enum.GetNames(typeof(Scrapers));
            int index = Array.FindIndex(scraperNames, x => String.Compare(x, scraperName) == 0);
            
            switch ((Scrapers)index)
            {
                case Scrapers.Station1033Scraper:
                    _container.RegisterType<IScrape, Station1033Scraper>();
                    break;
            }
        }
    }
}