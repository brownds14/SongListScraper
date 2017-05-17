using GalaSoft.MvvmLight;
using Microsoft.Practices.Unity;
using SongListScraper.Helpers.Download;
using SongListScraper.Helpers.Logging;
using SongListScraper.Helpers.SongWriter;
using SongListScraper.Scraper;
using SongListScraper.Settings;
using SongListScraper.UI.WPFApplication.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SongListScraper.UI.WPFApplication.ViewModel
{
    public class SongScraperViewModel : ViewModelBase
    {
        private UnityContainer _container;
        private SettingsConfig _settings;
        private ScrapingService _service = null;
        public ICommand ServiceButton { get; private set; }

        private SongScraperModel _model;
        public SongScraperModel Model
        {
            get { return _model; }
            set { _model = value; }
        }

        public SongScraperViewModel()
        {
            _container = new UnityContainer();

            _settings = new SettingsConfig();
            SettingsConfig.Load(ref _settings);
            _container.RegisterInstance<SettingsConfig>(_settings);

            _container.RegisterType<IDownload, HtmlDownloader>();
            _container.RegisterType<ILogger, Log4NetAdapter>();
            _container.RegisterType<IWrite, WriteSongToFile>();

            _model = new SongScraperModel();
        }

        public void ButtonPressed()
        {
            if (string.Compare(_model.ButtonText, SongScraperModel.StartString) == 0)
            {
                //TODO: change to reflect combobox
                if (_service == null)
                {
                    _container.RegisterType<IScrape, Station1033Scraper>();
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
    }
}