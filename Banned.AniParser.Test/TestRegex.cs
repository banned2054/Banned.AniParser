using Banned.AniParser.Test.Utils;
using MonoTorrent;
using System.Text.RegularExpressions;

namespace Banned.AniParser.Test;

internal class TestRegex
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestRegexWithOneString()
    {
        var parser = new
            Regex(@"\[樱桃花字幕组](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\s?[\(（]?(?<resolution>\d+p)[\)）]?\s?\[[a-z0-9]+]",
                  RegexOptions.IgnoreCase);
        parser = new
            Regex(@"\[樱桃花字幕组](?<title>[^\[\]]+?)-\s?(?<episode>\d+)(?:v(?<version>\d+))?\s?[\(（]?(?<resolution>\d+p)[\)）]?\s?\[[a-z0-9]+]",
                  RegexOptions.IgnoreCase);
        var testStrList = new List<string>
        {
            "[樱桃花字幕组]夏日口袋  Summer Pockets - 15(1080p) [简日双语]"
        };
        foreach (var testStr in testStrList)
        {
            var match = parser.Match(testStr);
            if (!match.Success) continue;
            Console.WriteLine(testStr);
            foreach (Group group in match.Groups)
            {
                Console.WriteLine($"\t{group.Name}:{group.Value}");
            }
        }
    }

    [Test]
    public async Task TestByTorrentFile()
    {
        var aniParser = new AniParser();
        // 读取 .torrent 文件
        var torrent =
            await
                Torrent.LoadAsync(@"D:\Downloads\[U2].42576.torrent");

        // 多文件 torrent
        if (torrent.Files.Count > 0)
        {
            foreach (var file in torrent.Files)
            {
                var result = aniParser.Parse(file.Path);
                TestPrintUtils.PrintParserInfo(result, file.Path);
            }
        }
        else
        {
            var result = aniParser.Parse(torrent.Name);
            TestPrintUtils.PrintParserInfo(result, torrent.Name);
        }
    }
}
