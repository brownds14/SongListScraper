using System.Threading.Tasks;

namespace SongListScraper.Helpers.Download
{
    public interface IDownload
    {
        Task<string> DownloadHtml(string address);
    }
}
