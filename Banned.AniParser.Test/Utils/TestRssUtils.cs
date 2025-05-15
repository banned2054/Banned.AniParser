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
}