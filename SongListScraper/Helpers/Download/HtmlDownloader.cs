using System;
using System.Net.Http;
using System.Threading.Tasks;
using SongListScraper.Helpers.Logging;

namespace SongListScraper.Helpers.Download
{
    public class HtmlDownloader : IDownload
    {
        private HttpClient _client;
        private ILogger _logger;
        
        public HtmlDownloader(ILogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException("logger");

            _logger = logger;
            _client = new HttpClient();
        }

        public async Task<string> DownloadHtml(string address)
        {
            string html = string.Empty;

            _logger.Log(LogType.INFO, "Attempting to download html");
            HttpResponseMessage msg = await _client.GetAsync(address);

            if (msg.IsSuccessStatusCode)
            {
                _logger.Log(LogType.INFO, "Html download was successful");
                html = await msg.Content.ReadAsStringAsync();
            }
            else
            {
                _logger.Log(LogType.WARN, $"Failed to retrieve webpage. Status code: {msg.StatusCode}");
            }

            return html;
        }
    }
}
