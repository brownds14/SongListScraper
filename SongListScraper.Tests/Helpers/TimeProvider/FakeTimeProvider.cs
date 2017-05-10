using System;

namespace SongListScraper.Tests.Helpers.TimeProvider
{
    public class FakeTimeProvider : SongListScraper.Helpers.TimeProvider.TimeProvider
    {
        public DateTime FakeTime;

        public override DateTime UtcNow
        {
            get { return FakeTime; }
        }
    }
}
