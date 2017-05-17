using GalaSoft.MvvmLight;

namespace SongListScraper.UI.WPFApplication.Model
{
    public class ServiceButton : ObservableObject
    {
        public static readonly string StartString = "Start Service";
        public static readonly string StopString = "Stop Service";

        private string _buttonText;
        public string ButtonText
        {
            get { return _buttonText; }
            set { Set<string>(() => this.ButtonText, ref _buttonText, value); }
        }
    }
}
