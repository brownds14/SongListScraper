using System;
using Xunit;
using SongListScraper.Helpers.Logging;
using SongListScraper.Helpers.Download;
using Moq;

namespace SongListScraper.Tests.Helpers.Download
{
    public class HtmlDownloadTest
    {
        private HtmlDownloader _downloader;

        public HtmlDownloadTest()
        {
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(x => x.Log(It.IsAny<LogType>(), It.IsAny<string>(), It.IsAny<Exception>())).Verifiable();
            _downloader = new HtmlDownloader(mockLogger.Object);
        }

        [Fact]
        public async void InvalidAddress()
        {
            string address = "invalid.address";

            string result = await _downloader.DownloadHtml(address);

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public async void ValidAddress()
        {
            string address = "https://www.google.com";

            string result = await _downloader.DownloadHtml(address);

            //This test can fail if Google's main page changes, but for the most part it should start with this text
            Assert.True(result.StartsWith("<!doctype html>"));
        }
    }
}
