using System;

namespace SongListScraper.Helpers.TimeProvider
{
    public abstract class TimeProvider
    {
        private static TimeProvider _current = new DefaultTimeProvider();

        public static TimeProvider Current
        {
            get { return _current; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("TimeProvider");
                _current = value;
            }
        }

        public abstract DateTime UtcNow { get; }
    }
}
