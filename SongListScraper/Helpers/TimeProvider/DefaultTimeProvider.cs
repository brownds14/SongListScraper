using System;

namespace SongListScraper.Helpers.TimeProvider
{
    public class DefaultTimeProvider : TimeProvider
    {
        public override DateTime UtcNow
        {
            get { return DateTime.UtcNow; }
        }
    }
}
