using SongListScraper.Settings;

namespace SongListScraper.SongWriter
{
    public static class SongWriterManager
    {
        private static IWrite _writer = null;

        public static IWrite CreateWriter(SettingsConfig settings)
        {
            if (_writer == null)
            {
                switch (settings.StorageType)
                {
                    case SongStorage.FILE:
                        _writer = new WriteSongToFile(settings);
                        break;
                    case SongStorage.CONSOLE:
                        _writer = new WriteSongToConsole();
                        break;
                }
            }

            return _writer;
        }

        public static IWrite GetInstance()
        {
            return _writer;
        }
    }
}
