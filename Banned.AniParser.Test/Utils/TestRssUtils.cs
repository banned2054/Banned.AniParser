using System.Xml.Linq;

namespace Banned.AniParser.Test.Utils;

internal class TestRssUtils
{
    public static List<string> GetAllTitle(string xml)
    {
        // 加载 XML 文件
        var xDoc = XDocument.Parse(xml);

        // 查找所有的 <item> 元素
        var items = xDoc.Descendants("item").ToList();
        return items
              .Where(item => item.Element("title") != null)
              .Select(item => item.Element("title")!.Value)
              .ToList();
    }

    public static List<(string Title, string TorrentUrl)> GetTitlesAndTorrentUrls(string xml)
    {
        var        xDocument = XDocument.Parse(xml);
        XNamespace nsTorrent = "https://mikanani.me/0.1/";

        var items = xDocument.Descendants("item");

        return (from item in items
                let title = (string?)item.Element("title")
                where !string.IsNullOrWhiteSpace(title)
                let enclosureUrl = item.Elements("enclosure")
                                       .Where(e => (string?)e.Attribute("type") == "application/x-bittorrent")
                                       .Select(e => (string?)e.Attribute("url"))
                                       .FirstOrDefault(u => !string.IsNullOrWhiteSpace(u))
                let torrentPageUrl = item.Element(nsTorrent + "torrent")
                                        ?.Element(nsTorrent + "link")
                                        ?.Value
                let fallbackLink = (string?)item.Element("link")
                let url = enclosureUrl ?? torrentPageUrl ?? fallbackLink
                where !string.IsNullOrWhiteSpace(url)
                select (title!, url!)).ToList();
    }
}
