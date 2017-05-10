using System;
using Moq;
using Xunit;
using SongListScraper.Helpers.Logging;
using SongListScraper.Helpers.Download;
using SongListScraper.Scraper;
using SongListScraper.Tests.Helpers.TimeProvider;
using SongListScraper.Helpers.TimeProvider;
using System.Reflection;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SongListScraper.Tests.Scrapers
{
    public class Station1033ScraperTests
    {
        private string _testHtml;
        private Mock<ILogger> _mockLogger;
        private Mock<IDownload> _mockDownload;
        private FakeTimeProvider _fakeTime;

        public Station1033ScraperTests()
        {
            //Setup mock ILogger
            _mockLogger = new Mock<ILogger>();
            _mockLogger.Setup(x => x.Log(It.IsAny<LogType>(), It.IsAny<string>(), It.IsAny<Exception>())).Verifiable();

            //Load example html into string
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = "SongListScraper.Tests.Scrapers.Station1033HtmlExample.html";

            string[] names = assembly.GetManifestResourceNames();

            using (Stream s = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(s))
                    _testHtml = reader.ReadToEnd();
            }

            //Setup mock IDownload
            Task<string> htmlTask = new Task<string>(() => _testHtml);
            htmlTask.Start();
            _mockDownload = new Mock<IDownload>();
            _mockDownload.Setup(x => x.DownloadHtml(It.IsAny<string>())).Returns(htmlTask);

            //Setup a fake TimeProvider
            DateTime testDate = new DateTime(2016, 6, 14);
            _fakeTime = new FakeTimeProvider() { FakeTime = testDate };
            TimeProvider.Current = _fakeTime;
        }

        [Fact]
        public async void SuccessfulDownloadReturnsTrue()
        {
            var scraper = new Station1033Scraper(_mockDownload.Object, _mockLogger.Object);

            bool result = await scraper.DownloadPage();

            Assert.True(result);
        }

        [Fact]
        public async void ExcessiveDownloadsThrowsError()
        {
            var scraper = new Station1033Scraper(_mockDownload.Object, _mockLogger.Object);

            bool result = await scraper.DownloadPage();
            var exception = await Record.ExceptionAsync(async () => await scraper.DownloadPage());

            Assert.IsType(typeof(ExcessiveDownloadException), exception);
        }

        [Fact]
        public async void MultipleDownloadSuccess()
        {
            var scraper = new Station1033Scraper(_mockDownload.Object, _mockLogger.Object);
            bool result = await scraper.DownloadPage();

            _fakeTime.FakeTime = _fakeTime.UtcNow.AddMinutes(11);
            TimeProvider.Current = _fakeTime;
            result = await scraper.DownloadPage();

            Assert.True(result);
        }

        [Fact]
        public async void NoSongsParsedFromBadHtml()
        {
            Task<string> returnEmpty = new Task<string>(() => string.Empty);
            returnEmpty.Start();
            _mockDownload.Setup(x => x.DownloadHtml(It.IsAny<string>())).Returns(returnEmpty);

            var scraper = new Station1033Scraper(_mockDownload.Object, _mockLogger.Object);
            bool result = await scraper.DownloadPage();

            List<Song> songs = scraper.ScrapeSongList();

            Assert.Equal(0, songs.Count);
        }

        [Fact]
        public async void SongsSuccessfullyParsed()
        {
            var scraper = new Station1033Scraper(_mockDownload.Object, _mockLogger.Object);
            bool result = await scraper.DownloadPage();

            List<Song> songs = scraper.ScrapeSongList();

            bool success = true;
            for (int i = 0; i < songs.Count; ++i)
            {
                if (songs[i].Title != $"Song{i}_Title")
                    success = false;

                if (songs[i].Artist != $"Song{i}_Artist")
                    success = false;

                DateTime expectedTime;
                if (i == 0)
                    expectedTime = TimeProvider.Current.UtcNow.AddHours(4).AddMinutes(15);
                else //i == 1
                    expectedTime = TimeProvider.Current.UtcNow.AddDays(-1).AddHours(23).AddMinutes(9);

                if (songs[i].Played != expectedTime)
                    success = false;
            }

            Assert.True(success);
        }
    }
}
