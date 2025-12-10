using RestSharp;
using System.Net;

namespace Banned.AniParser.Test.Utils;

internal class TestNetUtils
{
    public static async Task<string> Fetch(string url, bool useProxy = true)
    {
        var client = useProxy
            ? new RestClient(options => { options.Proxy = new WebProxy("http://127.0.0.1:7890"); })
            : new RestClient();
        Console.WriteLine(url);
        var response = await client.ExecuteAsync(new RestRequest(url));
        if (response.StatusCode == HttpStatusCode.OK) return response.Content ?? string.Empty;
        return string.Empty;
    }

    public static async Task<string> DownloadTorrentAsync(
        string             url,
        string             saveDir,
        string             proxyUrl   = "http://127.0.0.1:7890",
        NetworkCredential? credential = null,
        string?            referer    = null,
        CookieContainer?   cookies    = null)
    {
        var uri = new Uri(url);

        var options = new RestClientOptions(uri.GetLeftPart(UriPartial.Authority))
        {
            Proxy = string.IsNullOrWhiteSpace(proxyUrl) ? null : new WebProxy(proxyUrl) { Credentials = credential },
            AutomaticDecompression = DecompressionMethods.All,
            CookieContainer = cookies ?? new CookieContainer(),
            Timeout = TimeSpan.FromSeconds(100)
        };
        using var client = new RestClient(options);

        var req = new RestRequest(uri.PathAndQuery);
        req.AddHeader("Accept", "application/x-bittorrent, application/octet-stream;q=0.9, */*;q=0.8");
        req.AddHeader("User-Agent", "Mozilla/5.0");
        if (!string.IsNullOrEmpty(referer)) req.AddHeader("Referer", referer);

        // .torrent通常不大，直接拿字节数组即可（想省内存可换 DownloadStreamAsync）
        var resp = await client.ExecuteAsync(req);
        if (!resp.IsSuccessful || resp.RawBytes is null)
            throw new Exception($"下载失败：{(int)resp.StatusCode} {resp.StatusDescription}");

        // 从 Content-Disposition 取文件名；没有就用 URL 最后段；最后确保 .torrent 后缀
        var fileName = Path.GetFileName(uri.LocalPath);

        if (string.IsNullOrWhiteSpace(fileName)) fileName = $"{Guid.NewGuid().ToString()}.torrent";
        if (!fileName.EndsWith(".torrent", StringComparison.OrdinalIgnoreCase))
            fileName += ".torrent";

        Directory.CreateDirectory(saveDir);
        var fullPath = Path.Combine(saveDir, fileName);
        await File.WriteAllBytesAsync(fullPath, resp.RawBytes);
        return fullPath;
    }
}
