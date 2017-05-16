using GalaSoft.MvvmLight;
using SongListScraper.UI.WPFApplication.Model;
using System.Collections.ObjectModel;

namespace SongListScraper.UI.WPFApplication.ViewModel
{

    public class SongScraperViewModel : ViewModelBase
    {
        private SongScraperModel _model;
        private ObservableCollection<Song> _songList;

        public SongScraperModel Model
        {
            get { return _model; }
            set { _model = value; }
        }

        public ObservableCollection<Song> SongList
        {
            get { return _songList; }
            set { _songList = value; }
        }

        public SongScraperViewModel()
        {
            _model = new SongScraperModel();
            _songList = new ObservableCollection<Song>();
        }
    }
}