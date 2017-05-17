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

namespace SongListScraper.UI.WPFApplication.ViewModel
{
    public class SongScraperViewModel : ViewModelBase
    {
        public SongScraperModel Model { get; set; }

        private UnityContainer _container;
        private SettingsConfig _settings;
        private ScrapingService _service = null;
        public ICommand ServiceButton { get; private set; }

        public SongScraperViewModel()
        {
            _container = new UnityContainer();

            _settings = new SettingsConfig();
            SettingsConfig.Load(ref _settings);
            _container.RegisterInstance<SettingsConfig>(_settings);

            _container.RegisterInstance<SongCallback>(AddSongs);
            _container.RegisterType<IDownload, HtmlDownloader>();
            _container.RegisterType<ILogger, Log4NetAdapter>();

            Model = new SongScraperModel();

            ServiceButton = new RelayCommand(ButtonPressed);
        }

        public void ButtonPressed()
        {
            if (string.Compare(Model.ButtonText, SongScraperModel.StartString) == 0)
            {
                //TODO: change to reflect combobox
                if (_service == null)
                {
                    RegisterScraperToContainer();
                    _service = _container.Resolve<ScrapingService>();
                }
                _service.StartService();
                Model.ButtonText = SongScraperModel.StopString;
                Model.SelectScraperEnabled = false;
            }
            else //_model.ButtonText == SongScraperModel.StopString
            {
                _service.StopService();
                Model.ButtonText = SongScraperModel.StartString;
                Model.SelectScraperEnabled = true;
            }
        }

        public void AddSongs(List<Song> songs)
        {
            foreach (var s in songs)
            {
                Model.SongList.Add(s);
            }
        }

        public void SetScraperDetail()
        {
            RegisterScraperToContainer();
            IScrape scraper = _container.Resolve<IScrape>();
            Model.ScraperDesc = scraper.Description;
            Model.ScraperAddr = scraper.Address;
        }

        public void RegisterScraperToContainer()
        {
            string scraperName = Model.ScraperList[Model.SelectedScraper];
            string[] scraperNames = Enum.GetNames(typeof(Scrapers));
            int index = Array.FindIndex(scraperNames, x => String.Compare(x, scraperName) == 0);
            
            switch ((Scrapers)index)
            {
                case Scrapers.Station1033Scraper:
                    _container.RegisterType<IScrape, Station1033Scraper>();
                    break;
            }
        }

        public override void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.RaisePropertyChanged(propertyName);

            switch (propertyName)
            {
                case "SelectedScraper":
                    SetScraperDetail();
                    break;
            }
        }
    }
}